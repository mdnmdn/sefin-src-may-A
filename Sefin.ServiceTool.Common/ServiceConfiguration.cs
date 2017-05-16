using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.ServiceTool.Common
{
    public class ServiceConfiguration
    {
        #region Singleton

        static ServiceConfiguration _instance;

        static public ServiceConfiguration Instance
        {
            get
            {
                return _instance ?? (_instance = new ServiceConfiguration());
            }
        }

        #endregion

        private ServiceConfiguration()
        {
            ImportFilePath = NormalizeConfigPath("Sefin.Importer.ImportFilePath");
            StagingFilePath = NormalizeConfigPath("Sefin.Importer.StagingFilePath");
            CompleteFilePath = NormalizeConfigPath("Sefin.Importer.CompleteFilePath");
        }


        public string ImportFilePath { get; private set; }
        public string StagingFilePath { get; private set; }
        public string CompleteFilePath { get; private set; }


        private static string NormalizeConfigPath(string config)
        {
            var path = ConfigurationManager.AppSettings[config];
            if (!Path.IsPathRooted(path))
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            return path;
        }

    }
}
