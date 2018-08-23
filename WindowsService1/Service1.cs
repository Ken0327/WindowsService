using System;
using System.Diagnostics;
using System.Globalization;
using System.ServiceProcess;
using System.Timers;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        static System.Timers.Timer timer1;
        static string _ScheduledRunningTime = "12:35 AM";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //axWMP.URL = @"D:\Ken\Videos\CYMATICS_Science_NigelStanford.mp4";
            try
            {
                timer1 = new System.Timers.Timer();
                timer1.Interval = 1 * 10 * 1000;//Every one minute
                timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                timer1.Start();
                timer1.Enabled = true;
                Library.writeErrorLog("Timer windows service started");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                //Displays and Logs Message
                //_loggerDetails.LogMessage = ex.ToString();
                //_writeLog.LogDetails(_loggerDetails.LogLevel_Error, _loggerDetails.LogMessage);
            }
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
            Library.writeErrorLog("Timer windows service stopped");
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var _CurrentTime = DateTime.Now.ToString("hh:mm tt", new CultureInfo("en-US"));
            if (_CurrentTime == _ScheduledRunningTime)
            {
                UpdateITMXP();
            }
            Library.writeErrorLog("Timer jobs has been done successfully");
        }

        static void UpdateITMXP()
        {
            Console.WriteLine("Start run .bat");
            Process proc = null;
            try
            {
                string batDir = string.Format(@"C:\SW\Update");
                proc = new Process();
                proc.StartInfo.WorkingDirectory = batDir;
                proc.StartInfo.FileName = "test.bat";//"Update-ITMXP_task.bat";
                proc.StartInfo.CreateNoWindow = false;
                proc.StartInfo.Verb = "runas";
                proc.Start();
                proc.WaitForExit();
                Console.WriteLine("Bat file executed !!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
            }
            //Console.ReadKey();
        }
    }
}
