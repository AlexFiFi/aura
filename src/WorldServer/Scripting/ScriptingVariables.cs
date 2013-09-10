// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Aura.World.Database;

namespace Aura.World.Scripting
{
	public class ScriptingVariables
	{
		public dynamic Temp { get; set; }
		public dynamic Perm { get; set; }

		public ScriptingVariables()
		{
			this.Temp = new VariableManager();
			this.Perm = new VariableManager();
		}

		/// <summary>
		/// Saves all permanent variables with this account and character.
		/// </summary>
		public void Save(string accountName, ulong characterId)
		{
			WorldDb.Instance.SaveVars(accountName, characterId, (IDictionary<string, object>)this.Perm);
		}

		/// <summary>
		/// Reads all permanent variables for this account and character
		/// from the database.
		/// </summary>
		public void Load(string accountName, ulong characterId)
		{
			this.Perm = WorldDb.Instance.LoadVars(accountName, characterId);
		}
	}

	public class VariableManager : DynamicObject, IDictionary<string, object>
	{
		public Dictionary<string, object> Variables { get; protected set; }

		public VariableManager()
		{
			this.Variables = new Dictionary<string, object>();
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			this.Variables[binder.Name] = value;
			return true;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (!this.Variables.TryGetValue(binder.Name, out result))
				result = null;
			return true;
		}

		// IDictionary<string, object>
		// ------------------------------------------------------------------

		public void Add(string key, object value)
		{
			this.Variables.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return this.Variables.ContainsKey(key);
		}

		public ICollection<string> Keys
		{
			get { return this.Variables.Keys; }
		}

		public bool Remove(string key)
		{
			return this.Variables.Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return this.Variables.TryGetValue(key, out value);
		}

		public ICollection<object> Values
		{
			get { return this.Variables.Values; }
		}

		public dynamic this[string key]
		{
			get
			{
				if (!this.Variables.ContainsKey(key))
					return null;
				return this.Variables[key];
			}
			set
			{
				this.Variables[key] = value;
			}
		}

		public void Add(KeyValuePair<string, object> item)
		{
			this.Variables.Add(item.Key, item.Value);
		}

		public void Clear()
		{
			this.Variables.Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return this.Variables.Contains(item);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { return this.Variables.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return this.Variables.Remove(item.Key);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return this.Variables.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Variables.GetEnumerator();
		}
	}
}
