using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Utils
{
    public class SHA512
    {
        public static string Encrypt(string Password)
        {
            byte[] PasswordBytes = Encoding.UTF8.GetBytes(Password);
            byte[] HashBytes = new SHA512Managed().ComputeHash(PasswordBytes);

            string PasswordHash = Convert.ToBase64String(HashBytes);
            return PasswordHash;
        }
    }
}
