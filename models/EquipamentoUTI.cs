using System;

namespace SistemaHospitalarApp.Models
{
    public class EquipamentoUTI
    {
        public string Nome { get; set; }
        public bool EstaFuncionando { get; set; }

        public EquipamentoUTI(string nome)
        {
            Nome = nome;
            EstaFuncionando = true;
        }

        public bool SimularFalha()
        {
            var random = new Random();
            // 20% de chance de falha para test drive
            EstaFuncionando = random.NextDouble() > 0.2;
            return !EstaFuncionando;
        }

        public override string ToString()
        {
            return $"{Nome} - Status: {(EstaFuncionando ? "Operacional" : "Com falha")}";
        }
    }
}
