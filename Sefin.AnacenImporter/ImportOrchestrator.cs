using Sefin.ServiceTool.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.AnacenImporter
{
    public class ImportOrchestrator
    {


        public void Process()
        {
            Log("wow!");



        }

        void Log(string message)
        {
            ServiceLogger.Instance.Log(message);
        }
    }
}
