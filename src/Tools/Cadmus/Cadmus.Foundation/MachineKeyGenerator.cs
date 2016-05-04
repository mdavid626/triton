using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    public class MachineKeyGenerator
    {
        protected string BinaryToHex(byte[] bytes)
        {
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
            }
            return builder.ToString();
        }

        public string GenerateDecryptionKey()
        {
            using (var decr = new AesCryptoServiceProvider())
            {
                decr.GenerateKey();
                return BinaryToHex(decr.Key);
            }
        }

        public string GenerateValidationKey()
        {
            using (var validObj = new HMACSHA1())
            {
                return BinaryToHex(validObj.Key);
            }
        }
    }
}
