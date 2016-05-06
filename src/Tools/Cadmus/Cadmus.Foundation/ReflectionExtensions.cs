using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadmus.Foundation
{
    public static class ReflectionExtensions
    {
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static string ToSha256(this string text)
        {
            var bytes = Encoding.Unicode.GetBytes(text);
            var hashstring = new SHA256Managed();
            var hash = hashstring.ComputeHash(bytes);
            var hashString = string.Empty;
            foreach (var x in hash)
            {
                hashString += $"{x:x2}";
            }
            return hashString;
        }

        public static string Serialize(this object obj)
        {
            var dcSer = new XmlSerializer(obj.GetType());
            var memoryStream = new MemoryStream();

            dcSer.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            var sr = new StreamReader(memoryStream);
            return sr.ReadToEnd();
        }

        public static T Deserialize<T>(this string obj) where T : class
        {
            if (obj.IsNullOrEmpty())
                return default(T);
            var dcSer = new XmlSerializer(typeof(T));
            var memoryStream = new MemoryStream();
            var sw = new StreamWriter(memoryStream);
            sw.Write(obj);
            sw.Flush();
            memoryStream.Position = 0;
            return dcSer.Deserialize(memoryStream) as T;
        }

        public static void Apply<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
                action(item);
        }

        public static bool IsIn<T>(this T element, params T[] elements)
        {
            if (elements == null)
                return false;

            return elements.Contains(element);
        }

        public static byte[] ToBytes(this int number)
        {
            var intBytes = BitConverter.GetBytes(number);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            return intBytes;
        }

        public static int ReadInt(this Stream stream)
        {
            var bytes = new byte[sizeof(int)];
            stream.Read(bytes, 0, sizeof(int));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static SecureString ToSecureString(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;
            var result = new SecureString();
            foreach (var c in source)
                result.AppendChar(c);
            return result;
        }
    }
}
