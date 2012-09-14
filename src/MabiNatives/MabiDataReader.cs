// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.IO;

namespace MabiNatives
{

	public class MabiDataReader
	{
		private byte[] _indexBuf;
		private int _arrayIdx;
		private int _columeIdx;
		private int _stringIdx;
		private int _dataIdx;

		public MabiDataReader(string path)
		{
			byte[] hdrBuf = new byte[40];
			FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			fs.Read(hdrBuf, 0, 40);
			if (hdrBuf[0] != 'D' || hdrBuf[1] != 'D')
				throw new Exception();

			int size = BitConverter.ToInt32(hdrBuf, 16) + BitConverter.ToInt32(hdrBuf, 20) + BitConverter.ToInt32(hdrBuf, 24) - 24;
			this._indexBuf = new byte[size];
			fs.Read(this._indexBuf, 0, size);

			this._columeIdx = BitConverter.ToInt32(hdrBuf, 28);
			this._arrayIdx = this._columeIdx + BitConverter.ToInt32(hdrBuf, 32);
			this._stringIdx = this._arrayIdx + BitConverter.ToInt32(hdrBuf, 36);
			this._dataIdx = this._stringIdx + BitConverter.ToInt32(hdrBuf, 20);

			fs.Close();
		}

		private string GetNameInfo(string name, int index, byte[] decorator)
		{
			int nextIdx;
			for (; ; )
			{
				nextIdx = Array.IndexOf(this._indexBuf, (byte)'|', index + 1);
				if (name == System.Text.Encoding.UTF8.GetString(this._indexBuf, index, name.Length).ToLower() &&
					Array.IndexOf(decorator, this._indexBuf[index + name.Length]) >= 0) break;
				if (nextIdx < 0) return "";
				index = nextIdx + 1;
			}

			index += name.Length;
			if (nextIdx < 0)
			{
				nextIdx = Array.IndexOf(this._indexBuf, (byte)'\0', index);
				if (nextIdx < 0) return "";
			}
			return System.Text.Encoding.UTF8.GetString(this._indexBuf, index, nextIdx - index);
		}

		public int GetCount(string name)
		{
			int index, lastIdx;

			name = GetNameInfo(name.ToLower(), this._arrayIdx, new byte[] { (byte)'%', (byte)'#', (byte)'[', (byte)'.', (byte)'@' });

			// get row count
			index = name.IndexOf('[') + 1;
			if (index <= 0) return 0;
			lastIdx = name.IndexOf(']', index);
			if (lastIdx < 0) return 0;

			return Int32.Parse(name.Substring(index, lastIdx - index));
		}

		public int GetSize(string name)
		{
			int index, nextIdx;
			string colname = "";

			// split table and colume name
			name = name.ToLower();
			index = name.IndexOfAny(new char[] { '.', '[' });
			if (index >= 0)
			{
				if (name[index] == '.') colname = name.Substring(index);
				name = name.Remove(index);
			}

			name = GetNameInfo(name, this._arrayIdx, new byte[] { (byte)'%', (byte)'#', (byte)'[', (byte)'.', (byte)'@' });

			// get struct name
			index = name.IndexOf('@') + 1;
			if (index <= 0) return 0;
			nextIdx = name.IndexOf('%', index);
			if (index < 0) return 0;

			name = name.Substring(index, nextIdx - index).ToLower();
			if (colname.Length > 0)
			{
				// get colume size
				name = GetNameInfo(name + colname, 0, new byte[] { (byte)'%', (byte)'#', (byte)'[', (byte)'.', (byte)'@' });
				index = name.IndexOfAny(new char[] { '*', '#' }) + 1;
			}
			else
			{
				// get struct size
				name = GetNameInfo(name, 0, new byte[] { (byte)'%', (byte)'#', (byte)'[', (byte)'.', (byte)'@' });
				index = name.IndexOf('%') + 1;
			}
			if (index <= 0) return 0;

			return Int32.Parse(name.Substring(index));
		}

		public int GetPointer(string name)
		{
			int pointer = 0;
			int index, nextIdx;
			string colname = "";

			// split table and colume name
			name = name.ToLower();
			index = name.IndexOfAny(new char[] { '.', '[' });
			if (index >= 0)
			{
				if (name[index] == '.') colname = name.Substring(index);
				else
				{
					index += 1;
					nextIdx = name.IndexOf(']', index);
					if (nextIdx < 0) return -1;
					pointer += Int32.Parse(name.Substring(index, nextIdx - index));
				}
				name = name.Remove(index);
			}

			name = GetNameInfo(name, this._arrayIdx, new byte[] { (byte)'%', (byte)'#', (byte)'[', (byte)'.', (byte)'@' });

			// skip row count
			index = name.IndexOf(']');
			if (index < 0) return 0;

			// get struct name
			index += 2;
			nextIdx = name.IndexOf('%', index);
			if (nextIdx < 0) return 0;
			name = name.Substring(index, nextIdx - index).ToLower();
			// combine struct and colume name
			colname = name + colname;

			if (pointer > 0)
			{
				// multiply row size
				name = GetNameInfo(name, 0, new byte[] { (byte)'%' });
				if (name.Length <= 1) return -1;
				pointer *= Int32.Parse(name.Substring(1));
			}

			// get colume offset
			name = GetNameInfo(colname, this._columeIdx, new byte[] { (byte)'%', (byte)'#', (byte)'[', (byte)'.', (byte)'@' });
			index = name.IndexOf('%') + 1;
			if (index <= 0) return -1;
			nextIdx = name.IndexOfAny(new char[] { '*', '#' }, index);
			pointer += Int32.Parse(name.Substring(index, nextIdx - index));

			return this._dataIdx + pointer;
		}

		public void Fill(byte[] data, int index)
		{
			Array.Copy(this._indexBuf, index, data, 0, data.Length);
		}

		public string GetString(int index)
		{
			index += this._stringIdx;
			int lastIdx = Array.IndexOf(this._indexBuf, (byte)'\0', index);
			return System.Text.Encoding.UTF8.GetString(this._indexBuf, index, lastIdx - index);
		}
	}

}