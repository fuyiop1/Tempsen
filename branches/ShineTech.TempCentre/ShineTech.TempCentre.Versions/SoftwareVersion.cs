/* ***********************************************
 * Description: SoftwareVersion
 * CreateBy wangfei 
 * CreateOn 8/31/2011 10:08:13 AM 
 * Email: wangf@shinetechchina.com
 * ***********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShineTech.TempCentre.Versions
{
    public static class SoftwareVersion
    {
        public const SoftwareVersions Version = SoftwareVersions.Pro;
        public static SoftwareVersions VersionForAuthentication = SoftwareVersions.Pro;
    }

    public enum SoftwareVersions { Init, S, Pro }
}
/**********************************
╭━━灬╮╭━━∞╮ .︵ 
┃⌒　⌒┃┃⌒　⌒┃ (の) 
┃┃　┃┃┃▂　▂┃╱︶ 
╰━━━〇〇━━━〇 
**********************************/