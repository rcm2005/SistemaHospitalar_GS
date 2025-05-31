namespace SistemaHospitalarApp.Models
{
    public class Paciente
    {
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Prontuario { get; set; }

        public Paciente(string nome, int idade, string prontuario)
        {
            Nome = nome;
            Idade = idade;
            Prontuario = prontuario;
        }

        public override string ToString()
        {
            return $"Paciente: {Nome}, Idade: {Idade}, Prontuário: {Prontuario}";
        }
    }
}
