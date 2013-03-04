using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
	/// <summary>
	///		TracingManager
	/// </summary>
	/// <seealso cref="ITracing"/>
	/// <remarks>��GetTracing�������һ�����õ�ITracin�ӿ�</remarks>
    public static class TracingManager
    {
		/// <summary>
		///		��ȡһ��ITracing�ӿ�
		/// </summary>
		/// <param name="type">��¼Tracingʱ���е�Class��Ϣ</param>
		/// <returns>���õ�Tracing�ӿ�</returns>
		/// <remarks>������ÿ��class�л�ȡ�ӿڽ��м�¼</remarks>
		/// <example>private static ITracing _tracing = TracingManager.GetTracing(typeof(MyClass));</example>
        public static ITracing GetTracing(Type type)
        {
            return new TracingImpl(type);
        }

		public static void FlushCache()
		{
			TracingCacheThread.Instance.FlushCache();
		}
    }
}