using System.Timers;
using System;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class UserSession
    {
        private static bool sessionalive;
        private static Timer usertimer;
        private static int minutesAlive;

        public static int MinutesAlive
        {
            get { return UserSession.minutesAlive; }
            set { UserSession.minutesAlive = value; }
        }
        /// <summary>
        /// session有效
        /// </summary>
        public static bool SessionAlive
        {
            get { return UserSession.sessionalive; }
            set { UserSession.sessionalive = value; }
        }
        
        /// <summary>
        /// 时间timer
        /// </summary>
        public static Timer UserTimer
        {
            get { return UserSession.usertimer; }
            set { UserSession.usertimer = value; }
        }
        public static void BeginTimer(int interval, Action<object,EventArgs> action )
        {
            try
            {
                SessionAlive = false;
                UserTimer = new Timer(interval);
                UserTimer.Enabled = true;
                UserTimer.AutoReset = false;
                UserTimer.Elapsed += new ElapsedEventHandler(action);
                UserTimer.Start();
            }
            catch { return; }
        }
        public static void  Stop()
        {
            SessionAlive = false;
            UserTimer.Stop();
        }
        //private static void DisposeSession(object sender,ElapsedEventArgs args)
        //{
        //    try
        //    {
        //        SessionAlive = false;
        //    }
        //    catch { return; }
        //}
        public static void ResetTimer()
        {
            try
            {
                SessionAlive = false;
                usertimer.Stop();
                usertimer.Start();
            }
            catch { return; }
        }
    }
}
