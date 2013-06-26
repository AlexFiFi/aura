// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Aura.Shared.Const;
using Aura.Data;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.Util;
using Aura.World.World;
using csscript;
using CSScriptLibrary;
using System.Globalization;

namespace Aura.World.Scripting
{
	public class ScriptManager
	{
		public readonly static ScriptManager Instance = new ScriptManager();
		static ScriptManager() { }
		private ScriptManager() { }

		private int _loadedScripts, _cached;

		private Dictionary<string, Dictionary<string, List<ScriptHook>>> _hooks = new Dictionary<string, Dictionary<string, List<ScriptHook>>>();

		private Dictionary<uint, ItemScript> _itemScripts = new Dictionary<uint, ItemScript>();
		private Dictionary<uint, QuestScript> _questScripts = new Dictionary<uint, QuestScript>();

		public void LoadScripts()
		{
			CSScript.GlobalSettings.HideCompilerWarnings = true;
			CSScript.CacheEnabled = false;

			Logger.Info("Loading scripts...");

			_loadedScripts = _cached = 0;

			// Read script list
			var listPath = Path.Combine(WorldConf.ScriptPath, "scripts.txt");
			var npcListContents = new List<string>();

			if (File.Exists(listPath))
			{
				try
				{
					npcListContents = FileReader.GetAllLines(listPath);
				}
				catch (Exception ex)
				{
					Logger.Warning("Unable to fully parse " + listPath + ". Error: " + ex.Message);
				}
			}
			else
			{
				Logger.Warning("Unable to find script list at '" + listPath + "'.");
			}

			var scriptFiles = npcListContents.ToArray();

			// Check how much is to be cached
			var cacheDir = Path.Combine(WorldConf.ScriptPath, "cache", "npc");
			int cached = 0;
			if (Directory.Exists(cacheDir))
				cached = Directory.GetFiles(Path.Combine(WorldConf.ScriptPath, "cache", "npc"), "*.compiled", SearchOption.AllDirectories).Length;
			int notCached = scriptFiles.Length - cached;

			if (notCached > 30)
				Logger.Info("Caching the scripts may take a few minutes initially.");

			_hooks.Clear();
			_questScripts.Clear();

			// Load scripts
			int loaded = 0;
			foreach (var line in scriptFiles)
			{
				var file = line;
				bool virt = false;
				if (file.StartsWith("virtual:"))
				{
					virt = true;
					file = file.Replace("virtual:", "").Trim();
				}
				this.LoadScript(Path.Combine(WorldConf.ScriptPath, file), virt);

				// Don't print for every single NPC, so much flickering.
				if (++loaded % 5 == 0 || loaded == 1 || loaded >= scriptFiles.Length)
				{
					int loadPercBar = (int)(20f / scriptFiles.Length * loaded);
					var bar = "[" + "".PadLeft(loadPercBar, '#') + "".PadLeft(20 - loadPercBar, '.') + "]";

					Logger.Info(string.Format("{1} {0,6}%", (100f / scriptFiles.Length * loaded).ToString("0.0", CultureInfo.InvariantCulture), bar), false);
					Logger.RLine(); // Move cursor to pos 0, so error msgs can overwrite this line.
				}
			}

			// fin~
			Logger.ClearLine();
			Logger.Info("Done loading " + _loadedScripts + " scripts.");
			if (!WorldConf.DisableScriptCaching)
				Logger.Info("Cached " + _cached + " scripts.");

			LoadItemScripts();
		}

		private void LoadItemScripts()
		{
			_itemScripts.Clear();

			Logger.Info("Loading item scripts...");

			var tmpPath = Path.Combine(WorldConf.ScriptPath, "cache", "item", "inline.generated.cs");
			var compiledPath = Path.ChangeExtension(tmpPath, "compiled");

			// Create folders
			string d;
			if (!Directory.Exists(d = Path.Combine(WorldConf.ScriptPath, "cache")))
				Directory.CreateDirectory(d);
			if (!Directory.Exists(d = Path.Combine(WorldConf.ScriptPath, "cache", "item")))
				Directory.CreateDirectory(d);

			var updateInline = (File.GetLastWriteTime(Path.Combine(WorldConf.DataPath, "db", "items.txt")) >= File.GetLastWriteTime(compiledPath));

			// Re-generate/compile if something has changed
			var sb = new StringBuilder();

			// Default usings
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Collections;");
			sb.AppendLine("using Aura.Shared.Const;");
			sb.AppendLine("using Aura.World.Network;");
			sb.AppendLine("using Aura.World.Scripting;");
			sb.AppendLine("using Aura.World.World;");
			sb.AppendLine("using Aura.Shared.Util;");
			sb.AppendLine();

			foreach (var entry in MabiData.ItemDb.Entries.Values)
			{
				// Look for a default script (script/item/<id>.cs) if everything is empty
				var defaulting = false;
				if (string.IsNullOrWhiteSpace(entry.OnUse) && string.IsNullOrWhiteSpace(entry.OnEquip) && string.IsNullOrWhiteSpace(entry.OnUnequip))
				{
					defaulting = true;
					entry.OnUse = string.Format("use(\"{0}.cs\");", entry.Id);
				}

				// Load include scripts directly
				var match = Regex.Match(entry.OnUse, @"^\s*use\s*\(\s*""([^\)]+)""\s*\)\s*;?\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
				if (match.Success)
				{
					var path = Path.Combine(WorldConf.ScriptPath, "item", match.Groups[1].Value);
					if (!File.Exists(path))
					{
						if (!defaulting)
							Logger.Warning("Item script not found: {0}", Path.GetFileName(path));
					}
					else
					{
						var scriptAsm = this.GetScript(path);
						if (scriptAsm != null)
						{
							var iscr = scriptAsm.CreateObject("*") as ItemScript;
							if (iscr == null)
							{
								Logger.Warning("Not an item script in '{0}'.", path);
								continue;
							}

							_itemScripts[entry.Id] = iscr;
						}
					}

					// Reset, to avoid warning on reload.
					if (defaulting)
						entry.OnUse = string.Empty;

					continue;
				}

				if (updateInline)
				{
					// Add inline scripts to the collection,
					// wrapped in an ItemScript class.
					sb.AppendFormat("public class ItemScript{0} : ItemScript {{" + Environment.NewLine, entry.Id);
					{
						if (!string.IsNullOrWhiteSpace(entry.OnUse))
							sb.AppendFormat("	public override void OnUse(MabiCreature cr, MabiItem i)     {{ {0} }}" + Environment.NewLine, entry.OnUse.Trim());
						if (!string.IsNullOrWhiteSpace(entry.OnEquip))
							sb.AppendFormat("	public override void OnEquip(MabiCreature cr, MabiItem i)   {{ {0} }}" + Environment.NewLine, entry.OnEquip.Trim());
						if (!string.IsNullOrWhiteSpace(entry.OnUnequip))
							sb.AppendFormat("	public override void OnUnequip(MabiCreature cr, MabiItem i) {{ {0} }}" + Environment.NewLine, entry.OnUnequip.Trim());
					}
					sb.AppendFormat("}}" + Environment.NewLine + Environment.NewLine);
				}
			}

			if (updateInline)
			{
				using (var sw = new StreamWriter(tmpPath))
					sw.WriteLine(sb);
			}

			// Load compiled file
			var ilScriptAsm = this.GetScript(tmpPath, compiledPath);
			if (ilScriptAsm == null)
			{
				Logger.Info("Failed to load inline item scripts.");
				return;
			}

			var types = ilScriptAsm.GetTypes().Where(t => t.IsSubclassOf(typeof(ItemScript)));
			foreach (var type in types)
			{
				var scriptObj = Activator.CreateInstance(type) as ItemScript;
				if (scriptObj == null)
					continue;

				var itemId = Convert.ToUInt32(type.Name.Replace("ItemScript", ""));
				_itemScripts[itemId] = scriptObj;
			}

			Logger.Info("Done loading item scripts.");
		}

		/// <summary>
		/// Returns the ItemScript object for the item id,
		/// or null if no script was found.
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public ItemScript GetItemScript(uint itemId)
		{
			ItemScript result;
			_itemScripts.TryGetValue(itemId, out result);
			return result;
		}

		/// <summary>
		/// Loads the script file at the given path and adds the NPC to the world.
		/// </summary>
		/// <param name="path"></param>
		private void LoadScript(string scriptPath, bool virtualLoadFile = false)
		{
			try
			{
				var fileName = Path.GetFileName(scriptPath);
				if (fileName.StartsWith("_"))
					virtualLoadFile = true;

				var cleanScriptPath = scriptPath.Trim('.', '/').Replace("\\", "/");

				// Load assembly and loop through the defined classes.
				var scriptAsm = this.GetScript(scriptPath);
				if (scriptAsm == null)
					return;

				var types = scriptAsm.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseScript)));
				foreach (var type in types)
				{
					if (type.IsAbstract)
						continue;

					var sType = type.ToString();

					// Skip stuff generated by the compiler.
					if (sType.Contains("+") || sType.Contains("<"))
						continue;

					var virtualLoadType = false;
					if (sType.StartsWith("_"))
						virtualLoadType = true;

					// Create object from loaded assembly.
					//var scriptObj = scriptAsm.CreateObject(sType);
					var scriptObj = Activator.CreateInstance(type);

					// Check if the object is derived from NPCScript. Needed to
					// allow simpler scripts that only derive from BaseScript.
					if (scriptObj is NPCScript)
					{
						var script = scriptObj as NPCScript;
						script.ScriptPath = scriptPath;
						script.ScriptName = fileName;
						if (!virtualLoadFile && !virtualLoadType)
						{
							// New NPC with some defaults, so we don't crash if
							// somebody forgot something.
							var npc = new MabiNPC();
							npc.Name = "_undefined";
							npc.Race = 190140;

							npc.Script = script;
							npc.ScriptPath = cleanScriptPath;
							script.ScriptName = Path.GetFileName(scriptPath);
							script.NPC = npc;
							script.LoadType = NPCLoadType.Real;
							script.OnLoad();
							script.OnLoadDone();

							// Only load defaults if race is set.
							if (npc.Race != 0 && npc.Race != uint.MaxValue)
								npc.LoadDefault();

							WorldManager.Instance.AddCreature(npc);
						}
						//else
						//{
						//    script.LoadType = NPCLoadType.Virtual;
						//}
					}
					else if (scriptObj is QuestScript)
					{
						var script = scriptObj as QuestScript;
						script.ScriptPath = cleanScriptPath;
						script.ScriptName = Path.GetFileName(scriptPath);
						script.OnLoad();
						script.OnLoadDone();

						if (MabiData.QuestDb.Entries.ContainsKey(script.Info.Id))
							Logger.Warning("Double quest id '{0}', overwriting from '{1}'.", script.Info.Id, cleanScriptPath);

						MabiData.QuestDb.Entries[script.Id] = script.Info;
						_questScripts[script.Id] = script;
					}
					else if (scriptObj is BaseScript)
					{
						// Script that doesn't use an NPC or anything, like prop scripts and stuff.

						var script = scriptObj as BaseScript;
						script.ScriptPath = cleanScriptPath;
						script.ScriptName = Path.GetFileName(scriptPath);

						script.OnLoad();
						script.OnLoadDone();
					}
					else
					{
						// Type doesn't derive from NPCScript or BaseScript,
						// probably a custom class. Ignore.
						//Logger.Warning("Unknown script class: " + sType);
					}
				}

				Interlocked.Increment(ref _loadedScripts);
			}
			catch (Exception ex)
			{
				try
				{
					File.Delete(this.GetCompiledPath(scriptPath));
				}
				catch { }
				try
				{
					// "Touch" the file, to mark it for recompilation
					// (due to type load errors with inherited scripts)
					new FileInfo(scriptPath).LastWriteTime = DateTime.Now;
				}
				catch { }

				scriptPath = scriptPath.Replace(WorldConf.ScriptPath, "").Replace('\\', '/').TrimStart('/');

				Logger.Error("While loading '{0}': {1}\n{2}", scriptPath, ex.Message, ex.StackTrace);
			}
		}

		/// <summary>
		/// Adds "cache" to the given path.
		/// Example: data/script/folder/script.cs > data/script/cache/folder/script.cs
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		private string GetCompiledPath(string filePath)
		{
			return Path.ChangeExtension(filePath.Replace(WorldConf.ScriptPath, Path.Combine(WorldConf.ScriptPath, "cache")), ".compiled");
		}

		/// <summary>
		/// Loads the given script from the cache, or recaches it,
		/// if necessary. Returns script assembly.
		/// </summary>
		/// <param name="scriptPath"></param>
		/// <returns></returns>
		public Assembly GetScript(string scriptPath, string compiledPath = null)
		{
			if (compiledPath == null)
				compiledPath = this.GetCompiledPath(scriptPath);

			Assembly asm = null;
			try
			{
				if (!WorldConf.DisableScriptCaching && File.Exists(compiledPath) && File.GetLastWriteTime(compiledPath) >= File.GetLastWriteTime(scriptPath))
				{
					asm = Assembly.LoadFrom(compiledPath);
				}
				else
				{
					asm = CSScript.LoadCode(this.PreCompileScript(scriptPath));
					if (!WorldConf.DisableScriptCaching)
					{
						try
						{
							if (File.Exists(compiledPath))
								File.Delete(compiledPath);
							else
								Directory.CreateDirectory(Path.GetDirectoryName(compiledPath));

							File.Copy(asm.Location, compiledPath);

							Interlocked.Increment(ref _cached);
						}
						catch (UnauthorizedAccessException)
						{
							// Thrown if file can't be copied. Happens if script was
							// initially loaded from cache.
						}
						catch (Exception ex)
						{
							Logger.Warning(ex.Message);
						}
					}
				}
			}
			catch (CompilerException ex)
			{
				var errors = ex.Data["Errors"] as CompilerErrorCollection;
				var lines = File.ReadAllLines(scriptPath);

				scriptPath = scriptPath.Replace(WorldConf.ScriptPath, "").Replace('\\', '/').TrimStart('/');

				foreach (CompilerError err in errors)
				{
					// Error msg
					Logger.Error("In {0} on line {1}", scriptPath, err.Line - 1);
					Logger.Write("          " + err.ErrorText);

					// Display lines around the error
					int startIdx = err.Line - 2;
					if (startIdx < 1)
						startIdx = 1;
					else if (startIdx + 3 > lines.Length)
						startIdx = lines.Length - 2;
					for (int i = startIdx; i < startIdx + 3; ++i)
						Logger.Write("  {2} {0:0000}: {1}", i, lines[i - 1], (err.Line - 1 == i ? '*' : ' '));
				}
			}

			return asm;
		}

		private string PreCompileScript(string filePath)
		{
			var file = File.ReadAllText(filePath);
			var sb = new StringBuilder();

			if (WorldConf.ScriptStrictMode)
			{
				sb.Append("//css_co /warnaserror+ /warn:4;\r\n");
			}
			else
			{
				sb.Append("//css_co /warnaserror- /warn:0;\r\n");
			}

			// Usings
			{
				// Default:
				// using System;
				// using Common.Constants;
				// using Common.World;
				// using World.Network;
				// using World.Scripting;
				// using World.World;

				//if (!Regex.Match(file, @"(^|;)\s*using System;").Success) sb.Append("using System;");
				//if (!Regex.Match(file, @"(^|;)\s*using Common.Constants;").Success) sb.Append("using Common.Constants;");
				//if (!Regex.Match(file, @"(^|;)\s*using Common.World;").Success) sb.Append("using Common.World;");
				//if (!Regex.Match(file, @"(^|;)\s*using World.Network;").Success) sb.Append("using World.Network;");
				//if (!Regex.Match(file, @"(^|;)\s*using World.Scripting;").Success) sb.Append("using World.Scripting;");
				//if (!Regex.Match(file, @"(^|;)\s*using World.World;").Success) sb.Append("using World.World;");
			}

			// Wait, End, SubTalk
			{
				// [var] <variable> = Wait();
				// --> [var] <variable>Object = new Response(); yield return <variable>Object; [var] <variable> = <variable>Object.Value;
				//file = Regex.Replace(file,
				//    @"([\{\}:;\t ])(var )?[\t ]*([^\s\)]*)\s*=\s*Wait\s*\(\s*([^)]*)\s*\)\s*;",
				//    "$1$2$3Object = new Response(); yield return $3Object; $2$3 = $3Object.Value;",
				//    RegexOptions.Compiled);
				file = Regex.Replace(file,
					@"([\{\}:;\t ])?(var )?[\t ]*([^\s\)]*)\s*=\s*([^\.]\.)?Select\s*\(\s*([^)]*)\s*\)\s*;",
					"$1$4Select($5); $2$3Object = new Response(); yield return $3Object; $2$3 = $3Object.Value;",
					RegexOptions.Compiled);

				// (obsolete)End(); / Result();
				// --> yield break;
				file = Regex.Replace(file,
					@"([\{\}:;\t ])?(End|Return)\s*\(\s*\)\s*;",
					"yield break;",
					RegexOptions.Compiled);

				// Sub(Talk|Action)(<method_call>);
				// --> foreach(var __subTalkResponse in <method_call>) yield return __sub(Talk|Action)Response;
				file = Regex.Replace(file,
					@"([\{\}:;\t ])Sub(Talk|Action)\s*\(([^;]*)\)\s*;",
					"$1foreach(var __subTalkResponse in $3) yield return __subTalkResponse;",
					RegexOptions.Compiled);

				// Hook(<client>, <"hook">);
				// --> foreach(var __hook in ScriptManager.Instance.GetHooks(this.NPC.Name, <"hook">) foreach(var __subTalkResponse in __hook(<client>, this)) yield return __subTalkResponse;
				file = Regex.Replace(file,
					@"([\{\}:;\t ])Hook\s*\(([^,]*),([^;]*)\)\s*;",
					"$1foreach(var __hook in ScriptManager.Instance.GetHooks(this.NPC.Name, $3)) { bool stop = false; foreach(var __subTalkResponse in __hook($2, this)) { if(__subTalkResponse == null) { stop = true; break; } yield return __subTalkResponse; } if(stop) break; }",
					RegexOptions.Compiled);

				// Break();
				// --> yield return null;
				file = Regex.Replace(file,
					@"([\{\}:;\t ])Break\s*\(\s*\)\s*;",
					"yield return null;",
					RegexOptions.Compiled);

				// Stop();
				// --> yield return false;
				file = Regex.Replace(file,
					@"([\{\}:;\t ])Stop\s*\(\s*\)\s*;",
					"yield return false;",
					RegexOptions.Compiled);
			}

			// Append the (changed) file
			sb.Append(file);

			return sb.ToString();
		}

		public void LoadSpawns()
		{
			int count = 0;
			foreach (var spawnInfo in MabiData.SpawnDb.Entries.Values)
			{
				count += this.Spawn(spawnInfo);
			}

			Logger.Info("Spawned " + count.ToString() + " monsters.");
		}

		public void Spawn(uint spawnId, uint amount = 0)
		{
			var info = MabiData.SpawnDb.Find(spawnId);
			if (info == null)
			{
				Logger.Warning("Unknown spawn Id '" + spawnId.ToString() + "'.");
				return;
			}

			this.Spawn(info, amount);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="amount">SpawnInfo.Amount used if 0. Needed for respawning.</param>
		/// <param name="effect"></param>
		/// <returns>Amount of creatures spawned</returns>
		public int Spawn(SpawnInfo info, uint amount = 0, bool effect = false)
		{
			var result = 0;

			var rand = RandomProvider.Get();

			var raceInfo = MabiData.RaceDb.Find(info.RaceId);
			if (raceInfo == null)
			{
				Logger.Warning("Race not found: " + info.RaceId.ToString());
				return 0;
			}

			string aiFilePath = null;
			if (!string.IsNullOrEmpty(raceInfo.AI))
			{
				aiFilePath = Path.Combine(WorldConf.ScriptPath, "ai", raceInfo.AI + ".cs");
				if (!File.Exists(aiFilePath))
				{
					Logger.Warning("AI script '" + raceInfo.AI + ".cs' couldn't be found.");
					aiFilePath = null;
				}
			}

			if (amount == 0)
				amount = info.Amount;

			for (int i = 0; i < amount; ++i)
			{
				var monster = new MabiNPC();
				monster.SpawnId = info.Id;
				monster.Name = raceInfo.Name;
				var loc = info.GetRandomSpawnPoint(rand);
				monster.SetLocation(info.Region, loc.X, loc.Y);
				monster.AnchorPoint = new MabiVertex(loc.X, loc.Y);
				monster.Color1 = raceInfo.ColorA;
				monster.Color2 = raceInfo.ColorB;
				monster.Color3 = raceInfo.ColorC;
				monster.Height = raceInfo.Size;
				monster.LifeMaxBase = raceInfo.Life;
				monster.Life = raceInfo.Life;
				monster.Race = raceInfo.Id;
				monster.BattleExp = raceInfo.Exp;
				monster.Direction = (byte)rand.Next(256);
				monster.State &= ~CreatureStates.GoodNpc; // Use race default?
				monster.GoldMin = raceInfo.GoldMin;
				monster.GoldMax = raceInfo.GoldMax;
				monster.Drops = raceInfo.Drops;

				foreach (var skill in raceInfo.Skills)
				{
					monster.Skills.Add(new MabiSkill(skill.SkillId, skill.Rank, monster.Race));
				}

				monster.LoadDefault();

				// Missing stat data?
				if (monster.Life < 1)
					monster.Life = monster.LifeMaxBase = 10;

				try
				{
					if (aiFilePath != null)
					{
						var aiscriptAsm = this.GetScript(aiFilePath);
						if (aiscriptAsm != null)
						{
							var types = aiscriptAsm.GetTypes().Where(t => t.IsSubclassOf(typeof(AIScript)));
							monster.AIScript = Activator.CreateInstance(types.First()) as AIScript;
							monster.AIScript.Creature = monster;
							monster.AIScript.OnLoad();
							monster.AIScript.Activate(0); // AI is intially active
						}
						else
						{
							raceInfo.AI = null; // Suppress future attempts to load this AI
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Exception(ex);
				}

				monster.AncientEligible = true;
				monster.AncientTime = DateTime.Now.AddMinutes(WorldConf.TimeBeforeAncient);

				WorldManager.Instance.AddCreature(monster);

				if (effect)
					WorldManager.Instance.Broadcast(PacketCreator.SpawnEffect(monster, SpawnEffect.Monster), SendTargets.Range, monster);

				result++;
			}

			return result;
		}

		public IEnumerable<ScriptHook> GetHooks(string npc, string hook)
		{
			Dictionary<string, List<ScriptHook>> hooks;
			this._hooks.TryGetValue(npc, out hooks);
			if (hooks == null)
				return Enumerable.Empty<ScriptHook>();

			List<ScriptHook> calls;
			hooks.TryGetValue(hook, out calls);
			if (calls == null)
				return Enumerable.Empty<ScriptHook>();

			return calls;
		}

		public void AddHook(string npc, string hook, ScriptHook func)
		{
			if (!this._hooks.ContainsKey(npc))
				this._hooks[npc] = new Dictionary<string, List<ScriptHook>>();

			if (!this._hooks[npc].ContainsKey(hook))
				_hooks[npc][hook] = new List<ScriptHook>();

			_hooks[npc][hook].Add(func);
		}

		/// <summary>
		/// Returns the quest script with the given id,
		/// or null if it couldn't be found.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public QuestScript GetQuestScript(uint id)
		{
			QuestScript result;
			_questScripts.TryGetValue(id, out result);
			return result;
		}

		/// <summary>
		/// Returns a list of quests that have the given one listed as prerequisite.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public List<QuestScript> GetFollowUpQuestScripts(uint id)
		{
			var result = new List<QuestScript>();

			foreach (var questScript in _questScripts.Values)
			{
				foreach (var p in questScript.Prerequisites)
				{
					var pqc = p as PrerequisiteQuestCompleted;
					if (pqc != null && pqc.Id == id)
						result.Add(questScript);
				}
			}

			return result;
		}
	}

	public delegate IEnumerable ScriptHook(WorldClient c, NPCScript n);
}
