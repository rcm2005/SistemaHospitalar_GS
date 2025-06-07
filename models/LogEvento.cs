using System;

namespace SistemaHospitalarApp.Models
{
    public class LogEvento
    {
        public string Origem { get; set; }
        public string Mensagem { get; set; }
        public string Gravidade { get; set; }
        public DateTime DataHora { get; set; }

        public LogEvento()
        {
            Origem = "";
            Mensagem = "";
            Gravidade = "";
            DataHora = DateTime.Now;
        }

        public LogEvento(string origem, string mensagem, string gravidade, DateTime dataHora)
        {
            Origem = origem;
            Mensagem = mensagem;
            Gravidade = gravidade;
            DataHora = dataHora;
        }

        public LogEvento(string origem, string mensagem, string gravidade)
        {
            Origem = origem;
            Mensagem = mensagem;
            Gravidade = gravidade;
            DataHora = DateTime.Now;
        }

        public LogEvento(string mensagem, DateTime horario, string tipo)
        {
            Origem = "Sistema";
            Mensagem = mensagem;
            Gravidade = tipo;
            DataHora = horario;
        }

        public override string ToString()
        {
            return $"[{DataHora:dd/MM/yyyy HH:mm:ss}] (ALERTA) Origem: {Origem} | Mensagem: {Mensagem} | Gravidade: {Gravidade}";
        }
    }
}
