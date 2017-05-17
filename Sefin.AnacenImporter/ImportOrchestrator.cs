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

        HashSet<string> _companyRegistry = new HashSet<string>();

        int _numRunningThreads = 0;

        const int MaxConcurrentThread = 30;

        ILogger _logger;

        public void SetLogger(ILogger logger)
        {
            _logger = logger;
        }


        public void Process()
        {
            try
            {
                if (_numRunningThreads >= MaxConcurrentThread) return;

                var files = ListFilesToProcess();

                if (files.Length > 0)
                {
                    //Log("File da processare: " + String.Join(", ", files));

                    foreach(var file in files)
                    {
                        if (_numRunningThreads >= MaxConcurrentThread) return;
                        
                        var fileInfo = PreprocessImportFile(file);

                        // salta il file se la company è già in elaborazione
                        if (_companyRegistry.Contains(fileInfo.Company)) continue;

                        LogThreads();

                        StartProcess(fileInfo);
                    }
                }
            }catch(Exception ex)
            {
                Log("Errore in process: " + ex);
            }

        }

        

        private void StartProcess(ImportFileInfo file)
        {
            file.MoveToFolder(ServiceConfiguration.Instance.StagingFilePath);
            
            //Log("Sposto il file in " + stagingFilePath);

            var importer = new FileImporter(file);
            importer.SetLogger(_logger);

            var processingThread = new Thread(() =>
            {
                try
                {
                    importer.Import();
                }catch(Exception ex)
                {
                    Log("Errore su thread");
                }
                finally
                {
                    lock (_lock)
                    {
                        _numRunningThreads--;
                        _companyRegistry.Remove(file.Company);
                    }

                    //Interlocked.Decrement(ref _numRunningThreads);
                    
                    LogThreads();
                }
            });

            lock (_lock)
            {
                _numRunningThreads++;
                _companyRegistry.Add(file.Company);
            }

            //Interlocked.Increment(ref _numRunningThreads);


            processingThread.Start();

        }

        private ImportFileInfo PreprocessImportFile(string fullPath)
        {
            var path = Path.GetDirectoryName(fullPath);
            var companyFolder = Path.GetFileName(path);
            return new ImportFileInfo
            {
                FullPath = fullPath,
                Company = companyFolder
            };
        }

        private void LogThreads()
        {
            Log("Thread attivi: " + _numRunningThreads);
        }

        private string[] ListFilesToProcess()
        {
            var filePath = ServiceConfiguration.Instance.ImportFilePath;
            return Directory.GetFiles(filePath, "*.txt",SearchOption.AllDirectories);
        }



        void Log(string message)
        {
            if (_logger != null)
                _logger.Log(message);
        }

        
    }
}
