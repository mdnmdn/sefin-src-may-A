using Sefin.ServiceTool.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.AnacenImporter
{
    public class FileImporter
    {
        private string _filePath;

        

        public FileImporter(string filePath)
        {
            _filePath = filePath;
        }

        public void Process()
        {
            var fileName = Path.GetFileName(_filePath);

            Log("Avvio importazione :" + fileName);
            // System.Threading.Thread.Sleep(4000 + new Random().Next(40) * 1000);

            if (new Random().Next(3) == 0)
            {
                Log("errore!!!!");
                throw new CustomException("Errore!!");
            }
                    

            for(long i = 0; i < 100000L; i++)
            {
                for (long j = 0; j < 10000L; j++)
                {
                    var res = Math.Sqrt(i*j);
                }
            }

            var stagingFilePath = Tools.MoveFileToFolder(_filePath, ServiceConfiguration.Instance.CompleteFilePath);

            Log("Importazione completa :" + fileName);
            // importa il file
        }


        #region logger 

        ILogger _logger;

        public void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        void Log(string message)
        {
            if (_logger != null)
                _logger.Log(message);
        }

        #endregion

    }
}
