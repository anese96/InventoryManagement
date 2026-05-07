using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Logger
{
    public static class ErrorLogger
    {
       // private static string logFilePath = "log.txt";

        public static void Log(string message)
        {
            string folder = "Logs";

            // créer le dossier si n'existe pas
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, "log.txt");
            try
            {
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                File.AppendAllText(path, logMessage + Environment.NewLine);

                //string logMessage = $"{DateTime.Now} - {message}";
                //File.AppendAllText(path, logMessage + Environment.NewLine);
            }
            catch
            {
                // éviter crash si erreur de log
            }
        }
    }
}
