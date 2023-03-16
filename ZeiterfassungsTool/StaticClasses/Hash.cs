using CryptSharp.Utility;
using Scrypt;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeiterfassungsTool.StaticClasses
{
    
    public static class Hash
    {

        public static ScryptEncoder Encoder { get; set; } = new ScryptEncoder();
        public static string HashPassword(string password)            //SHA3-224	 Man sollte am besten Sha3 verwenden, da sicherer
        {
            SHA512 hash = SHA512.Create();    
            var passwordBytes = Encoding.Default.GetBytes(password); 
            var hashedpassword = hash.ComputeHash(passwordBytes);

            return Convert.ToHexString(hashedpassword);  
        }

        // Dieses Verschlüsselungsverfahren müsste sicherer sein als der von oben. Es benötigt in diesen Fall keinen Salt, da er wahrscheinlich intern erzeugt wird.
        // Quelle: https://www.youtube.com/watch?v=Kl34GpcbEqg
        /// <summary>
        /// Verschlüsselt das Passwort in einen 1 Way Hashwert
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPasswordScrypt(string password)
        {
            if (password != null)
                return Encoder.Encode(password);
            else
                return null;
        }
    }
}
