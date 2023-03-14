using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZeiterfassungsTool.StaticClasses
{
    public static class Hash
    {
        public static string HashPassword(string password)            //SHA3-224	 Man sollte am besten Sha3 verwenden, da sicherer
        {
            SHA512 hash = SHA512.Create();
            var passwordBytes = Encoding.Default.GetBytes(password);
            var hashedpassword = hash.ComputeHash(passwordBytes);

            return Convert.ToHexString(hashedpassword);
        }
    }
}
