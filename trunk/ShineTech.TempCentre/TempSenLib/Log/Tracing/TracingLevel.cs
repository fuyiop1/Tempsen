using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    class TracingLevel
    {
        internal readonly int		_levelValue;
        internal readonly string	_levelName;

        private TracingLevel(int levelValue, string levelName)
        {
			_levelValue = levelValue;
			_levelName = levelName;        
        }

        internal readonly static TracingLevel Off = new TracingLevel(int.MaxValue, "OFF");
        internal readonly static TracingLevel Error = new TracingLevel(80000, "ERROR");
        internal readonly static TracingLevel Warn = new TracingLevel(50000, "WARN");
        internal readonly static TracingLevel Info = new TracingLevel(30000, "INFO");
        internal readonly static TracingLevel All = new TracingLevel(int.MinValue, "ALL");

        internal static TracingLevel GetLogLevel(string levelName)
        {
            switch (levelName.ToUpper())
            {
                case "OFF":   return TracingLevel.Off;
                case "ERROR": return TracingLevel.Error;
                case "WARN":  return TracingLevel.Warn;
                case "INFO":  return TracingLevel.Info;
                case "ALL":   return TracingLevel.All;
                default:      return TracingLevel.Info;
            }
        }

        internal static bool CanLog(TracingLevel level)
        {
			return level._levelValue >= TracingConfiguration._currentLevel._levelValue;
        }
		
		internal static bool CanLogInfo(TracingImpl impl)
		{
			return Info._levelValue >= impl._levelValue;
		}

		internal static bool CanLogWarn(TracingImpl impl)
		{
			return Warn._levelValue >= TracingConfiguration._currentLevel._levelValue;
		}

		internal static bool CanLogError(TracingImpl impl)
		{
			return Error._levelValue >= TracingConfiguration._currentLevel._levelValue;
		}

	}
}
