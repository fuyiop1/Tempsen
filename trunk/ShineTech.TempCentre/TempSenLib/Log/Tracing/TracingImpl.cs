using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Services.Common
{
    class TracingImpl: ITracing
    {
        private string				_loggerName;
		internal int				_levelValue; 

        internal TracingImpl(Type type)
        {
			if (type == null) {
				_loggerName = "NotAssigned";
				_levelValue = TracingConfiguration._currentLevel._levelValue;
			} else {
				_loggerName = type.FullName;
				_levelValue = TracingConfiguration.GetLoggerLevel(_loggerName);
			}
        }

        #region ITracing Members
        public void Info(string message)
        {
			try {
				if (TracingLevel.CanLogInfo(this))
					WriteLog(TracingLevel.Info, message);
			} catch (Exception ex) {
				Trace.Write(ex);
			}
        }
		
        public void Info(string from, string to, string callId, string cseq, string message)
        {
			try {
				if (TracingLevel.CanLogInfo(this))
					WriteLog(TracingLevel.Info, message, from, to, callId, cseq);
			} catch (Exception ex) {
				Trace.Write(ex);
			}
        }

        public void Info(Exception exception, string message)
        {
			try {
				if (TracingLevel.CanLogInfo(this))
					WriteLog(TracingLevel.Info, message, exception);
			} catch (Exception ex) {
				Trace.Write(ex);
			}

        }

        public void Info(string from, string to, string callId, string cseq, Exception exception, string message)
        {
			try {
				if (TracingLevel.CanLogInfo(this))
					WriteLog(TracingLevel.Info, message, exception, from, to, callId, cseq);
			} catch (Exception ex) {
				Trace.Write(ex);
			}

        }

		public void InfoFmt(string message, params object[] args)
		{
			try {
				if (TracingLevel.CanLogInfo(this))
					WriteLog(TracingLevel.Info, string.Format(message, args));
			} catch (Exception ex) {
				Trace.Write(ex);
			}
		}

		public void InfoFmtEx(Exception exception, string message, params object[] args)
		{
			try {
				if (TracingLevel.CanLogInfo(this))
					WriteLog(TracingLevel.Info, string.Format(message, args), exception);
			} catch (Exception ex) {
				Trace.Write(ex);
			}
		}

		public void InfoFmtSip(string from, string to, string callId, string cseq, string message, params object[] args)
		{
			try {
				if (TracingLevel.CanLogInfo(this))
					WriteLog(TracingLevel.Info, string.Format(message, args), from, to, callId, cseq);
			} catch (Exception ex) {
				Trace.Write(ex);
			}
		}

		public void InfoFmtSipEx(string from, string to, string callId, string cseq, Exception exception, string message, params object[] args)
		{
			try {
				if (TracingLevel.CanLogInfo(this))
					WriteLog(TracingLevel.Info, string.Format(message, args), exception, from, to, callId, cseq);
			} catch (Exception ex) {
				Trace.Write(ex);
			}
		}

        public void Warn(string message)
        {
			try {
				if (TracingLevel.CanLogWarn(this))
					WriteLog(TracingLevel.Warn, message);
			} catch (Exception ex) {
				Trace.Write(ex);
			}
        }
		
        public void Warn(string from, string to, string callId, string cseq, string message)
        {
			try {
				if (TracingLevel.CanLogWarn(this))
					WriteLog(TracingLevel.Warn, message, from, to, callId, cseq);
			} catch (Exception ex) {
				Trace.Write(ex);
			}
        }

        public void Warn(Exception exception, string message)
        {
			try {
				if (TracingLevel.CanLogWarn(this))
					WriteLog(TracingLevel.Warn, message, exception);
			} catch (Exception ex) {
				Trace.Write(ex);
			}
        }



		public void WarnFmt(string message, params object[] args)
		{
			try {
				if (TracingLevel.CanLogWarn(this))
					WriteLog(TracingLevel.Warn, string.Format(message, args));
			} catch (Exception ex) {
				Trace.Write(ex);
			}
		}

		public void WarnFmtEx(Exception exception, string message, params object[] args)
		{
			try {
				if (TracingLevel.CanLogWarn(this))
					WriteLog(TracingLevel.Warn, string.Format(message, args), exception);
			} catch (Exception ex) {
				Trace.Write(ex);
			}
		}




        public void Error(string message)
        {
			try {
				if (TracingLevel.CanLogError(this))
					WriteLog(TracingLevel.Error, message);
			} catch (Exception ex) {
				Trace.Write(ex);
			}

        }
		


        public void Error(Exception exception, string message)
        {
			try {
				if (TracingLevel.CanLogError(this))
					WriteLog(TracingLevel.Error, message, exception);
			} catch (Exception ex) {
				Trace.Write(ex);
			}

        }



		public void ErrorFmt(string message, params object[] args)
		{
			try {
				if (TracingLevel.CanLogError(this))
					WriteLog(TracingLevel.Error, string.Format(message, args));
			} catch (Exception ex) {
				Trace.Write(ex);
			}
		}

		public void ErrorFmtEx(Exception exception, string message, params object[] args)
		{
			try {
				if (TracingLevel.CanLogError(this))
					WriteLog(TracingLevel.Error, string.Format(message, args), exception);
			} catch (Exception ex) {
				Trace.Write(ex);
			}
		}



        #endregion

		#region WriterLogs
		private void WriteLog(TracingLevel level, string message)
        {
            TracingEvent logEvent = new TracingEvent();
            logEvent.Message = message;
            logEvent.LoggerName = _loggerName;
            logEvent.Level = level;

			DoAppend(logEvent);
		}

        private void WriteLog(TracingLevel level, string message, Exception e)
        {
            TracingEvent logEvent = new TracingEvent();
            logEvent.Message = message;
            logEvent.LoggerName = _loggerName;
            logEvent.Level = level;
            logEvent.Error = e;

			DoAppend(logEvent);       
		}

        private void WriteLog(TracingLevel level, string message, string from, string to, string callId, string cseq)
        {
            TracingEvent logEvent = new TracingEvent();
            logEvent.Message = message;
            logEvent.LoggerName = _loggerName;
            logEvent.Level = level;
            logEvent.From = from;
            logEvent.To = to;
            logEvent.CallId = callId;
            logEvent.Cseq = cseq;

			DoAppend(logEvent);      
		}

        private void WriteLog(TracingLevel level, string message, Exception e, string from, string to, string callId, string cseq)
        {
            TracingEvent logEvent = new TracingEvent();
            logEvent.Message = message;
            logEvent.LoggerName = _loggerName;
            logEvent.Level = level;
            logEvent.Error = e;
            logEvent.From = from;
            logEvent.To = to;
            logEvent.CallId = callId;
            logEvent.Cseq = cseq;

			DoAppend(logEvent);
		}
		#endregion

		private void DoAppend(TracingEvent evt) 
		{
			_thread.Enqueue(evt);
		}

		private static TracingCacheThread _thread = TracingCacheThread.Instance;
    }
}