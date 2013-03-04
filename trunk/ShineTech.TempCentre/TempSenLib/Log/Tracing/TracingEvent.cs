using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Services.Common
{
    class TracingEvent
    {
        public string WorkStation = string.Empty;        
        public string LoggerName = string.Empty;
        public TracingLevel Level;
        public string Message = string.Empty;
        public string ThreadName = string.Empty;
        public int ThreadId;
        public string ProcessName = string.Empty;
        public int ProcessId;
        public DateTime TimeStamp;
        public Exception Error;
        public string From = string.Empty;
        public string To = string.Empty;
        public string CallId = string.Empty;
        public string Cseq = string.Empty;

        internal TracingEvent()
        {
            Process process  = Process.GetCurrentProcess();
            this.WorkStation = Environment.MachineName;
            this.ProcessName = process.ProcessName;
            this.ProcessId   = process.Id;
            this.ThreadName  = Thread.CurrentThread.Name;
            this.ThreadId    = Thread.CurrentThread.ManagedThreadId;
            this.TimeStamp   = DateTime.Now;

			if (ThreadName == null)
				ThreadName = string.Empty;
        }
    }
}