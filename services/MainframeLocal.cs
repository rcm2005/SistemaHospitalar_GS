using System;
using SistemaHospitalarApp.Models;

namespace SistemaHospitalarApp.Services
{
    public class MainframeLocal
    {
        public void ProcessarFalha(Falha falha)
        {
            try
            {
                if (falha == null)
                {
                    Console.WriteLine("Falha inválida para processar.");
                    return;
                }

                var log = new LogEvento(falha.Mensagem, DateTime.Now, falha.Tipo);
                LogService.SalvarLog(log);

                var alerta = new Alerta("MainframeLocal", $"Falha crítica: {falha.Mensagem}", "Crítica");
                alerta.Exibir();
                LogService.SalvarLog(new LogEvento("Alerta emitido com sucesso", DateTime.Now, "ALERTA"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar falha no MainframeLocal: {ex.Message}");
            }
        }
    }
}
