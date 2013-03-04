using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
	/// <summary>
	///		����Tracing�Ľӿ�
	/// <seealso cref="TracingManager"/>
	/// <remarks>���TracingManager.GetTracing(Type type)�������һ�����õĽӿ�</remarks>
	/// </summary>
    public interface ITracing {
		/// <summary>
		///		��¼Info�����Tracing
		/// </summary>
		/// <param name="message">Tracing Content</param>
		/// <remarks>�����Ҫstring.Format(),��ʹ��InfoFmt()����</remarks>
        void Info(string message);



		/// <summary>
		///		��¼Info�����Tracing, ����Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="message">Tracing Content</param>
		/// <remarks>�����Ҫstring.Format(),��ʹ��InfoFmtEx()����</remarks>
        void Info(Exception exception, string message);

		/// <summary>
		///		��¼Info�����Tracing
		/// </summary>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void InfoFmt(string format, params object[] args);


		/// <summary>
		///		��¼Info�����Tracing, ����Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void InfoFmtEx(Exception exception, string format, params object[] args);

		/// <summary>
		///		��¼Warn�����Tracing
		/// </summary>
		/// <param name="message">Tracing Content</param>
		/// <remarks>�����Ҫstring.Format(),��ʹ��WarnFmt()����</remarks>
        void Warn(string message);

		/// <summary>
		///		��¼Warn�����Tracing, ����Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="message">Tracing Content</param>
		/// <remarks>�����Ҫstring.Format(),��ʹ��WarnFmtEx()����</remarks>
        void Warn(Exception exception, string message);

		

		/// <summary>
		///		��¼Warn�����Tracing
		/// </summary>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void WarnFmt(string format, params object[] args);


		/// <summary>
		///		��¼Warn�����Tracing, ����Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void WarnFmtEx(Exception exception, string format, params object[] args);



		/// <summary>
		///		��¼Error�����Tracing
		/// </summary>
		/// <param name="message">Tracing Content</param>
		/// <remarks>�����Ҫstring.Format(),��ʹ��ErrorFmt()����</remarks>
        void Error(string message);



		/// <summary>
		///		��¼Error�����Tracing, ����Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="message">Tracing Content</param>
		/// <remarks>�����Ҫstring.Format(),��ʹ��ErrorFmtEx()����</remarks>
        void Error(Exception exception, string message);


		/// <summary>
		///		��¼Error�����Tracing
		/// </summary>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void ErrorFmt(string format, params object[] args);


		/// <summary>
		///		��¼Error�����Tracing, ����Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void ErrorFmtEx(Exception exception, string format, params object[] args);

    }
}
