using System;
using System.Collections.Generic;
using System.Text;
using Services.Common;

namespace Debuglog
{
    public class Class1
    {
        private static ITracing _tracing = TracingManager.GetTracing(typeof(Class1));

        public static void process()
        {
            int a = 2;
            int b = 3;
            int sum = a + b;
            _tracing.Info(a.ToString() + "+" + b.ToString() + "=" + sum.ToString());

            int d = 0;
            int split = 0;
            try {
                split = sum / d;
            }catch(Exception ex)
            {
                _tracing.Error(ex,"can not split by zero .");
            }

        }
    }
}
