using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    public enum PasswordProtectionScope
    {
        CurrentUser = 0,
        LocalMachine = 1,
    }

    public class PasswordProtector
    {
        private readonly Protector _protector = new Protector();

        public string Protect(string password, PasswordProtectionScope scope)
        {
            using (var memoryStream = new MemoryStream())
            {
                var toEncrypt = Encoding.UTF8.GetBytes(password);
                var entropy = _protector.CreateRandomEntropy();
                memoryStream.Write(entropy, 0, entropy.Length);
                memoryStream.WriteByte((byte)scope);
                _protector.EncryptDataToStream(toEncrypt, entropy, ToDataProtectionScope(scope), memoryStream);
                var bytes = memoryStream.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }

        public string UnProtect(string encryptedPassword)
        {
            if (encryptedPassword.Length < 150) // ahh dirty hack...
                return encryptedPassword;
            var toDecrypt = Convert.FromBase64String(encryptedPassword);
            using (var memoryStream = new MemoryStream(toDecrypt))
            {
                var entropy = new byte[Protector.EntropyLength];
                memoryStream.Read(entropy, 0, Protector.EntropyLength);
                var scope = (PasswordProtectionScope)memoryStream.ReadByte();
                var length = memoryStream.Length - memoryStream.Position;
                var decryptedData = _protector.DecryptDataFromStream(entropy, ToDataProtectionScope(scope), memoryStream, (int)length);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        private DataProtectionScope ToDataProtectionScope(PasswordProtectionScope scope)
        {
            switch (scope)
            {
                case PasswordProtectionScope.CurrentUser: return DataProtectionScope.CurrentUser;
                case PasswordProtectionScope.LocalMachine: return DataProtectionScope.LocalMachine;
            }
            return DataProtectionScope.CurrentUser;
        }
    }
}
