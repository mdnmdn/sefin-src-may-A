using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sefin.ServiceTool.Common;

namespace Sefin.AnacenImporter
{
    public class ProcessWrapper
    {
        public ImportFileInfo ImportFile { get; private set; }

        public Thread ImportThread { get; private set; }

        public FileImporter Importer { get; private set; }

        public event Action<ImportFileInfo> ProcessClose;

        public ProcessWrapper(ImportFileInfo file)
        {
            ImportFile = file;
        }

        public void Start()
        {
            Importer = new FileImporter(ImportFile);
            Importer.SetLogger(_logger);

            var processingThread = new Thread(() =>
            {
                try
                {
                    Importer.Import();
                }
                catch (Exception ex)
                {
                    Log("Errore su thread");
                }
                finally
                {
                    if (ProcessClose != null)
                        ProcessClose(ImportFile);
                }
            });
            
         
            processingThread.Start();
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
