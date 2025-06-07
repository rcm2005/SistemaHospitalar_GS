using System;
using System.Collections.Generic;
using System.IO;
using SistemaHospitalarApp.Models;

namespace SistemaHospitalarApp.Services
{
    public static class LogService
    {
        private static readonly List<LogEvento> logs = new();
        private const string LogFilePath = "logs.txt";

        public static void SalvarLog(LogEvento log)
        {
            logs.Add(log);
            Console.WriteLine(log.ToString());
            try
            {
                File.AppendAllText(LogFilePath, log.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao gravar log em arquivo: {ex.Message}");
            }
        }

        public static List<LogEvento> ObterLogs()
        {
            return logs;
        }

        public static void CarregarLogs()
        {
            logs.Clear();

            if (File.Exists(LogFilePath))
            {
                try
                {
                    var linhas = File.ReadAllLines(LogFilePath);
                    foreach (var linha in linhas)
                    {
                        var log = ParseLinhaParaLogEvento(linha);
                        if (log != null)
                        {
                            logs.Add(log);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERRO] Falha ao carregar logs do arquivo: {ex.Message}");
                }
            }
        }

        private static LogEvento? ParseLinhaParaLogEvento(string linha)
        {
            try
            {
                var dataInicio = linha.IndexOf('[') + 1;
                var dataFim = linha.IndexOf(']');
                var dataHoraStr = linha.Substring(dataInicio, dataFim - dataInicio);

                var restante = linha.Substring(dataFim + 10).Trim();

                var partes = restante.Split('|');
                var origem = partes[0].Replace("Origem:", "").Trim();
                var mensagem = partes[1].Replace("Mensagem:", "").Trim();
                var gravidade = partes[2].Replace("Gravidade:", "").Trim();

                return new LogEvento
                {
                    DataHora = DateTime.Parse(dataHoraStr),
                    Origem = origem,
                    Mensagem = mensagem,
                    Gravidade = gravidade
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
