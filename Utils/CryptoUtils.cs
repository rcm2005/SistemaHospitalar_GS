using System.Security.Cryptography;
using System.Text;

namespace SistemaHospitalarApp.Utils
{
    public static class CryptoUtils
    {
        private static readonly string chave = "S3nh@Sup3rS3cr3ta!";
        private static readonly byte[] iv = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes fixos

        public static string Encrypt(string texto)
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(chave.PadRight(32).Substring(0, 32)); // AES-256
            aes.IV = iv;

            using var encryptor = aes.CreateEncryptor();
            byte[] inputBytes = Encoding.UTF8.GetBytes(texto);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string textoCriptografado)
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(chave.PadRight(32).Substring(0, 32)); // AES-256
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(textoCriptografado);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
