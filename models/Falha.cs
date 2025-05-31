using System;

namespace SistemaHospitalarApp.Models
{
    public class Falha
    {
        public DateTime DataHora { get; set; }
        public string Tipo { get; set; }  // Ex: "Energia", "Conexão", "Equipamento"
        public string Descricao { get; set; }

        public Falha(string tipo, string descricao)
        {
            DataHora = DateTime.Now;
            Tipo = tipo;
            Descricao = descricao;
        }

        public override string ToString()
        {
            return $"[{DataHora}] Falha: {Tipo} - {Descricao}";
        }
    }
}
