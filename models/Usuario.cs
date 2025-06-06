namespace SistemaHospitalarApp.Models
{
    public class Usuario
    {
        // Agora o setter é público para que o deserializador possa atribuir o valor
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Cargo { get; set; }

        public Usuario(string nome, string senha, string cargo)
        {
            nome = nome?.Trim();
            senha = senha?.Trim();
            cargo = cargo?.Trim();

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Senha não pode ser vazia.");

            if (string.IsNullOrWhiteSpace(cargo))
                throw new ArgumentException("Cargo não pode ser vazio.");

            Nome = nome;
            Senha = senha;
            Cargo = cargo;
        }

        // Construtor vazio para desserialização
        public Usuario() { }
    }
}
