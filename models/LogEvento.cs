using System;

namespace SistemaHospitalarApp.Models
{
    public class LogEvento
    {
        public DateTime Timestamp { get; set; }
        public string Evento { get; set; }

        public LogEvento(string evento)
        {
            Timestamp = DateTime.Now;
            Evento = evento;
        }

        public override string ToString()
        {
            return $"[{Timestamp}] {Evento}";
        }
    }
}
