using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using sqmgr_wcf_svc;

namespace selfhost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://127.0.0.1:5656/squid_manager");
            ServiceHost sh = new ServiceHost(typeof(SqMgrSvc), baseAddress);
            //ServiceHost sh = new ServiceHost(typeof(SqMgrSvc));
            try
            {
                sh.Open();
            }
            catch (Exception e)
            { }
            Console.ReadKey();
        }
    }
}
