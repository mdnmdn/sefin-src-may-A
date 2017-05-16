using Sefin.ServiceTool.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sefin.AnacenImporter
{
    public class ImportOrchestrator
    {

        object _lock = new object();

        int _numTheadRunning = 0;

        const int MaxConcurrentThread = 3;

        ILogger _logger;

        public void SetLogger(ILogger logger)
        {
            _logger = logger;
        }


        public void Process()
        {
            try
            {
                if (_numTheadRunning >= MaxConcurrentThread) return;

                var files = ListFilesToProcess();

                if (files.Length > 0)
                {
                    Log("File da processare: " + String.Join(", ", files));

                    foreach(var file in files)
                    {
                        if (_numTheadRunning >= MaxConcurrentThread) return;

                        LogThreads();

                        StartProcess(file);
                    }
                }
            }catch(Exception ex)
            {
                Log("Errore in process: " + ex);
            }

        }

        private void StartProcess(string file)
        {
            var stagingFilePath = Tools.MoveFileToFolder(file, ServiceConfiguration.Instance.StagingFilePath);

            //Log("Sposto il file in " + stagingFilePath);

            var importer = new FileImporter(stagingFilePath);
            importer.SetLogger(_logger);

            var processingThread = new Thread(() =>
            {
                try
                {
                    importer.Process();
                }catch(Exception ex)
                {
                    Log("Errore su thread");
                }
                finally
                {
                    lock (_lock)
                    {
                        _numTheadRunning--;
                    }
                    LogThreads();
                }
            });

            lock (_lock)
            {
                _numTheadRunning++;
            }

            processingThread.Start();

        }

        private void LogThreads()
        {
            Log("Thread attivi: " + _numTheadRunning);
        }

        private string[] ListFilesToProcess()
        {
            var filePath = ServiceConfiguration.Instance.ImportFilePath;
            return Directory.GetFiles(filePath, "*.txt");
        }



        void Log(string message)
        {
            if (_logger != null)
                _logger.Log(message);
        }

        
    }
}
