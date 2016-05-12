using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cadmus.Foundation.Tests
{
    [TestClass]
    public class ProtectorTest
    {
        // from https://msdn.microsoft.com/en-us/library/ms229741(v=vs.110).aspx
        [TestMethod]
        public void TestRun()
        {
            try
            {
                var protector = new Protector();

                ///////////////////////////////
                //
                // Memory Encryption - ProtectedMemory
                //
                ///////////////////////////////

                // Create the original data to be encrypted (The data length should be a multiple of 16).
                byte[] toEncrypt = UnicodeEncoding.ASCII.GetBytes("ThisIsSomeData16");

                //Console.WriteLine("Original data: " + UnicodeEncoding.ASCII.GetString(toEncrypt));
                //Console.WriteLine("Encrypting...");

                // Encrypt the data in memory.
                protector.EncryptInMemoryData(toEncrypt, MemoryProtectionScope.SameLogon);

                //Console.WriteLine("Encrypted data: " + UnicodeEncoding.ASCII.GetString(toEncrypt));
                //Console.WriteLine("Decrypting...");

                // Decrypt the data in memory.
                protector.DecryptInMemoryData(toEncrypt, MemoryProtectionScope.SameLogon);

                //Console.WriteLine("Decrypted data: " + UnicodeEncoding.ASCII.GetString(toEncrypt));

                ///////////////////////////////
                //
                // Data Encryption - ProtectedData
                //
                ///////////////////////////////

                // Create the original data to be encrypted
                toEncrypt = UnicodeEncoding.ASCII.GetBytes("This is some data of any length.");

                // Create a file.
                FileStream fStream = new FileStream("Data.dat", FileMode.OpenOrCreate);

                // Create some random entropy.
                byte[] entropy = protector.CreateRandomEntropy();

                //Console.WriteLine();
                //Console.WriteLine("Original data: " + UnicodeEncoding.ASCII.GetString(toEncrypt));
                //Console.WriteLine("Encrypting and writing to disk...");

                // Encrypt a copy of the data to the stream.
                int bytesWritten = protector.EncryptDataToStream(toEncrypt, entropy, DataProtectionScope.CurrentUser, fStream);

                fStream.Close();

                //Console.WriteLine("Reading data from disk and decrypting...");

                // Open the file.
                fStream = new FileStream("Data.dat", FileMode.Open);

                // Read from the stream and decrypt the data.
                byte[] decryptData = protector.DecryptDataFromStream(entropy, DataProtectionScope.CurrentUser, fStream, bytesWritten);

                fStream.Close();

                //Console.WriteLine("Decrypted data: " + UnicodeEncoding.ASCII.GetString(decryptData));
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }
    }
}
