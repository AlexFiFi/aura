// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.IO;
using System;

namespace Aura.Data
{
	public abstract class DatabaseCSV<TInfo> : Database<TInfo> where TInfo : class, new()
	{
		public override int Load(string path, bool clear)
		{
			if (clear)
				this.Clear();

			using (var csv = new CSVReader(path))
			{
				foreach (var entry in csv.Next())
				{
					try
					{
						this.ReadEntry(entry);
					}
					catch (DatabaseWarningException ex)
					{
						ex.Line = entry.Line;
						ex.Source = Path.GetFileName(path);
						this.Warnings.Add(ex);
						continue;
					}
					//catch (FormatException)
					//{
					//    this.Warnings.Add(new DatabaseWarningException(Path.GetFileName(path), entry.Line, "Number format exception."));
					//    continue;
					//}
				}
			}

			return this.Entries.Count;
		}

		protected abstract void ReadEntry(CSVEntry entry);
	}

	public abstract class DatabaseCSVIndexed<TIndex, TInfo> : DatabaseIndexed<TIndex, TInfo> where TInfo : class, new()
	{
		public override int Load(string path, bool clear)
		{
			if (clear)
				this.Clear();

			using (var csv = new CSVReader(path))
			{
				foreach (var entry in csv.Next())
				{
					try
					{
						this.ReadEntry(entry);
					}
					catch (DatabaseWarningException ex)
					{
						ex.Line = entry.Line;
						ex.Source = Path.GetFileName(path);
						this.Warnings.Add(ex);
						continue;
					}
					//catch (FormatException)
					//{
					//    this.Warnings.Add(new DatabaseWarningException(Path.GetFileName(path), entry.Line, "Number format exception."));
					//    continue;
					//}
				}
			}

			return this.Entries.Count;
		}

		protected abstract void ReadEntry(CSVEntry entry);
	}
}
