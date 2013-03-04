using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
	/// <summary>
	///		TracingManager
	/// </summary>
	/// <seealso cref="ITracing"/>
	/// <remarks>从GetTracing方法获得一个可用的ITracin接口</remarks>
    public static class TracingManager
    {
		/// <summary>
		///		获取一个ITracing接口
		/// </summary>
		/// <param name="type">记录Tracing时带有的Class信息</param>
		/// <returns>可用的Tracing接口</returns>
		/// <remarks>建议在每个class中获取接口进行记录</remarks>
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