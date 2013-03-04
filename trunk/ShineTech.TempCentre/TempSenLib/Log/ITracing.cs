using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
	/// <summary>
	///		处理Tracing的接口
	/// <seealso cref="TracingManager"/>
	/// <remarks>请从TracingManager.GetTracing(Type type)方法获得一个可用的接口</remarks>
	/// </summary>
    public interface ITracing {
		/// <summary>
		///		记录Info级别的Tracing
		/// </summary>
		/// <param name="message">Tracing Content</param>
		/// <remarks>如果需要string.Format(),请使用InfoFmt()代替</remarks>
        void Info(string message);



		/// <summary>
		///		记录Info级别的Tracing, 附带Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="message">Tracing Content</param>
		/// <remarks>如果需要string.Format(),请使用InfoFmtEx()代替</remarks>
        void Info(Exception exception, string message);

		/// <summary>
		///		记录Info级别的Tracing
		/// </summary>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void InfoFmt(string format, params object[] args);


		/// <summary>
		///		记录Info级别的Tracing, 附带Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void InfoFmtEx(Exception exception, string format, params object[] args);

		/// <summary>
		///		记录Warn级别的Tracing
		/// </summary>
		/// <param name="message">Tracing Content</param>
		/// <remarks>如果需要string.Format(),请使用WarnFmt()代替</remarks>
        void Warn(string message);

		/// <summary>
		///		记录Warn级别的Tracing, 附带Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="message">Tracing Content</param>
		/// <remarks>如果需要string.Format(),请使用WarnFmtEx()代替</remarks>
        void Warn(Exception exception, string message);

		

		/// <summary>
		///		记录Warn级别的Tracing
		/// </summary>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void WarnFmt(string format, params object[] args);


		/// <summary>
		///		记录Warn级别的Tracing, 附带Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void WarnFmtEx(Exception exception, string format, params object[] args);



		/// <summary>
		///		记录Error级别的Tracing
		/// </summary>
		/// <param name="message">Tracing Content</param>
		/// <remarks>如果需要string.Format(),请使用ErrorFmt()代替</remarks>
        void Error(string message);



		/// <summary>
		///		记录Error级别的Tracing, 附带Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="message">Tracing Content</param>
		/// <remarks>如果需要string.Format(),请使用ErrorFmtEx()代替</remarks>
        void Error(Exception exception, string message);


		/// <summary>
		///		记录Error级别的Tracing
		/// </summary>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void ErrorFmt(string format, params object[] args);


		/// <summary>
		///		记录Error级别的Tracing, 附带Exception
		/// </summary>
		/// <param name="exception">Catched Exception</param>
		/// <param name="format">The Format String</param>
		/// <param name="args">params</param>
		void ErrorFmtEx(Exception exception, string format, params object[] args);

    }
}
