using System;
using SistemaHospitalarApp.Services;

namespace SistemaHospitalarApp.Services
{
    public class Sincronizador
    {
        public bool Conectado { get; private set; }

        public Sincronizador()
        {
            Conectado = true; // simula que começa conectado
        }

        public void VerificarConexao()
        {
            var random = new Random();
            // 30% de chance de perder conexão
            Conectado = random.NextDouble() > 0.3;
        }

        public void Sincronizar(MainframeLocal mainframe)
        {
            if (Conectado)
            {
                Console.WriteLine("[✔] Dados sincronizados com a nuvem.");
                mainframe.Logs.Add(new Models.LogEvento("Sincronização bem-sucedida com a nuvem."));
            }
            else
            {
                Console.WriteLine("[✘] Sem conexão com a nuvem. Operando em modo offline.");
                mainframe.Logs.Add(new Models.LogEvento("Falha na sincronização. Modo offline ativado."));
            }
        }
    }
}
