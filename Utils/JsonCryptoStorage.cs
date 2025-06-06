using System.Text.Json;
using SistemaHospitalarApp.Utils;
using System.IO;

namespace SistemaHospitalarApp.Utils
{
    public static class JsonCryptoStorage
    {
        // Salva um objeto como JSON criptografado no caminho especificado
        public static void Salvar<T>(string caminho, T objeto)
        {
            string json = JsonSerializer.Serialize(objeto, new JsonSerializerOptions { WriteIndented = true });
            string criptografado = CryptoUtils.Encrypt(json);
            File.WriteAllText(caminho, criptografado);
        }

        // Lê um arquivo JSON criptografado, descriptografa e desserializa para o tipo T
        public static T? Ler<T>(string caminho)
        {
            if (!File.Exists(caminho))
                return default;

            string criptografado = File.ReadAllText(caminho);
            string json = CryptoUtils.Decrypt(criptografado);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
