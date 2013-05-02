// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Aura.Shared.Util;
using System.Globalization;

namespace Aura.Shared.Network
{
	public class MabiPacket
	{
		public enum ElementType : byte { None = 0, Byte, Short, Int, Long, Float, String, Bin, Ptr }
		public enum PacketType : byte { Normal, Chat }

		private List<object> _elements = new List<object>();

		public byte[] _buffer;
		private int _ptr;

		private bool _new = false;

		public uint Op;
		public ulong Id;

		public PacketType Type = PacketType.Normal;

		public MabiPacket(uint op, ulong id = 0)
		{
			this.Op = op;
			this.Id = id;
			_new = true;

			if (op >= 0xC000 && op < 0xD000)
				this.Type = PacketType.Chat;
		}

		public MabiPacket(byte[] buffer, bool isChat = false)
			: this(buffer, buffer.Length, isChat)
		{ }

		public MabiPacket(byte[] buffer, int length, bool isChat = false)
		{
			if (isChat)
				this.Type = PacketType.Chat;

			_buffer = buffer;
			_ptr = 0;

			// Set start of actual packet
			if (this.Type == PacketType.Normal)
				_ptr = 6;
			else if (this.Type == PacketType.Chat)
			{
				_ptr = 3;
				while (_ptr < length)
				{ if (_buffer[++_ptr] == 0) break; }
			}

			// Read header data
			this.Op = (uint)IPAddress.HostToNetworkOrder(BitConverter.ToInt32(_buffer, _ptr));
			this.Id = (ulong)IPAddress.HostToNetworkOrder(BitConverter.ToInt64(_buffer, _ptr + 4));
			_ptr += 12;

			while (_ptr < length)
			{ if (_buffer[++_ptr - 1] == 0) break; }
		}

		public ElementType GetElementType()
		{
			if (_ptr + 2 > _buffer.Length)
				return 0;
			return (ElementType)_buffer[_ptr];
		}

		// Setters
		// ------------------------------------------------------------------
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
		public MabiPacket PutLong(MabiTime val) { return this.Put((ulong)val.MabiTimeStamp); }
		public MabiPacket PutLongs(params ulong[] vals) { foreach (var val in vals) { this.Put(val); } return this; }

		public MabiPacket PutFloat(float val) { return this.Put(val); }
		public MabiPacket PutFloat(double val) { return this.Put((float)val); }
		public MabiPacket PutFloats(params float[] vals) { foreach (var val in vals) { this.Put(val); } return this; }

		public MabiPacket PutString(string val) { return this.Put(val != null ? val : string.Empty); }
		public MabiPacket PutString(string format, params object[] args) { return this.Put(string.Format((format != null ? format : string.Empty), args)); }
		public MabiPacket PutStrings(params string[] vals) { foreach (var val in vals) { this.PutString(val); } return this; }

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

		// Getters
		// ------------------------------------------------------------------
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

			_ptr += 1;
			var val = BitConverter.ToSingle(_buffer, _ptr);
			_ptr += 4;

			return val;
		}

		public string GetString()
		{
			if (this.GetElementType() != ElementType.String)
				throw new ArgumentException("Expected String, got " + this.GetElementType() + ".");

			int len = (_buffer[_ptr + 1] << 8) + _buffer[_ptr + 2];
			_ptr += 3;
			var val = Encoding.UTF8.GetString(_buffer, _ptr, len - 1);
			_ptr += len;

			return val;
		}

		/// <summary>
		/// Returns string from packet, or "" if next element isn't a string.
		/// Doesn't jump to next element if "".
		/// </summary>
		/// <returns></returns>
		public string GetStringOrEmpty()
		{
			try
			{
				return this.GetString();
			}
			catch (ArgumentException)
			{
				return "";
			}
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
			var ptr = 0;

			var header = new byte[20];
			var headerLen = 12;

			var body = new byte[4096];
			var bodyLen = 0;
			var bodyCount = 0;

			// Packet body
			{
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
						var valb = Encoding.UTF8.GetBytes((element as string) + '\0');
						var len = (short)(valb.Length);

						body[ptr++] = 6;
						Array.Copy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder(len)), 0, body, ptr, 2);
						ptr += 2;
						Array.Copy(valb, 0, body, ptr, len);
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
			}

			// Packet header
			{
				ptr = 0;

				// Op + Id
				Array.Copy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((int)this.Op)), 0, header, ptr, 4);
				Array.Copy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((long)this.Id)), 0, header, ptr + 4, 8);
				ptr = 12;

				// Body len
				int n = bodyLen;
				do
				{
					header[ptr++] = (byte)(n > 0x7F ? (0x80 | (n & 0xFF)) : n & 0xFF);
					n >>= 7;
					headerLen++;
				}
				while (n != 0);

				// Element amount
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
			}

			byte[] result = null;

			// Combine + overall header
			{
				ptr = 0;

				// Add overall
				if (includeOverallHeader)
				{
					if (this.Type == PacketType.Normal)
					{
						result = new byte[6 + headerLen + bodyLen + 4];
						result[ptr++] = 0x88;
						Array.Copy(BitConverter.GetBytes(result.Length), 0, result, ptr, 4);
						ptr += 4;
						result[ptr++] = 0;
					}
					else if (this.Type == PacketType.Chat)
					{
						var overall = new byte[10];
						var overallLen = ptr = 3;

						overall[0] = 0x55;
						overall[1] = 0x12;
						overall[2] = 0x00;

						var n = bodyLen + headerLen;
						do
						{
							overall[ptr++] = (byte)(n > 0x7F ? (0x80 | (n & 0xFF)) : n & 0xFF);
							n >>= 7;
							overallLen++;
						}
						while (n != 0);

						result = new byte[overallLen + headerLen + bodyLen];
						Array.Copy(overall, 0, result, 0, overallLen);
						ptr = overallLen;
					}
				}
				else
				{
					result = new byte[headerLen + bodyLen];
				}

				// Add header and body
				Array.Copy(header, 0, result, ptr, headerLen);
				ptr += headerLen;
				Array.Copy(body, 0, result, ptr, bodyLen);
			}

			return result;
		}

		/// <summary>
		/// Prints packet in a readable format.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var result = new StringBuilder();

			result.Append("Op: " + this.Op.ToString("X").PadLeft(8, '0') + ", Id: " + this.Id.ToString("X").PadLeft(16, '0') + "\n");

			uint i = 1;
			if (!_new)
			{
				var savPtr = _ptr;

				ElementType type;
				while ((type = this.GetElementType()) != ElementType.None)
				{
					if (type == ElementType.Byte)
					{
						var data = this.GetByte();
						result.Append(i.ToString().PadLeft(3, '0') + " [" + data.ToString("X02").PadLeft(16, '.') + "] Byte   : " + data);
					}
					else if (type == ElementType.Short)
					{
						var data = this.GetShort();
						result.Append(i.ToString().PadLeft(3, '0') + " [" + data.ToString("X04").PadLeft(16, '.') + "] Short  : " + data);
					}
					else if (type == ElementType.Int)
					{
						var data = this.GetInt();
						result.Append(i.ToString().PadLeft(3, '0') + " [" + data.ToString("X08").PadLeft(16, '.') + "] Int    : " + data);
					}
					else if (type == ElementType.Long)
					{
						var data = this.GetLong();
						result.Append(i.ToString().PadLeft(3, '0') + " [" + data.ToString("X16").PadLeft(16, '.') + "] Long   : " + data);
					}
					else if (type == ElementType.Float)
					{
						var data = this.GetFloat();
						result.Append(i.ToString().PadLeft(3, '0') + " [........xxxxxxxx] Float  : " + data.ToString(CultureInfo.InvariantCulture));
					}
					else if (type == ElementType.String)
					{
						var data = this.GetString();
						result.Append(i.ToString().PadLeft(3, '0') + " [................] String : " + data);
					}
					else if (type == ElementType.Bin)
					{
						var data = BitConverter.ToString(this.GetBin());
						var splitted = data.Split('-');

						result.Append(i.ToString().PadLeft(3, '0') + " [................] Bin    : ");
						for (var j = 1; j <= splitted.Length; ++j)
						{
							result.Append(splitted[j - 1]);
							if (j < splitted.Length)
								if (j % 16 == 0)
									result.Append("\n".PadRight(33, ' '));
								else
									result.Append(' ');
						}
					}
					result.Append("\n");

					i++;
				}

				_ptr = savPtr;
			}
			else
			{
				foreach (var el in _elements)
				{
					if (el is byte)
					{
						var data = (byte)el;
						result.Append(i.ToString().PadLeft(3, '0') + " [" + data.ToString("X02").PadLeft(16, '.') + "] Byte   : " + data);
					}
					else if (el is ushort)
					{
						var data = (ushort)el;
						result.Append(i.ToString().PadLeft(3, '0') + " [" + data.ToString("X04").PadLeft(16, '.') + "] Short  : " + data);
					}
					else if (el is uint)
					{
						var data = (uint)el;
						result.Append(i.ToString().PadLeft(3, '0') + " [" + data.ToString("X08").PadLeft(16, '.') + "] Int    : " + data);
					}
					else if (el is ulong)
					{
						var data = (ulong)el;
						result.Append(i.ToString().PadLeft(3, '0') + " [" + data.ToString("X16").PadLeft(16, '.') + "] Long   : " + data);
					}
					else if (el is float)
					{
						var data = (float)el;
						result.Append(i.ToString().PadLeft(3, '0') + " [........xxxxxxxx] Float  : " + data.ToString(CultureInfo.InvariantCulture));
					}
					else if (el is string)
					{
						var data = (string)el;
						result.Append(i.ToString().PadLeft(3, '0') + " [................] String : " + data);
					}
					else if (el is byte[])
					{
						var data = BitConverter.ToString((byte[])el);
						var splitted = data.Split('-');

						result.Append(i.ToString().PadLeft(3, '0') + " [................] Bin    : ");
						for (var j = 1; j <= splitted.Length; ++j)
						{
							result.Append(splitted[j - 1]);
							if (j < splitted.Length)
								if (j % 16 == 0)
									result.Append("\n".PadRight(33, ' '));
								else
									result.Append(' ');
						}
					}
					result.Append("\n");

					i++;
				}
			}

			return result.ToString();
		}
	}
}
