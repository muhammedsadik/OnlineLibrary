using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Hashing
{
	public class HashingHelper
	{
		public static void CreatePasswordHash(string password, out byte[] passordHash, out byte[] passordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passordSalt = hmac.Key;
				passordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			}
		}

		public static bool VerifyPasswordHash(string password, byte[] passordHash, byte[] passordSalt)
		{
			using (var hmac = new HMACSHA512(passordSalt))
			{
				var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

				for (int i = 0 ; i < computeHash.Length ; i++)
				{
					if (computeHash[i] != passordHash[i]) return false;
				}
			}

			return true;
		}

	}
}
