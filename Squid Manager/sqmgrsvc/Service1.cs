using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using sqmgr;

namespace sqmgrsvc
{
    public partial class Service1 : ServiceBase
    {
        ServiceHost host;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Type serviceType = typeof(Program);
            host = new ServiceHost(serviceType);
            host.Open();
        }

        protected override void OnStop()
        {
            if (null != host)
                host.Close();
        }
    }
}
