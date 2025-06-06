using System;

namespace SistemaHospitalarApp.Models
{
    public class Falha
    {
        public string Mensagem { get; private set; }
        public string Tipo { get; private set; }

        public Falha(string mensagem, string tipo)
        {
            if (string.IsNullOrWhiteSpace(mensagem))
                throw new ArgumentException("A mensagem da falha não pode ser vazia.");

            if (string.IsNullOrWhiteSpace(tipo))
                throw new ArgumentException("O tipo da falha não pode ser vazio.");

            Mensagem = mensagem;
            Tipo = tipo;
        }

        public override string ToString()
        {
            return $"Falha [{Tipo}]: {Mensagem}";
        }
    }
}
