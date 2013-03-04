using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Services.Common
{
    class TextAppender: IAppender 
	{
		#region Members
		private string		_path = string.Empty;
		private static bool	_console = false;

		internal bool		Enabled;
		internal bool		BackupForDbError;
		#endregion

		#region IAppender Mode
		public TextAppender(string path, bool console) {
			if (path != null && path != string.Empty) {
				if (!path.EndsWith("\\") ) {
					_path = path + "\\";
				} else
					_path = path;

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
			} else {
				_path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			}
			_console = console;
		}
		#endregion

		#region Appender Method
		public void DoAppend(TracingEvent[] logEvents) 
		{
			int retryCount = 0;
			while (retryCount < 3) {
				string path = retryCount == 0 ? 
					_path + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt"
				:
					_path + DateTime.Now.ToString("yyyy-MM-dd HH [") + retryCount + "].txt";

				try {
					 using (FileStream fs = File.Open(path, FileMode.Append, FileAccess.Write, FileShare.Read)) {
						StreamWriter sw = new StreamWriter(fs);

						foreach (TracingEvent evt in logEvents) {
							string log = GetLogString(evt);
							sw.Write(log);

							if (_console)
								Console.WriteLine(log);
						}
						sw.Close();
					}
				} catch (IOException) {
					retryCount++;
					continue;
				} catch (Exception ex) {
					SystemTrace.Error(EventID.SERVICE_HINT, "Tracing", TracingUtils.FormatException(ex));
				}

				return;
			}
			SystemTrace.Error(EventID.SERVICE_HINT, "Tracing", "IOException after try max count.");
        }

        internal static string GetLogString(TracingEvent logEvent)
		{
            StringBuilder sb = new StringBuilder();

			sb.Append("***\r\n-------------------------------------------------------------------------------\r\n");

			sb.AppendFormat("{0} [{1}] Tracing by <{2}> at \\\\{3} \r\n",
				logEvent.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff"),
				logEvent.Level._levelName,
				logEvent.LoggerName,
				logEvent.WorkStation);

            sb.AppendFormat(
				"Process: <{0}> - \"{1}\" Thread: <{2}> - \"{3}\" \r\n",
				logEvent.ProcessId,
				logEvent.ProcessName, 
				logEvent.ThreadId,
				logEvent.ThreadName
			);

            sb.AppendFormat("Message: {0}\r\n", logEvent.Message);

            if (logEvent.Error != null) {
				sb.Append(TracingUtils.FormatException(logEvent.Error));
            }

            if (logEvent.From != string.Empty) {
                sb.AppendFormat(
					"[SIP INFO]: From: <{0}>\r\nTo: <{1}>\r\nCallId: <{2}>\r\nCSeq: <{3}>\r\n",
                    logEvent.From, logEvent.To, logEvent.CallId, logEvent.Cseq
				);
            }

            sb.Append("-------------------------------------------------------------------------------\r\n");

            return sb.ToString();
		}
		#endregion
	}
}