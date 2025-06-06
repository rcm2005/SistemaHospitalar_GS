using System;

namespace SistemaHospitalarApp.Models
{
    public class LogEvento
    {
        // Propriedades somente leitura
        public string Mensagem { get; }
        public DateTime Horario { get; }
        public string Tipo { get; }

        // Construtor com validações
        public LogEvento(string mensagem, DateTime horario, string tipo)
        {
            if (string.IsNullOrWhiteSpace(mensagem))
                throw new ArgumentException("A mensagem do log não pode ser vazia.");

            if (string.IsNullOrWhiteSpace(tipo))
                throw new ArgumentException("O tipo do log não pode ser vazio.");

            Mensagem = mensagem;
            Horario = horario;
            Tipo = tipo;
        }

        public override string ToString()
        {
            return $"[{Horario:dd/MM/yyyy HH:mm:ss}] ({Tipo.ToUpper()}) {Mensagem}";
        }
    }
}
