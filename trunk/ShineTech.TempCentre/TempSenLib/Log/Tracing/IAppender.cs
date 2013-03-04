using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    interface IAppender
    {
        void DoAppend(TracingEvent[] logEvent);
    }
}