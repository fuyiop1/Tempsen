using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common {
	/// <summary>
	/// EVENT ID的常数选择范围
	/// </summary>
	/// <remarks>
	/// 1000 - 1059 系统公共定义	Info级别
	/// 1060 - 1099 系统公共定义	Error级别
	/// 1200 -	  各子系统定义EVENTID
	/// </remarks>
	public static class EventID {

		/// <summary>服务启动</summary>
		public const int SERVICE_START		= 1000;

		/// <summary>服务停止</summary>
		public const int SERVICE_STOP		= 1001;

		/// <summary>服务暂停</summary>
		public const int SERVICE_PAUSE		= 1002;

		/// <summary>服务恢复</summary>
		public const int SERVICE_RESUME		= 1003;

		/// <summary>配置改变</summary>
		public const int CONFIG_FAILED		= 1004;

		/// <summary>服务错误</summary>
		public const int SERVICE_ERROR		= 1005;

		/// <summary>配置改变</summary>
		public const int CONFIG_CHANGE		= 1012;

		/// <summary>服务异常终止</summary>
		public const int SERVICE_ABORT		= 1060;

		/// <summary>服务内部错误</summary>
		public const int SERVICE_HINT		= 1063;

		/// <summary>网络失败</summary>
		public const int NETWORK_FAILED		= 1061;

		/// <summary>REMOTING失败</summary>
		public const int REMOTING_FAILED	= 1062;
	}
}
