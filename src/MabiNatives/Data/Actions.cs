// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace MabiNatives
{
	public class ActionInfo
	{
		public string Classifier;
		public float Speed;

		//public float Transition_Interval;
		//public float Intro_Interval;
		//public float Intro_Speed;
		//public float Repeat_Interval;
	}

	/// <summary>
	/// Contains Information about walking speed of several races.
	/// This is for information purposes only, actually changing
	/// the speed would require client modifications.
	/// </summary>
	public class ActionDb : DataManager<ActionInfo>
	{
		/// <summary>
		/// Returns the action info for the given identifier or null.
		/// </summary>
		/// <param name="classifier"></param>
		/// <returns></returns>
		public ActionInfo Find(string classifier)
		{
			return this.Entries.FirstOrDefault(a => a.Classifier == classifier);
		}

		public override void LoadFromXml(string path)
		{
			if (!File.Exists(path))
				throw new FileNotFoundException("File not found: " + path);

			var reader = new MabiDataReader(path);

			byte[] data = new byte[reader.GetSize("ActionList")];
			int idx = reader.GetPointer("ActionList");
			int count = reader.GetCount("ActionList");
			for (int i = 0; i < count; i++, idx += data.Length)
			{
				reader.Fill(data, idx);
				var classifier = reader.GetString(BitConverter.ToInt32(data, 0));

				var info = new ActionInfo();
				//info.Transition_Interval = (float)BitConverter.ToInt32(data, 4);
				//info.Intro_Interval = (float)BitConverter.ToInt32(data, 8);
				//info.Intro_Speed = (float)BitConverter.ToInt32(data, 12);
				//info.Repeat_Interval = (float)BitConverter.ToInt32(data, 16);
				info.Classifier = classifier;
				info.Speed = BitConverter.ToSingle(data, 20);

				this.Entries.Add(info);
			}
		}
	}
}
