using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Services.Common;

namespace Services.Common {
    class TracingConfigSection: ConfigurationSection {

        [ConfigurationProperty("version", DefaultValue = 1, IsRequired = true)]
        public int Version
        {
            get { return (int)this["version"]; }
        }

        [ConfigurationProperty("level", DefaultValue = "info", IsRequired = true)]
        public string Level
        {
            get { return (string)this["level"]; }
            set { this["level"] = value; }
        }

        [ConfigurationProperty("TextAppender")]
        public TextAppenderElement TextAppender
        {
            get { return (TextAppenderElement)this["TextAppender"]; }
            set { this["TextAppender"] = value; }
        }
		
    }


    class TextAppenderElement: ConfigurationElement {
        [ConfigurationProperty("enable", DefaultValue = true, IsRequired = true)]
        public bool Enable {
            get { return (bool)this["enable"]; }
        }

        [ConfigurationProperty("console", DefaultValue = false, IsRequired = false)]
        public bool Console {
            get { return (bool)this["console"]; }
        }

        [ConfigurationProperty("path", DefaultValue = "log", IsRequired = false)]
        public string Path {
            get { return (string)this["path"]; }
        }
    }




}
