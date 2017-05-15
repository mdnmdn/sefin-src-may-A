using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.ServiceTool.Common
{
    public class ServiceLogger
    {

        #region Singleton

        static ServiceLogger _instance;

        static public ServiceLogger Instance
        {
            get
            {
                return _instance ?? (_instance = new ServiceLogger());
            }
        }

        #endregion

        DateTime _lastTimeStamp = DateTime.Now;

        public Action<string> Logger { get; set; }

        public void Log(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);

            var delta = DateTime.Now.Subtract(_lastTimeStamp);
            _lastTimeStamp = DateTime.Now;


            if (Logger != null)
            {
                var msg = String.Format("{0:0.00}] {1} ", delta.TotalMilliseconds / 1000, message);
                Logger(msg);
            }
        }
    }
}
