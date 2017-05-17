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

        Dictionary<string, ProcessWrapper> _processRegistry 
            = new Dictionary<string, ProcessWrapper>();

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
                if (_processRegistry.Count >= MaxConcurrentThread) return;

                var files = ListFilesToProcess();

                if (files.Length > 0)
                {
                    //Log("File da processare: " + String.Join(", ", files));

                    foreach(var file in files)
                    {
                        if (_processRegistry.Count >= MaxConcurrentThread) return;
                        
                        var fileInfo = PreprocessImportFile(file);

                        // salta il file se la company è già in elaborazione
                        if (_processRegistry.ContainsKey(fileInfo.Company)) continue;

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
            var processWrapper = new ProcessWrapper(file);
            processWrapper.SetLogger(_logger);

            _processRegistry.Add(file.Company, processWrapper);

            processWrapper.ProcessClose += endingImportFile =>
            {
                _processRegistry.Remove(endingImportFile.Company);
            };
            
            processWrapper.Start();
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
            Log("Thread attivi: " + _processRegistry.Count);
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
