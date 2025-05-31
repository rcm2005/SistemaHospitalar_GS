using System;
using System.Collections.Generic;
using SistemaHospitalarApp.Models;

namespace SistemaHospitalarApp.Services
{
    public class SistemaHospitalar
    {
        private readonly MainframeLocal _mainframe = new();
        private readonly Sincronizador _sincronizador = new();
        private readonly List<EquipamentoUTI> _equipamentos = new();

        public void InicializarEquipamentos()
        {
            _equipamentos.Add(new EquipamentoUTI("Respirador"));
            _equipamentos.Add(new EquipamentoUTI("Monitor Cardíaco"));
            _equipamentos.Add(new EquipamentoUTI("Bomba de Infusão"));
        }

        public void RodarMonitoramento()
        {
            _sincronizador.VerificarConexao();
            _sincronizador.Sincronizar(_mainframe);

            foreach (var eq in _equipamentos)
            {
                bool falhou = eq.SimularFalha();
                Console.WriteLine(eq);

                if (falhou)
                {
                    var falha = new Falha("Equipamento", $"{eq.Nome} apresentou falha.");
                    _mainframe.RegistrarFalha(falha);
                    _mainframe.GerarAlerta("Crítico", $"Falha detectada no equipamento: {eq.Nome}");
                }
            }
        }

        public void MostrarLogs()
        {
            Console.WriteLine("\n== LOG DE EVENTOS ==");
            _mainframe.ExibirLogs();
        }
    }
}
