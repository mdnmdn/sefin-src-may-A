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
        private ImportFileInfo _importFile;

        private bool _requestStop = false;
        

        public FileImporter(ImportFileInfo importFile)
        {
            _importFile = importFile;
        }

        public void Import()
        {
            

            Log("Avvio importazione :" + _importFile);
            // System.Threading.Thread.Sleep(4000 + new Random().Next(40) * 1000);

            //if (new Random().Next(3) == 0)
            //{
            //    Log("errore!!!!");
            //    throw new CustomException("Errore!!");
            //}

            for (long i = 0; i < 1000L; i++)
            {
                for (long j = 0; j < 1000000L; j++)
                {
                    var res = Math.Sqrt(i * j);
                }

                if (_requestStop) return;

                //for (long j = 0; j < 5000L; j++)
                //{
                //    var res = Math.Sqrt(i * j);
                //}
                //System.Threading.Thread.Sleep(3);
            }


            _importFile.MoveToFolder(ServiceConfiguration.Instance.CompleteFilePath);

            Log("Importazione completa :" + _importFile);
            // importa il file
        }


        public void RequestStop()
        {
            _requestStop = true;
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
