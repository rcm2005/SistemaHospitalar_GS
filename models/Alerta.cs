using System;

namespace SistemaHospitalarApp.Models
{
    public class Alerta
    {
        public DateTime DataHora { get; set; }
        public string Nivel { get; set; }  // Ex: "Crítico", "Aviso", "Informação"
        public string Mensagem { get; set; }

        public Alerta(string nivel, string mensagem)
        {
            DataHora = DateTime.Now;
            Nivel = nivel;
            Mensagem = mensagem;
        }

        public void Exibir()
        {
            Console.WriteLine($"[ALERTA - {Nivel}] {Mensagem} ({DataHora})");
        }
    }
}
