/* ***********************************************
 * Description: LogAction
 * CreateBy wangfei 
 * CreateOn 8/25/2011 1:59:44 PM 
 * Email: wangf@shinetechchina.com
 * ***********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShineTech.TempCentre.BusinessFacade
{
    public  class LogAction
    {
        public static readonly string AddUser = "Add user";
        public static readonly string EditUser = "Edit user";
        public static readonly string ChangePassword = "Change password";
        public static readonly string ChangeGroup = "Change group";
        public static readonly string Logon = "Login";
        public static readonly string Logoff = "Logout";
        public static readonly string AssignRights = "Assign rights";
        public static readonly string DisableUser = "Disable user";
        public static readonly string LockUser = "Lock user";
        public static readonly string UnlockUser = "Unlock user";
        public static readonly string ConfigurateDevice = "Configurate device";
        public static readonly string Saverecord = "Save record";
        public static readonly string Signrecord = "Sign record";
        public static readonly string Commentrecord = "Comment record";
        public static readonly string Deleterecord = "Delete record";
        public static readonly int SystemAuditTrail = 0;
        public static readonly int AnalysisAuditTrail = 1;
    }
}
/**********************************
╭━━灬╮╭━━∞╮ .︵ 
┃⌒　⌒┃┃⌒　⌒┃ (の) 
┃┃　┃┃┃▂　▂┃╱︶ 
╰━━━〇〇━━━〇 
**********************************/