using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
	/// <summary>
	///		��¼Window Event Log
	/// </summary>
    public static class SystemTrace
    {
		private static string _eventSource = String.Empty;

		/// <summary>
		///		����Ĭ�ϵ�eventSource
		/// </summary>
		/// <param name="eventSource">Ĭ�ϵ�eventSource, ��������Ϊ�������������</param>
		public static void SetDefaultEventSource(string eventSource)
		{
			_eventSource = eventSource;
			if (!EventLog.SourceExists(_eventSource))
				EventLog.CreateEventSource(_eventSource, "Application");
		}

		/// <summary>
		///		��¼Info�����EventLog
		/// </summary>
		/// <param name="eventId">Event ID,��ο�Guideline�е�ȡֵ��Χ</param>
		/// <param name="eventSource">Event Source</param>
		/// <param name="message">Event����</param>
        public static void Info(int eventId, string eventSource, string message) 
		{
            WriteEventLog(eventId, eventSource, message, EventLogEntryType.Information);
        }

		/// <summary>
		///		��¼Info�����EventLog, ʹ�����úõ�EventSource
		/// </summary>
		/// <param name="eventId">Event ID,��ο�Guideline�е�ȡֵ��Χ</param>
		/// <param name="message">Event����</param>
		/// <remarks>ʹ�ñ�����ǰ,����ʹ��SetDefaultEventSource����Ĭ�ϵ�EventSource</remarks>
		public static void Info(int eventId, string message) 
		{
			WriteEventLog(eventId, message, EventLogEntryType.Information);
		}

		/// <summary>
		///		��¼Error�����EventLog
		/// </summary>
		/// <param name="eventId">Event ID,��ο�Guideline�е�ȡֵ��Χ</param>
		/// <param name="eventSource">Event Source</param>
		/// <param name="message">Event����</param>
        public static void Error(int eventId, string eventSource, string message)
        {
            WriteEventLog(eventId, eventSource, message, EventLogEntryType.Error);
        }

		/// <summary>
		///		��¼Error�����EventLog, ʹ�����úõ�EventSource
		/// </summary>
		/// <param name="eventId">Event ID,��ο�Guideline�е�ȡֵ��Χ</param>
		/// <param name="message">Event����</param>
		/// <remarks>ʹ�ñ�����ǰ,����ʹ��SetDefaultEventSource����Ĭ�ϵ�EventSource</remarks>
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