// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Common.Network
{
	public class MabiPacket
	{
		public enum ElementType : byte { None = 0, Byte, Short, Int, Long, Float, String, Bin, Ptr }

		private List<object> _elements = new List<object>();

		private byte[] _buffer;
		private int _ptr;

		public uint Op;
		public ulong Id;

		public MabiPacket()
		{
		}

		public MabiPacket(uint op, ulong id)
		{
			this.Op = op;
			this.Id = id;
		}

		public MabiPacket(byte[] buffer, int length, bool includeOverall = true)
		{
			if (length < (includeOverall ? 21 : 15))
				throw new Exception("Unsufficent amount of bytes.");

			_buffer = buffer;

			_ptr = (includeOverall ? 6 : 0);

			this.Op = (uint)IPAddress.HostToNetworkOrder(BitConverter.ToInt32(_buffer, _ptr));
			this.Id = (ulong)IPAddress.HostToNetworkOrder(BitConverter.ToInt64(_buffer, _ptr + 4));
			_ptr += 12;

			for (int i = 0; i < 6 && _buffer[++_ptr - 1] != 0x00 && _ptr < length; ++i) { }
			//do { _ptr++; } while (_buffer[_ptr - 1] != 0x00 && _ptr < length);
		}

		public MabiPacket Put<T>(T val)
		{
			_elements.Add(val);
			return this;
		}

		public MabiPacket Put<T>(T val, ushort index)
		{
			_elements.Insert(index, val);
			return this;
		}

		public ElementType GetElementType()
		{
			if (_ptr + 2 > _buffer.Length)
				return 0;
			return (ElementType)_buffer[_ptr];
		}

		public MabiPacket PutByte(byte val) { return this.Put(val); }
		public MabiPacket PutSByte(byte val) { return this.Put(val); }
		public MabiPacket PutByte(bool val) { return this.Put((byte)(val ? 1 : 0)); }
		public MabiPacket PutBytes(params byte[] vals) { foreach (var val in vals) { this.Put(val); } return this; }

		public MabiPacket PutShort(ushort val) { return this.Put(val); }
		public MabiPacket PutSShort(short val) { return this.Put((ushort)val); }
		public MabiPacket PutShorts(params ushort[] vals) { foreach (var val in vals) { this.Put(val); } return this; }

		public MabiPacket PutInt(uint val) { return this.Put(val); }
		public MabiPacket PutSInt(int val) { return this.Put((uint)val); }
		public MabiPacket PutInts(params uint[] vals) { foreach (var val in vals) { this.Put(val); } return this; }

		public MabiPacket PutLong(ulong val) { return this.Put(val); }
		public MabiPacket PutSLong(long val) { return this.Put((ulong)val); }
		public MabiPacket PutLong(DateTime val) { return this.Put((ulong)(val.Ticks / 10000)); }
		public MabiPacket PutLong(TimeSpan val) { return this.Put((ulong)(val.Ticks / 10000)); }
		public MabiPacket PutLongs(params ulong[] vals) { foreach (var val in vals) { this.Put(val); } return this; }

		public MabiPacket PutFloat(float val) { return this.Put(val); }
		public MabiPacket PutFloat(double val) { return this.Put((float)val); }
		public MabiPacket PutFloats(params float[] vals) { foreach (var val in vals) { this.Put(val); } return this; }

		public MabiPacket PutString(string val) { return this.Put(val); }
		public MabiPacket PutBin(byte[] val) { return this.Put(val); }

		public MabiPacket PutBin(object obj)
		{
			int size = Marshal.SizeOf(obj);
			byte[] arr = new byte[size];
			IntPtr ptr = Marshal.AllocHGlobal(size);

			Marshal.StructureToPtr(obj, ptr, true);
			Marshal.Copy(ptr, arr, 0, size);
			Marshal.FreeHGlobal(ptr);

			return this.PutBin(arr);
		}

		public byte GetByte()
		{
			if (this.GetElementType() != ElementType.Byte)
				throw new Exception("Expected Byte, got " + this.GetElementType() + ".");

			_ptr += 1;
			return _buffer[_ptr++];
		}
		public sbyte GetSByte() { return (sbyte)this.GetByte(); }
		public bool GetBool() { return (this.GetByte() > 0); }

		public ushort GetShort()
		{
			if (this.GetElementType() != ElementType.Short)
				throw new Exception("Expected Short, got " + this.GetElementType() + ".");

			_ptr += 1;
			var val = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(_buffer, _ptr));
			_ptr += 2;

			return val;
		}
		public short GetSShort() { return (short)this.GetShort(); }

		public uint GetInt()
		{
			if (this.GetElementType() != ElementType.Int)
				throw new Exception("Expected Int, got " + this.GetElementType() + ".");

			_ptr += 1;
			var val = (uint)IPAddress.HostToNetworkOrder(BitConverter.ToInt32(_buffer, _ptr));
			_ptr += 4;

			return val;
		}
		public int GetSInt() { return (int)this.GetInt(); }

		public ulong GetLong()
		{
			if (this.GetElementType() != ElementType.Long)
				throw new Exception("Expected Long, got " + this.GetElementType() + ".");

			_ptr += 1;
			var val = (ulong)IPAddress.HostToNetworkOrder(BitConverter.ToInt64(_buffer, _ptr));
			_ptr += 8;

			return val;
		}
		public long GetSLong() { return (long)this.GetLong(); }
		public DateTime GetDate() { return new DateTime(this.GetSLong() * 10000); }
		public TimeSpan GetTime() { return new TimeSpan(this.GetSLong() * 10000); }

		public float GetFloat()
		{
			if (this.GetElementType() != ElementType.Float)
				throw new Exception("Expected Float, got " + this.GetElementType() + ".");

			_ptr += 5;
			return BitConverter.ToSingle(_buffer, _ptr - 3);
		}

		public string GetString()
		{
			if (this.GetElementType() != ElementType.String)
				throw new Exception("Expected String, got " + this.GetElementType() + ".");

			_ptr += 3;
			var sb = new StringBuilder();
			while (_buffer[_ptr] != 0x00)
			{
				sb.Append((char)_buffer[_ptr++]);
			}

			_ptr++;

			return sb.ToString();
		}

		public byte[] GetBin()
		{
			if (this.GetElementType() != ElementType.Bin)
				throw new Exception("Expected Bin, got " + this.GetElementType() + ".");

			_ptr += 1;

			var len = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(_buffer, _ptr));
			_ptr += 2;

			var val = new byte[len];
			Array.Copy(_buffer, _ptr, val, 0, len);
			_ptr += len;

			return val;
		}

		public byte[] Build(bool includeOverallHeader = true)
		{
			var header = new byte[30];
			var headerLen = 12;
			var body = new byte[4096];
			var bodyLen = 0;
			var bodyCount = 0;
			var ptr = 0;

			// Body
			foreach (var element in _elements)
			{
				// Resize if we need more space
				if (ptr + 2048 > body.Length)
				{
					Array.Resize(ref body, body.Length + 512);
				}

				if (element is byte)
				{
					body[ptr++] = 1;
					Array.Copy(BitConverter.GetBytes((byte)element), 0, body, ptr, 1);
					ptr += 1;
					bodyLen += 2;
				}
				else if (element is ushort)
				{
					body[ptr++] = 2;
					Array.Copy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((short)(ushort)element)), 0, body, ptr, 2);
					ptr += 2;
					bodyLen += 3;
				}
				else if (element is uint)
				{
					body[ptr++] = 3;
					Array.Copy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((int)(uint)element)), 0, body, ptr, 4);
					ptr += 4;
					bodyLen += 5;
				}
				else if (element is ulong)
				{
					body[ptr++] = 4;
					Array.Copy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((long)(ulong)element)), 0, body, ptr, 8);
					ptr += 8;
					bodyLen += 9;
				}
				else if (element is float)
				{
					body[ptr++] = 5;
					Array.Copy(BitConverter.GetBytes((float)element), 0, body, ptr, 4);
					ptr += 4;
					bodyLen += 5;
				}
				else if (element is string)
				{
					var val = (element as string) + '\0';
					var len = (short)(val.Length);

					body[ptr++] = 6;
					Array.Copy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder(len)), 0, body, ptr, 2);
					ptr += 2;
					Array.Copy(Encoding.ASCII.GetBytes(val), 0, body, ptr, len);
					ptr += len;
					bodyLen += len + 3;
				}
				else if (element is byte[])
				{
					var len = (short)((element as byte[]).Length);

					body[ptr++] = 7;
					Array.Copy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder(len)), 0, body, ptr, 2);
					ptr += 2;
					Array.Copy((element as byte[]), 0, body, ptr, len);
					ptr += len;
					bodyLen += len + 3;
				}
				else
				{
					throw new Exception("Unsupported variable type. (" + element.GetType() + ")");
				}

				bodyCount++;
			}

			// Header
			ptr = 0;

			Array.Copy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((int)this.Op)), 0, header, ptr, 4);
			Array.Copy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((long)this.Id)), 0, header, ptr + 4, 8);
			ptr = 12;

			int n = bodyLen;
			do
			{
				header[ptr++] = (byte)(n > 0x7F ? (0x80 | (n & 0xFF)) : n & 0xFF);
				n >>= 7;
				headerLen++;
			}
			while (n != 0);

			n = bodyCount;
			do
			{
				header[ptr++] = (byte)(n > 0x7F ? (0x80 | (n & 0xFF)) : n & 0xFF);
				n >>= 7;
				headerLen++;
			}
			while (n != 0);

			header[ptr++] = 0;
			headerLen++;

			// Combine
			ptr = 0;

			byte[] result;

			if (includeOverallHeader)
			{
				result = new byte[6 + headerLen + bodyLen];
				result[ptr++] = 0x88;
				Array.Copy(BitConverter.GetBytes(result.Length), 0, result, ptr, 4);
				ptr += 4;
				result[ptr++] = 0;
			}
			else
			{
				result = new byte[headerLen + bodyLen];
			}

			Array.Copy(header, 0, result, ptr, headerLen);
			ptr += headerLen;
			Array.Copy(body, 0, result, ptr, bodyLen);

			return result;
		}
	}
}
