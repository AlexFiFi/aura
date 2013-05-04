﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;

namespace Aura.Data
{
	public interface IDatabase
	{
		int Count { get; }
		void Clear();
		int Load(string path);
		int Load(string path, bool clear);
		List<DatabaseWarningException> Warnings { get; }
	}

	public abstract class Database<TInfo> : IDatabase where TInfo : class, new()
	{
		public List<TInfo> Entries = new List<TInfo>();
		protected List<DatabaseWarningException> _warnings = new List<DatabaseWarningException>();

		public List<DatabaseWarningException> Warnings { get { return _warnings; } }

		public int Count
		{
			get { return this.Entries.Count; }
		}

		public virtual void Clear()
		{
			this.Entries.Clear();
		}

		public int Load(string path)
		{
			return this.Load(path, false);
		}

		public abstract int Load(string path, bool clear);
	}

	public abstract class DatabaseIndexed<TIndex, TInfo> : IDatabase where TInfo : class, new()
	{
		public Dictionary<TIndex, TInfo> Entries = new Dictionary<TIndex, TInfo>();
		protected List<DatabaseWarningException> _warnings = new List<DatabaseWarningException>();

		public List<DatabaseWarningException> Warnings { get { return _warnings; } }

		public TInfo Find(TIndex key)
		{
			//TInfo result = null;
			//this.IdxEntries.TryGetValue(index, out result);
			//return result;
			return this.Entries.GetValueOrDefault(key);
		}

		public int Count
		{
			get { return this.Entries.Count; }
		}

		public bool Has(TIndex key)
		{
			return this.Entries.ContainsKey(key);
		}

		public virtual void Clear()
		{
			this.Entries.Clear();
		}

		public int Load(string path)
		{
			return this.Load(path, false);
		}

		public abstract int Load(string path, bool clear);
	}

	public class DatabaseWarningException : Exception
	{
		public int Line { get; set; }

		public DatabaseWarningException(string source, int line, string msg, params object[] args)
			: base(string.Format(msg, args))
		{
			this.Source = source;
			this.Line = line;
		}

		public DatabaseWarningException(string msg, params object[] args)
			: base(string.Format(msg, args))
		{ }

		public override string ToString()
		{
			return string.Format("{0} on line {1}: {2}", this.Source, this.Line, this.Message);
		}
	}

	public class FieldCountException : DatabaseWarningException
	{
		public FieldCountException(int expectedAmount)
			: base("Expected at least {0} fields.", expectedAmount)
		{ }
	}
}
