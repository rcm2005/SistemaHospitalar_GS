using System;
using System.Collections.Generic;
using SistemaHospitalarApp.Models;

namespace SistemaHospitalarApp.Services
{
    public static class LogService
    {
        private static readonly List<LogEvento> logs = new();

        public static void SalvarLog(LogEvento log)
        {
            logs.Add(log);
            Console.WriteLine(log.ToString());
        }

        public static List<LogEvento> ObterLogs()
        {
            return logs;
        }
    }
}
