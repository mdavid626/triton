using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    // Based on https://msdn.microsoft.com/en-us/library/ms229741(v=vs.110).aspx
    public class Protector
    {
        public const int EntropyLength = 16;

        public void EncryptInMemoryData(byte[] buffer, MemoryProtectionScope scope)
        {
            if (buffer.Length <= 0)
                throw new ArgumentException("Buffer");
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            // Encrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Protect(buffer, scope);
        }

        public void DecryptInMemoryData(byte[] buffer, MemoryProtectionScope scope)
        {
            if (buffer.Length <= 0)
                throw new ArgumentException("Buffer");
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            // Decrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Unprotect(buffer, scope);
        }

        public byte[] CreateRandomEntropy()
        {
            // Create a byte array to hold the random value.
            byte[] entropy = new byte[EntropyLength];

            // Create a new instance of the RNGCryptoServiceProvider.
            // Fill the array with a random value.
            new RNGCryptoServiceProvider().GetBytes(entropy);

            // Return the array.
            return entropy;
        }

        public int EncryptDataToStream(byte[] buffer, byte[] entropy, DataProtectionScope scope, Stream stream)
        {
            if (buffer.Length <= 0)
                throw new ArgumentException("Buffer");
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (entropy.Length <= 0)
                throw new ArgumentException("Entropy");
            if (entropy == null)
                throw new ArgumentNullException("entropy");
            if (stream == null)
                throw new ArgumentNullException("stream");

            var length = 0;

            // Encrypt the data in memory. The result is stored in the same same array as the original data.
            var encrptedData = ProtectedData.Protect(buffer, entropy, scope);

            // Write the encrypted data to a stream.
            if (stream.CanWrite)
            {
                stream.Write(encrptedData, 0, encrptedData.Length);
                length = encrptedData.Length;
            }

            // Return the length that was written to the stream. 
            return length;
        }

        public byte[] DecryptDataFromStream(byte[] entropy, DataProtectionScope scope, Stream stream, int length)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (length <= 0)
                throw new ArgumentException("Length");
            if (entropy == null)
                throw new ArgumentNullException("entropy");
            if (entropy.Length <= 0)
                throw new ArgumentException("Entropy");

            var inBuffer = new byte[length];
            byte[] outBuffer;

            // Read the encrypted data from a stream.
            if (stream.CanRead)
            {
                stream.Read(inBuffer, 0, length);
                outBuffer = ProtectedData.Unprotect(inBuffer, entropy, scope);
            }
            else
            {
                throw new IOException("Could not read the stream.");
            }

            // Return the length that was written to the stream. 
            return outBuffer;
        }
    }
}
