using System;
using System.Text.Json.Serialization;

namespace SistemaHospitalarApp.Models
{
    public class Alerta
    {
        // Marca cada propriedade para que, mesmo tendo setter privado, o JsonSerializer 
        // consiga “injetar” o valor durante o Deserialize
        [JsonInclude]
        public DateTime DataHora { get; private set; }

        [JsonInclude]
        public string Origem { get; private set; }

        [JsonInclude]
        public string Mensagem { get; private set; }

        [JsonInclude]
        public string Gravidade { get; private set; }


        /// <summary>
        /// Construtor Padrão (necessário para o JsonSerializer “pegar” estas propriedades).
        /// </summary>
        public Alerta()
        {
            // NOTE: ao desserializar, o JsonSerializer vai primeiro criar a instância via este
            // construtor vazio, depois “injetar” cada propriedade marcada com [JsonInclude].
            // Por ora deixamos tudo em defaults; a validação acontecerá em Exibir() ou manualmente.
            Origem = "";
            Mensagem = "";
            Gravidade = "";
            DataHora = DateTime.Now; // Define um valor padrão, poderá ser sobrescrito se o JSON trouxer “DataHora”.
        }


        /// <summary>
        /// Construtor que você usava para criar “na mão” um alerta, validando imediatamente.
        /// </summary>
        [JsonConstructor]
        public Alerta(string origem, string mensagem, string gravidade = "Média")
            : this() // chama o construtor vazio para inicializar DataHora em DateTime.Now
        {
            // Validação manual: garante que, ao usar “new Alerta(…)” no seu código, ninguém crie sem origem/mensagem.
            if (string.IsNullOrWhiteSpace(origem))
                throw new ArgumentException("A origem do alerta não pode ser vazia.");

            if (string.IsNullOrWhiteSpace(mensagem))
                throw new ArgumentException("A mensagem do alerta não pode ser vazia.");

            if (string.IsNullOrWhiteSpace(gravidade))
                throw new ArgumentException("A gravidade do alerta não pode ser vazia.");

            Origem = origem.Trim();
            Mensagem = mensagem.Trim();
            Gravidade = gravidade.Trim();
            DataHora = DateTime.Now;
        }


        /// <summary>
        /// Chamamos essa validação toda vez que formos exibir (ou salvar) o alerta,
        /// para garantir que ninguém desserializou um JSON sem “origem” ou “mensagem”.
        /// </summary>
        public void Validar()
        {
            // Se veio do JSON sem “origem”, temos que lançar exceção.
            if (string.IsNullOrWhiteSpace(Origem))
                throw new ArgumentException("A origem do alerta não pode ser vazia.");

            if (string.IsNullOrWhiteSpace(Mensagem))
                throw new ArgumentException("A mensagem do alerta não pode ser vazia.");

            if (string.IsNullOrWhiteSpace(Gravidade))
                throw new ArgumentException("A gravidade do alerta não pode ser vazia.");
        }


        /// <summary>
        /// Exibe o alerta no console (antes disso, dispara a validação).
        /// </summary>
        public void Exibir()
        {
            Validar();
            Console.WriteLine($"[ALERTA] {DataHora:dd/MM/yyyy HH:mm:ss} | Origem: {Origem} | Mensagem: {Mensagem} | Gravidade: {Gravidade}");
        }

        public override string ToString()
        {
            return $"[{DataHora:dd/MM/yyyy HH:mm:ss}] (ALERTA) Origem: {Origem} | Mensagem: {Mensagem} | Gravidade: {Gravidade}";
        }
    }
}
