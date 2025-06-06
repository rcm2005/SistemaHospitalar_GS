using System;
using System.Collections.Generic;

namespace SistemaHospitalarApp.Models
{
    public class Paciente
    {
        private string _nome;
        private string _cpf;
        private int _idade;
        private string _diagnostico;

        public string Nome
        {
            get => _nome;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("O nome do paciente não pode ser vazio.");
                _nome = value.Trim();
            }
        }

        public string CPF
        {
            get => _cpf;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("O CPF não pode ser vazio.");
                _cpf = value.Trim();
            }
        }

        public int Idade
        {
            get => _idade;
            set
            {
                if (value < 0 || value > 130)
                    throw new ArgumentException("Idade inválida.");
                _idade = value;
            }
        }

        public string Diagnostico
        {
            get => _diagnostico;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("O diagnóstico não pode ser vazio.");
                _diagnostico = value.Trim();
            }
        }

        public List<Laudo> Laudos { get; private set; } = new();

        public Paciente(string nome, string cpf, int idade, string diagnostico)
        {
            Nome = nome;
            CPF = cpf;
            Idade = idade;
            Diagnostico = diagnostico;
        }

        // Construtor vazio para desserialização
        public Paciente() { }

        public override string ToString()
        {
            return $"Nome: {Nome} | CPF: {CPF} | Idade: {Idade} | Diagnóstico: {Diagnostico}";
        }
    }
}
