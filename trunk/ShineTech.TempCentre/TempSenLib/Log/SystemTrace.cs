using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
	/// <summary>
	///		记录Window Event Log
	/// </summary>
    public static class SystemTrace
    {
		private static string _eventSource = String.Empty;

		/// <summary>
		///		设置默认的eventSource
		/// </summary>
		/// <param name="eventSource">默认的eventSource, 建议设置为进程名或服务名</param>
		public static void SetDefaultEventSource(string eventSource)
		{
			_eventSource = eventSource;
			if (!EventLog.SourceExists(_eventSource))
				EventLog.CreateEventSource(_eventSource, "Application");
		}

		/// <summary>
		///		记录Info级别的EventLog
		/// </summary>
		/// <param name="eventId">Event ID,请参考Guideline中的取值范围</param>
		/// <param name="eventSource">Event Source</param>
		/// <param name="message">Event内容</param>
        public static void Info(int eventId, string eventSource, string message) 
		{
            WriteEventLog(eventId, eventSource, message, EventLogEntryType.Information);
        }

		/// <summary>
		///		记录Info级别的EventLog, 使用设置好的EventSource
		/// </summary>
		/// <param name="eventId">Event ID,请参考Guideline中的取值范围</param>
		/// <param name="message">Event内容</param>
		/// <remarks>使用本函数前,请先使用SetDefaultEventSource设置默认的EventSource</remarks>
		public static void Info(int eventId, string message) 
		{
			WriteEventLog(eventId, message, EventLogEntryType.Information);
		}

		/// <summary>
		///		记录Error级别的EventLog
		/// </summary>
		/// <param name="eventId">Event ID,请参考Guideline中的取值范围</param>
		/// <param name="eventSource">Event Source</param>
		/// <param name="message">Event内容</param>
        public static void Error(int eventId, string eventSource, string message)
        {
            WriteEventLog(eventId, eventSource, message, EventLogEntryType.Error);
        }

		/// <summary>
		///		记录Error级别的EventLog, 使用设置好的EventSource
		/// </summary>
		/// <param name="eventId">Event ID,请参考Guideline中的取值范围</param>
		/// <param name="message">Event内容</param>
		/// <remarks>使用本函数前,请先使用SetDefaultEventSource设置默认的EventSource</remarks>
		public static void Error(int eventId, string message) 
		{
			WriteEventLog(eventId, message, EventLogEntryType.Error);
		}

        private static void WriteEventLog(int id, string eventSource, string message, EventLogEntryType logType) 
		{
			try {
				if (!EventLog.SourceExists(eventSource))
					EventLog.CreateEventSource(eventSource, "Application");
				EventLog.WriteEntry(eventSource, message, logType, id);
			} catch (Exception ex) {
				try {
					Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Write EventLog Failed: \r\n" + ex.ToString());
				} catch {
					// Noting can do 
				}
			}
        }

		private static void WriteEventLog(int id, string message, EventLogEntryType logType) 
		{
			try {
				if (_eventSource == string.Empty)
					_eventSource = Process.GetCurrentProcess().ProcessName;
				EventLog.WriteEntry(_eventSource, message, logType, id);
			} catch (Exception ex) {
				try {
					Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Write EventLog Failed. \r\n" + ex.ToString());
				} catch {
					// Noting can do 
				}
			}
		}
    }
}