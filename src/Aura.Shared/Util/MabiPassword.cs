// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Aura.Shared.Util
{
	public static class MabiPassword
	{
		private static MD5 _md5 = MD5.Create();
		private static SHA256Managed _sha256 = new SHA256Managed();

		public static PasswordType CurrentType { get { return PasswordType.SHA256; } }

		/// <summary>
		/// Hashes password with MD5.
		/// </summary>
		public static string RawToMD5(string password)
		{
			return BitConverter.ToString(_md5.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
		}

		/// <summary>
		/// Hashes password with SHA256.
		/// </summary>
		public static string MD5ToSHA256(string password)
		{
			return BitConverter.ToString(_sha256.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
		}

		/// <summary>
		/// Hashes password first with MD5, then with SHA256.
		/// </summary>
		public static string RawToMD5SHA256(string password)
		{
			return MD5ToSHA256(RawToMD5(password));
		}

		/// <summary>
		/// Hashes password coming from the client with BCrypt.
		/// </summary>
		public static string Hash(string password)
		{
			return BCrypt.HashPassword(password, BCrypt.GenerateSalt(12));
		}

		/// <summary>
		/// Hashes raw password with MD5, SHA256, and BCrypt.
		/// </summary>
		public static string HashRaw(string password)
		{
			return BCrypt.HashPassword(RawToMD5SHA256(password), BCrypt.GenerateSalt(12));
		}

		/// <summary>
		/// Checks the password with BCrypt.
		/// </summary>
		public static bool Check(string password, string hashedPassword)
		{
			return BCrypt.CheckPassword(password, hashedPassword);
		}
	}

	public enum PasswordType : byte
	{
		Raw = 0,
		MD5 = 1,
		SHA256 = 2,
	}
}
