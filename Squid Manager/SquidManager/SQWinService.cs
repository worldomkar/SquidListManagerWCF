using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using sqmgr_wcf_svc;

namespace SquidManager
{
    public partial class SQWinService : ServiceBase
    {
        internal static ServiceHost sh = null;

        public SQWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (null != sh)
            {
                sh.Close();
            }
            Uri baseAddress = new Uri("http://127.0.0.1:5656/squid_manager/");
            sh = new ServiceHost(typeof(SqMgrSvc), baseAddress);
            //sh = new ServiceHost(typeof(SqMgrSvc));
            try
            {
                sh.Open();
            }
            catch (Exception e)
            {
                try
                {
                    FileStream of = new FileStream("C:\\Users\\Omkar\\Documents\\Visual Studio 2012\\Projects\\Squid Manager\\SquidManager\\bin\\Debug\\errorlog",
                        FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    StreamWriter swF = new StreamWriter(of);
                    swF.WriteLine(e.Message);
                    swF.Flush();
                    swF.Close();
                }
                catch (Exception ee)
                {
                }
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (null != sh)
                {
                    sh.Close();
                    sh = null;
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
