namespace SistemaHospitalarApp.Models

{
    public class Usuario
    {
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }


        public Usuario(string nome, string cargo, string login, string senha)
        {
            Nome = nome;
            Cargo = cargo;
            Login = login;
            Senha = senha;
        }

        public bool Autenticar(string login, string senha)
        {
            return Login == login && Senha == senha;
        
        }
    }
}