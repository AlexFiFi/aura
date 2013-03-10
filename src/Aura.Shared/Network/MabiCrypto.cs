﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;

namespace Aura.Shared.Network
{
	public class MabiCrypto
	{
		private static uint[] _MCInitVector = {
			0xDB792195, 0xE63BF923, 0x4F6F076D, 0x122CDED5, 0xDA711220, 0x4F6FCEE6, 0x45E45C44, 0xCF852932,
			0x469ADBF6, 0x5E12481F, 0x682D2354, 0xBDD21105, 0x5BB5F3E6, 0x6E8C82CA, 0xD0FDD8E3, 0xBE4CB2DF,
			0xE3EB3F01, 0xD03D3F32, 0x77A4E3FF, 0xDF17D4B5, 0x2AC710AC, 0x5ACAC8ED, 0x703BD8E4, 0x1D9C3B3B,
			0x4564A5D2, 0xB0224742, 0xB5A3048A, 0xA35A3F55, 0xCA33744F, 0xF1AEC0A0, 0x0BF4B1A4, 0x04038654,
			0x7C65A9EC, 0xAA41DC51, 0xE5096687, 0x1D433C75, 0x35DE20AF, 0xDB75DA77, 0x230A8E81, 0x18BE6B31,
			0x8D8857BA, 0x0EC016A2, 0x2FF6FB27, 0x37D3EB09, 0x91A6A259, 0x13E0EA7B, 0x07926A43, 0x21DF97F9,
			0x24FD4BEB, 0x10315AA3, 0x58D70DF3, 0x7C56F52D, 0x1ABF4E8F, 0x50E140B0, 0xE1E9FEB2, 0x90380A5E,
			0xEA3761D3, 0x583B843B, 0xF0F9496F, 0xC9FCD225, 0x35EC4846, 0x550CFA8B, 0x574DD012, 0x66C3ABF9,
			0x55A7DBF8, 0x510A4DAE, 0xFF1738CC, 0x9AC85D2F, 0x3A7704F5, 0xDDB85A5C, 0x86A50929, 0xFB01BD4F,
			0x877DDF76, 0x187EF004, 0x912E5B3E, 0xEBE1CBDD, 0x354F0206, 0x9D889D03, 0x910D16EF, 0xA67178A7,
			0xC2DEEEDF, 0xE433A4BC, 0x474B91F7, 0x14A28299, 0xCBAD7D66, 0x494D39DF, 0x8B6BB28E, 0xCDCC33EE,
			0xAF37C9D3, 0x9D79794F, 0x6B2C05ED, 0xA8823F18, 0x6225D7ED, 0x292E7345, 0xA04CE9DF, 0x951ABE7F,
			0x72C3CC98, 0xCD1E0582, 0x41224C23, 0x90D26E39, 0xC75B1461, 0xE0B5DBD1, 0x83BA4F78, 0x2A072F2B,
			0xA1A24F28, 0xB3BF06C8, 0xD5335518, 0x186662AC, 0xBC71C21F, 0x2BFC24A8, 0xDBB15F07, 0x076C83F5,
			0xBD656129, 0x507A23D6, 0xB3CE5930, 0xD68E24EF, 0xE82CAF40, 0xA53C0493, 0x79CE3BB7, 0x030FEF29,
			0x347E8A30, 0xFF6E9D5C, 0xCE9B9A04, 0xBC4E5140, 0x949BBE83, 0xCB8B3DBE, 0xAE1D5F44, 0x21148102,
			0x754E9EAF, 0xF2DEAE15, 0xF2EB3A8C, 0x8B1614AE, 0x6ED005B6, 0xCA16DD83, 0x29BD701B, 0xAA6201C9,
			0x58CE4025, 0x9C6121CB, 0x11C03985, 0xEFFC2A2A, 0x4136605B, 0x7F30DDFD, 0x19ECCC35, 0x4D6E1426,
			0x6360EE03, 0xE69CB55A, 0xD181A69D, 0x7E69A4C7, 0x91B4C97C, 0x9BA0F967, 0xE05438A6, 0xF6A6AB45,
			0xD0808096, 0xCE1DB289, 0x8FDF796F, 0x4D3B9B25, 0xA4AD9AE6, 0x5DF9105F, 0x71B1FD3E, 0xF90A6F7C,
			0xEE1D1310, 0x183B6922, 0x2AD9A280, 0x50EAD939, 0x0B5990F6, 0xEAB6D31C, 0xD1F23104, 0x3FA68196,
			0xA228DCCC, 0x68C10912, 0x9003CF9C, 0xB92E58C4, 0x9FBBA10F, 0x3DA05555, 0xAE52996D, 0x8F0A3B2C,
			0x12F8D4DF, 0x02DF25C9, 0x51D99D11, 0x950AEAD2, 0xFFF87B29, 0xCDA6B1E7, 0x13056DC7, 0x189BA81A,
			0xA23C22E1, 0x0F9E5E7F, 0x799B23CD, 0x53DB90F6, 0xA388548B, 0xB9C0D3E5, 0x753BB810, 0xC3768EFD,
			0x2D66B7F2, 0x16EEC5CB, 0x4490D722, 0xF813E0E6, 0xD02DC104, 0x1FE5CA4F, 0x18A27E69, 0x191699FF,
			0xB366A330, 0x4B01495D, 0x5529AD4D, 0x48F2FE87, 0x926C55F1, 0x53491F9B, 0x875BA8D1, 0xEF2C3CD8,
			0xE50C89F9, 0x5201768C, 0x1A5C4B35, 0x114C0C76, 0x77D8B1DF, 0x75E77E8A, 0x39826BA2, 0x3FFACC4F,
			0xF5FF4FF7, 0xA0B10525, 0xFD46ED4F, 0xE97C9F45, 0x568CB141, 0xD45C1365, 0x788B2B7E, 0xABAA5158,
			0x73AD6AEA, 0x041E5FCA, 0x0BE48855, 0x01C285AF, 0x0B2EFB29, 0x49655AEE, 0xABBF0B3C, 0x958E4617,
			0x0A8CD37A, 0x871758F9, 0x0DB1A84D, 0xE453974A, 0x78308BD6, 0x7456A7EB, 0x76FF0F79, 0x3C1957D9,
			0xD6E89EE6, 0x4DB3A5B2, 0xBAC849BA, 0xC68E2110, 0xE8F9F2EB, 0x970F676E, 0x034377B6, 0xF0DC724F,
			0xFF290056, 0xD3B7F2FF, 0x72DF023B, 0x3021ACA2, 0x352EE316, 0xE046B5CA, 0xE94E08BC, 0x41BFFB5A
		};

		private byte[] _decryptKey;
		private uint[] _decryptSet;
		private uint _decryptState1;
		private uint _decryptState2;
		private uint _decryptState3;
		private int _decryptSetIndex;
		private int _decryptKeyIndex;

		public uint Seed { get; set; }

		public MabiCrypto(uint seed)
		{
			this.Seed = seed;

			_decryptKey = new byte[128];
			_decryptSet = new uint[512];

			// round 0
			Array.Copy(_MCInitVector, _decryptSet, 256);
			Array.Clear(_decryptSet, 256, 256);
			_decryptState1 = seed;
			_decryptState2 = 0;
			_decryptState3 = 0;
			_decryptSetIndex = 0;
			uint key = getNextKey();

			// round 1
			Array.Copy(_MCInitVector, _decryptSet, 256);
			Array.Clear(_decryptSet, 256, 256);
			_decryptState1 = seed;
			_decryptState2 = key;
			_decryptState3 = 0;
			_decryptSetIndex = 0;
			for (int i = 0; i < 5; i++)
			{
				key = getNextKey();
			}

			// round 2
			Array.Copy(_MCInitVector, _decryptSet, 256);
			Array.Clear(_decryptSet, 256, 256);
			_decryptState1 = key;
			_decryptState2 = 0;
			_decryptState3 = 0;
			_decryptSetIndex = 0;

			// fill decrypt keys
			_decryptKeyIndex = 31;
			for (int i = 124; i >= 0; i -= 4)
			{
				BitConverter.GetBytes(getNextKey()).CopyTo(_decryptKey, i);
			}
		}

		private uint getNextKey()
		{
			int i = (--_decryptSetIndex);
			if (i < 256)
			{
				uint var1 = _decryptState1;
				uint var2 = _decryptState2 + (++_decryptState3);

				i = 0;
				while (i < 256)
				{
					uint j, k;
					var1 = (var1 ^ (var1 << 13)) + _decryptSet[(i + 128) & 0xff];
					j = _decryptSet[i];
					k = _decryptSet[i] = _decryptSet[(j & 0x3ff) >> 2] + var1 + var2;
					var2 = _decryptSet[i + 256] = _decryptSet[((k >> 8) & 0x3ff) >> 2] + j;
					i++;
					var1 = (var1 ^ (var1 >> 6)) + _decryptSet[(i + 128) & 0xff];
					j = _decryptSet[i];
					k = _decryptSet[i] = _decryptSet[(j & 0x3ff) >> 2] + var1 + var2;
					var2 = _decryptSet[i + 256] = _decryptSet[((k >> 8) & 0x3ff) >> 2] + j;
					i++;
					var1 = (var1 ^ (var1 << 2)) + _decryptSet[(i + 128) & 0xff];
					j = _decryptSet[i];
					k = _decryptSet[i] = _decryptSet[(j & 0x3ff) >> 2] + var1 + var2;
					var2 = _decryptSet[i + 256] = _decryptSet[((k >> 8) & 0x3ff) >> 2] + j;
					i++;
					var1 = (var1 ^ (var1 >> 16)) + _decryptSet[(i + 128) & 0xff];
					j = _decryptSet[i];
					k = _decryptSet[i] = _decryptSet[(j & 0x3ff) >> 2] + var1 + var2;
					var2 = _decryptSet[i + 256] = _decryptSet[((k >> 8) & 0x3ff) >> 2] + j;
					i++;
				}

				_decryptState1 = var1;
				_decryptState2 = var2;
				i = _decryptSetIndex = 511;
			}

			return _decryptSet[i];
		}


		public void EncodePacket(ref byte[] packet)
		{
			if (packet[5] == 0x1)
				BitConverter.GetBytes(BitConverter.ToUInt32(packet, 6) ^ 0x98BADCFE).CopyTo(packet, 6);
			else
				packet[5] = 0x3;
		}

		public void DecodePacket(ref byte[] packet)
		{
			int len = packet.Length;

			len -= 4;
			int idx = _decryptKeyIndex * 4;
			int i = 6;
			// block decryption (OFB mode)
			while (i < len)
			{
				int j = idx & 0x1f; idx += 4;
				packet[i++] ^= _decryptKey[j++];
				packet[i++] ^= _decryptKey[j++];
				packet[i++] ^= _decryptKey[j++];
				packet[i++] ^= _decryptKey[j++];
			}
			len += 4;
			// process left bytes
			while (i < len)
			{
				packet[i++] ^= _decryptKey[(idx++) & 0x7f];
			}

			// update key stream
			idx = _decryptKeyIndex;
			BitConverter.GetBytes(getNextKey()).CopyTo(_decryptKey, idx);
			_decryptKeyIndex = (idx + 31) & 0x1f;
		}
	}
}
