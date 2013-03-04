using System.Timers;
using System;
using System.Timers;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShineTech.TempCentre.Platform;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class UserSession
    {
        //private static bool sessionalive;
        //private static Timer usertimer;
        //private static int minutesAlive;

        //public static int MinutesAlive
        //{
        //    get { return UserSession.minutesAlive; }
        //    set { UserSession.minutesAlive = value; }
        //}
        ///// <summary>
        ///// session有效
        ///// </summary>
        //public static bool SessionAlive
        //{
        //    get { return UserSession.sessionalive; }
        //    set { UserSession.sessionalive = value; }
        //}
        
        ///// <summary>
        ///// 时间timer
        ///// </summary>
        //public static Timer UserTimer
        //{
        //    get { return UserSession.usertimer; }
        //    set { UserSession.usertimer = value; }
        //}
        //public static void BeginTimer(int interval, Action<object,EventArgs> action )
        //{
        //    try
        //    {
        //        SessionAlive = false;
        //        UserTimer = new Timer(interval);
        //        UserTimer.Enabled = true;
        //        UserTimer.AutoReset = false;
        //        UserTimer.Elapsed += new ElapsedEventHandler(action);
        //        UserTimer.Start();
        //    }
        //    catch { return; }
        //}
        //public static void  Stop()
        //{
        //    SessionAlive = false;
        //    UserTimer.Stop();
        //}
        //public static void ResetTimer()
        //{
        //    try
        //    {
        //        SessionAlive = false;
        //        usertimer.Stop();
        //        usertimer.Start();
        //    }
        //    catch { return; }
        //}
        private static System.Timers.Timer  _CheckTimer;
        public static System.Timers.Timer CheckTimer
        {
            get { return _CheckTimer; }
            set { _CheckTimer = value; }
        }
        private static MouseKeyHooker _hooker;
        public static void BeginCheckUserSession(Action action)
        {
            try
            {
                if (Common.Policy.InactivityTime != 0)
                {
                    _CheckTimer = new System.Timers.Timer();
                    _CheckTimer.Interval = Common.Policy.InactivityTime * 60 * 1000;
                    //_CheckTimer.Interval = 5000;
                    _CheckTimer.Elapsed += new ElapsedEventHandler((a, b) => action());
                    //_CheckTimer.Elapsed += new ElapsedEventHandler(LogOut);

                    if (_hooker == null)
                        _hooker = new MouseKeyHooker(true, true);
                    _hooker.KeyDown += new KeyEventHandler((a, b) => TimerReset());
                    _hooker.KeyPress += new KeyPressEventHandler((a, b) => TimerReset());
                    _hooker.KeyUp += new KeyEventHandler((a, b) => TimerReset());
                    _hooker.OnMouseActivity += new MouseEventHandler(TimerReset);
                    _CheckTimer.Enabled = false;
                    //form.Close();
                }
            }
            catch
            {
            }
        }
        public static void EndCheckUserSession()
        {
            _hooker.Stop();
            _CheckTimer.Stop();
            
        }
        private static void TimerReset()
        {
            lock (_CheckTimer)
            {
                _CheckTimer.Stop();
                _CheckTimer.Start();
            }
        }
        private static void TimerReset(object sender,MouseEventArgs args)
        {
            lock (_CheckTimer)
            {
                _CheckTimer.Stop();
                _CheckTimer.Start();
            }
        }
        public static void LogOut(object sender, EventArgs args)
        {
            ////干掉当前的进程，重新新的进程
            //Process[] allProcess = Process.GetProcesses();
            //List<Process> pros = Process.GetProcesses().Where(p => p.ProcessName.Split(new char[] { '.' })[0].ToLower() + ".exe" == "TempCentre.exe".ToLower()).ToList();
            //if (pros != null && pros.Count > 0)
            //    pros.ForEach(p =>
            //    {
            //        p.Threads.Cast<ProcessThread>().ToList().ForEach(v =>
            //        {
            //            v.Dispose();
            //        });
            //        p.Kill();
            //    });
            //System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\TempCentre.exe");
            Utils.ShowMessageBox(Messages.SessionExpire, Messages.TitleWarning);
        }
    }
}
