using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace Services.Common {
	class TracingCacheThread 
	{
		#region Constant
		public const int				DefaultSleep = 1000;
		public const int				MaxDelaySpan = 3000;
		public const int				BatchCount = 32;
		public const int				MaxQueueCapacity = 16384;
		#endregion

		#region Members
		protected Queue<TracingEvent>	_queue = new Queue<TracingEvent>();

		protected DateTime				_lastTime;
		protected TextAppender			_textAppender;

		protected Thread				_thread;
		#endregion

		#region Singlton Pattern
		private static TracingCacheThread _instance = new TracingCacheThread();

		public static TracingCacheThread Instance
		{
			get { return _instance; }
		}

		private TracingCacheThread()
		{
			if (TracingConfiguration.Current == null) {
				SystemTrace.Error(EventID.CONFIG_FAILED, "LoadingSky:Tracing Configuration Failed");
				throw new Exception("Get LoadingSky:Tracing Configuration Failed, Please Check Config!");
			}

			string path = TracingConfiguration.Current.TextAppender.Path;
			bool console = TracingConfiguration.Current.TextAppender.Console;
			_textAppender = new TextAppender(path, console);
			_textAppender.Enabled = TracingConfiguration.Current.TextAppender.Enable;
			_textAppender.BackupForDbError = false;

			_thread = new Thread(ThreadProc);
			_thread.IsBackground = true;
			_thread.Start();
		}
		#endregion

		public void Enqueue(TracingEvent evt)
		{
			if (_queue.Count > MaxQueueCapacity)
				return;

			lock (_queue) {
				_queue.Enqueue(evt);
			}
		}

		private int GetReadyCount()
		{
			if (_queue.Count > BatchCount)
				return BatchCount;
			else {
				TimeSpan span = DateTime.Now.Subtract(_lastTime);
				if (span.TotalMilliseconds > MaxDelaySpan)
					return _queue.Count;
				else
					return 0;
			}
		}

		private void ThreadProc()
		{
			try {
				while (true) {
					int readyCount = GetReadyCount();
					
					if (readyCount > 0) {
						DoWriteLogs(readyCount);
						_lastTime = DateTime.Now;
					} else 
						Thread.Sleep(DefaultSleep);

				}
			} catch (ThreadAbortException) {
				FlushCache();
			} catch (Exception ex) {
				try {
					SystemTrace.Error(EventID.SERVICE_ERROR, GetTracingSource(), ex.ToString());
				} catch {
					// ...
					// I can do nothing
				}
			}
		}

		public void FlushCache()
		{
			try {
				int count;
				while ((count = _queue.Count) > 0) {
					if (count >= BatchCount)
						DoWriteLogs(BatchCount);
					else
						DoWriteLogs(count);
				}
			} catch (Exception ex) {
				SystemTrace.Error(EventID.SERVICE_ERROR, 
					GetTracingSource(),
					"Tracing Flush Cache Failed: \r\n" + TracingUtils.FormatException(ex));
			}
		}

		private void DoWriteLogs(int count)
		{
			//
			// Dequeue Events
			TracingEvent[] evts = new TracingEvent[count];

			lock (_queue) {
				for (int i = 0; i < count; i++) {
					evts[i] =  _queue.Dequeue();
				}
			}

			if (!_textAppender.Enabled)
				return;

			

			//
			// Writer Text Tracing
			if (_textAppender.Enabled || _textAppender.BackupForDbError) {
				try {
					_textAppender.DoAppend(evts);
				} catch (Exception ex) {
					SystemTrace.Error(EventID.SERVICE_ERROR, GetTracingSource(), TracingUtils.FormatException(ex));
				}
			}
		}

		private string GetTracingSource()
		{
			try {
				Process process = Process.GetCurrentProcess();
				return string.Format("Tracing {0}", process.ProcessName);
			} catch (Exception) {
				return "Tracing";
			}
		}
	}
}