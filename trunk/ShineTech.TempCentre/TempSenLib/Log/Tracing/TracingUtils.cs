using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Services.Common {
	/// <summary>
	///		格式化 Tracing Utils
	/// </summary>
	public static class TracingUtils {
		/// <summary>
		///		格式化Exception
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static string FormatException(Exception ex) 
		{
			if (ex == null)
				return "";

			try {
				StringBuilder msg = new StringBuilder(1000);
				msg.AppendFormat("\r\n* Catched Exception: [{0}]: \"{1}\" in <{2}> \r\n", ex.GetType().ToString(), ex.Message, ex.Source);
				FormatInnnerException(msg, ex.InnerException, 1);

				if (ex.StackTrace != null) {
					msg.AppendFormat("- StackTrace: {0}\r\n", ex.StackTrace.ToString());
					msg.Append("--- End of exception stack trace ---\r\n");
				}
				return msg.ToString();
			} catch {
				try {
					return ex.ToString();
				} catch {
					return "exception!!!";
				}
			}
		}

		private static void FormatInnnerException(StringBuilder msg, Exception ex, int n)
		{
			if (ex == null)
				return;

			if (ex.InnerException != null)
				FormatInnnerException(msg, ex.InnerException, n + 1);
			else {
				msg.AppendFormat("- Inner Exception({0}):  [{1}]: \"{2}\" in <{3}> \r\n", 
					n,
					ex.GetType().ToString(),
					ex.Message,
					ex.Source);

				if (ex.StackTrace != null) {
					msg.AppendFormat("- Inner StackTrack({0}): {1}\r\n",
						n,
						ex.StackTrace.ToString());
					msg.AppendFormat("--- End of InnerException StackTrace({0}) ---\r\n", n);
				}
			}
		}

		public static string FormatCurrentThreads()
		{
			StringBuilder buf = new StringBuilder();

			foreach (ProcessThread  thread in Process.GetCurrentProcess().Threads) {
				
				buf.AppendFormat("<{0}({1})>:{2}\r\n", 
					thread.Id,
					thread.ThreadState,
					thread.StartTime);				
			}
			return buf.ToString();
		}

		public static string FormatCurrentThreadPool()
		{
			return null;
		}
	}
}
