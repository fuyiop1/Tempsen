using System;
using System.Web;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Services.Common
{
    class TracingConfiguration
    {
		private static TracingConfigSection	_defaultConfig;

		internal static TracingLevel		_currentLevel;	

        static TracingConfiguration()
        {
            try
            {
                _defaultConfig = (TracingConfigSection)ConfigurationManager.GetSection("LoadingSkyLog");
                if (_defaultConfig == null)
                {
                    _defaultConfig = Activator.CreateInstance<TracingConfigSection>();
                }
            }
            catch {

                _defaultConfig = Activator.CreateInstance<TracingConfigSection>();
                
            }

            _currentLevel = TracingLevel.GetLogLevel(_defaultConfig.Level);
        }

	    public static TracingConfigSection Current
        {
            get { return _defaultConfig; }
        }

		public static int GetLoggerLevel(string loggerName)
		{
			TracingLevel level = TracingConfiguration._currentLevel;
			int levelLength = 0;
			return level._levelValue;
		}
    }
}