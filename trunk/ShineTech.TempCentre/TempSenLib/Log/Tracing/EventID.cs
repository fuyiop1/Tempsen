using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common {
	/// <summary>
	/// EVENT ID�ĳ���ѡ��Χ
	/// </summary>
	/// <remarks>
	/// 1000 - 1059 ϵͳ��������	Info����
	/// 1060 - 1099 ϵͳ��������	Error����
	/// 1200 -	  ����ϵͳ����EVENTID
	/// </remarks>
	public static class EventID {

		/// <summary>��������</summary>
		public const int SERVICE_START		= 1000;

		/// <summary>����ֹͣ</summary>
		public const int SERVICE_STOP		= 1001;

		/// <summary>������ͣ</summary>
		public const int SERVICE_PAUSE		= 1002;

		/// <summary>����ָ�</summary>
		public const int SERVICE_RESUME		= 1003;

		/// <summary>���øı�</summary>
		public const int CONFIG_FAILED		= 1004;

		/// <summary>�������</summary>
		public const int SERVICE_ERROR		= 1005;

		/// <summary>���øı�</summary>
		public const int CONFIG_CHANGE		= 1012;

		/// <summary>�����쳣��ֹ</summary>
		public const int SERVICE_ABORT		= 1060;

		/// <summary>�����ڲ�����</summary>
		public const int SERVICE_HINT		= 1063;

		/// <summary>����ʧ��</summary>
		public const int NETWORK_FAILED		= 1061;

		/// <summary>REMOTINGʧ��</summary>
		public const int REMOTING_FAILED	= 1062;
	}
}
