using System;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Collections.Generic;
using Sefin.AnacenImporter;
using Sefin.ServiceTool.Common;

namespace Sefin.ServiceTool
{
    public partial class SchedulerService : ServiceBase
    {
        /// <summary>
        /// main thread
        /// </summary>
        protected Thread _thread;


        /// <summary>
        /// 
        /// </summary>
        protected bool _continue = true;

        /// <summary>
        /// 
        /// </summary>
        private int _joinTimeMs = 2000;

        /// <summary>
        /// ctor: init component and local data
        /// </summary>
        public SchedulerService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// starts the main thread
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            _continue = true;
            _thread = new Thread(Process);
            _thread.IsBackground = true;
            _thread.Start();
        }

        /// <summary>
        /// tells the main thread to stop and waits for its termination
        /// </summary>
        protected override void OnStop()
        {
            _continue = false;
            _thread.Join(_joinTimeMs);
            if (_thread.ThreadState != ThreadState.Stopped) { 
                _thread.Abort();
                _thread.Join(_joinTimeMs);
            }
        }

        protected void Process()
        {
            var orchestrator = new ImportOrchestrator();

            Log("  - Starting process -");
            while (true)
            {
                Log("  Performing...-");

                orchestrator.Process();

                Thread.Sleep(2000);
                if (!_continue) return;
            }
            Log("  - Process completed -");
        }


        public bool IsRunning { get { return _thread != null && _thread.ThreadState == ThreadState.Running; } }

        /// <summary>
        /// starts the service
        /// </summary>
        public void StartService()
        {
            Log("calling startservice");
            this.OnStart(null);
        }

        /// <summary>
        /// stops the service
        /// </summary>
        public void StopService()
        {
            Log("calling stopservice");
            this.OnStop();
        }

        void Log(string message)
        {
            ServiceLogger.Instance.Log(message);
        }
    }
}
