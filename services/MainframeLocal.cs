using System.Collections.Generic;
using SistemaHospitalarApp.Models;

namespace SistemaHospitalarApp.Services
{
    public class MainframeLocal
    {
        public List<Falha> FalhasRegistradas { get; private set; } = new();
        public List<Alerta> AlertasGerados { get; private set; } = new();
        public List<LogEvento> Logs { get; private set; } = new();

        public void RegistrarFalha(Falha falha)
        {
            FalhasRegistradas.Add(falha);
            Logs.Add(new LogEvento($"Falha registrada: {falha.Tipo}"));
        }

        public void GerarAlerta(string nivel, string mensagem)
        {
            var alerta = new Alerta(nivel, mensagem);
            AlertasGerados.Add(alerta);
            alerta.Exibir();
            Logs.Add(new LogEvento($"Alerta gerado: {mensagem}"));
        }

        public void ExibirLogs()
        {
            foreach (var log in Logs)
                Console.WriteLine(log);
        }
    }
}
