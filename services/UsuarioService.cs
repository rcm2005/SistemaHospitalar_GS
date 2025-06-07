using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SistemaHospitalarApp.Models;
using SistemaHospitalarApp.Utils; //para JsonCryptoStorage

namespace SistemaHospitalarApp.Services
{
    public static class UsuarioService
    {
        private static readonly string CaminhoArquivo =
            Path.Combine(AppContext.BaseDirectory, "data", "usuarios.json");

        public static List<Usuario> ObterUsuarios()
        {
            try
            {
                string pastaData = Path.GetDirectoryName(CaminhoArquivo)!;
                Directory.CreateDirectory(pastaData);

                var usuarios = JsonCryptoStorage.Ler<List<Usuario>>(CaminhoArquivo);
                return usuarios ?? new List<Usuario>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Erro ao carregar usuários criptografados: {ex.Message}");
                return new List<Usuario>();
            }
        }

        public static bool AdicionarUsuario(Usuario novoUsuario)
        {
            try
            {
                var usuarios = ObterUsuarios();

                // verifica ignorando caixa alta
                bool existe = usuarios.Any(u =>
                    !string.IsNullOrWhiteSpace(u.Nome) &&
                    u.Nome.Equals(novoUsuario.Nome, StringComparison.OrdinalIgnoreCase));

                if (existe)
                {
                    Console.WriteLine("Já existe um usuário com esse nome.");
                    return false;
                }

                usuarios.Add(novoUsuario);

                JsonCryptoStorage.Salvar(CaminhoArquivo, usuarios);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar usuário criptografado: {ex.Message}");
                return false;
            }
        }


        public static Usuario? Autenticar(string nome, string senha)
        {
            var usuarios = ObterUsuarios();
            Console.WriteLine($"[DEBUG] Usuários carregados (criptografados): {usuarios.Count}");


            return usuarios.FirstOrDefault(u =>
                !string.IsNullOrWhiteSpace(u.Nome) &&
                u.Nome.Equals(nome.Trim(), StringComparison.OrdinalIgnoreCase) &&
                u.Senha == senha);
        }

        public static void AtualizarUsuarioPorIndice(int indice, Usuario usuarioAtualizado)
        {
            var usuarios = ObterUsuarios();
            if (indice < 0 || indice >= usuarios.Count)
            {
                throw new IndexOutOfRangeException("Índice de usuário inválido.");
            }

            usuarios[indice] = usuarioAtualizado;
            JsonCryptoStorage.Salvar(CaminhoArquivo, usuarios);
        }

        public static void RemoverUsuarioPorIndice(int indice)
        {
            var usuarios = ObterUsuarios();
            if (indice < 0 || indice >= usuarios.Count)
            {
                throw new IndexOutOfRangeException("Índice de usuário inválido.");
            }

            usuarios.RemoveAt(indice);
            JsonCryptoStorage.Salvar(CaminhoArquivo, usuarios);
        }


    }
}
