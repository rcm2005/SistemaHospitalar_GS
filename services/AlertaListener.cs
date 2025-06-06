using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SistemaHospitalarApp.Models;

namespace SistemaHospitalarApp.Services
{
    public static class AlertaListener
    {
        private static HttpListener? _listener;

        public static void Iniciar()
        {
            try
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add("http://*:5000/alerta/");
                _listener.Start();
                Console.WriteLine("[HTTP] Escutando alertas em http://localhost:5000/alerta/");

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            var contexto = _listener.GetContext();
                            var requisicao = contexto.Request;
                            var resposta = contexto.Response;

                            if (requisicao.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
                            {
                                using var reader = new StreamReader(requisicao.InputStream, requisicao.ContentEncoding);
                                var json = reader.ReadToEnd();

                                Console.WriteLine($"[DEBUG] JSON recebido: {json}");

                                var alerta = JsonSerializer.Deserialize<Alerta>(json, jsonOptions);
                                if (alerta != null)
                                {
                                    try
                                    {
                                        alerta.Exibir();
                                        LogService.SalvarLog(new LogEvento(alerta.ToString(), DateTime.Now, "ALERTA"));
                                    }
                                    catch (ArgumentException validaEx)
                                    {
                                        Console.WriteLine($"[ERRO] Alerta inválido: {validaEx.Message}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("JSON inválido para alerta.");
                                }

                                resposta.StatusCode = 200;
                                var buffer = Encoding.UTF8.GetBytes("Alerta recebido com sucesso");
                                resposta.OutputStream.Write(buffer, 0, buffer.Length);
                            }
                            else
                            {
                                resposta.StatusCode = 405;
                            }

                            resposta.Close();
                        }
                        catch (Exception exLoop)
                        {
                            Console.WriteLine($"Erro no listener de alertas: {exLoop.Message}");
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha ao iniciar listener de alertas: {ex.Message}");
            }
        }
    }
}
