

/////Funcionalidade futura/////





using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SistemaHospitalarApp.Models;
using SistemaHospitalarApp.Services;

namespace SistemaHospitalarApp.Services
{
    public class Sincronizador
    {
        private const string UrlSync = "http://localhost:6000/sync";
        private static readonly HttpClient _httpClient = new HttpClient();

        public bool Conectado { get; private set; }

        public Sincronizador()
        {
            Conectado = false;
        }

        public async Task SincronizarAsync()
        {
            try
            {
                // Monta o pacote de dados locais: Pacientes + Logs
                var pacientes = PacienteService.ObterPacientes();
                var logs = LogService.ObterLogs();

                var pacote = new
                {
                    Timestamp = DateTime.Now,
                    Pacientes = pacientes,
                    Logs = logs
                };

                string json = JsonSerializer.Serialize(pacote, new JsonSerializerOptions { WriteIndented = true });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Envia via POST para a URL definida
                var response = await _httpClient.PostAsync(UrlSync, content);

                if (response.IsSuccessStatusCode)
                {
                    Conectado = true;
                    Console.WriteLine("[✔] Dados sincronizados com o servidor na nuvem.");
                    LogService.SalvarLog(new LogEvento("Sincronização bem-sucedida com a nuvem.", DateTime.Now, "SINCRONIZAÇÃO"));
                }
                else
                {
                    Conectado = false;
                    Console.WriteLine($"[✘] Erro na sincronização (HTTP {(int)response.StatusCode}). Modo offline.");
                    LogService.SalvarLog(new LogEvento($"Falha na sincronização. Código HTTP: {(int)response.StatusCode}", DateTime.Now, "ERRO"));
                }
            }
            catch (Exception ex)
            {
                Conectado = false;
                Console.WriteLine($"[✘] Sem conexão com a nuvem. Modo offline. Erro: {ex.Message}");
                LogService.SalvarLog(new LogEvento($"Erro durante sincronização: {ex.Message}", DateTime.Now, "ERRO"));
            }
        }
    }
}
