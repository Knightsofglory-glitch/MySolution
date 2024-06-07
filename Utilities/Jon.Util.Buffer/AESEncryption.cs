using System;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Security.Cryptography;


namespace Jon.Util.Encryption
{
    public class AESEncryption
    {
        // This key is 32 bytes (256 bits). The key size must be 128, 192, or 256 bits
        // Picked random bytes
        private static byte[] key = { 174, 152, 19, 88, 5, 55, 196, 44, 6, 194, 54, 2, 171, 65, 222, 87, 53, 67, 8, 19, 123, 25, 93, 29, 52, 26, 187, 148, 253, 33, 111, 60 };
        private static byte[] vector = { 188, 67, 159, 105, 82, 7, 164, 219, 213, 149, 229, 112, 75, 82, 14, 247 };
        private ICryptoTransform encryptor, decryptor;
        private UTF8Encoding encoder;

        public AESEncryption()
        {
            RijndaelManaged rm = new RijndaelManaged();
            encryptor = rm.CreateEncryptor(key, vector);
            decryptor = rm.CreateDecryptor(key, vector);
            encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
        }

        public string Decrypt(string encrypted)
        {
            return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, encryptor);
        }

        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}
