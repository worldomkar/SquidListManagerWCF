//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="_">
// Program
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace SquidManager
{
    using System;
    using System.ServiceModel;
    using System.Threading;

    /// <summary>
    /// SquidManager entry point class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// SquidManager entry point
        /// </summary>
        /// <param name="args">Currently ignored command line arguments</param>
        public static void Main(string[] args)
        {
            SquidManager sm = new SquidManager();
            ////Uri baseAddress = new Uri("http://127.0.0.1:5656/squid_manager/");
            ServiceHost squidManagerHost = new ServiceHost(sm); ////, baseAddress);
            squidManagerHost.Open();
            sm.WaitForWorkers();
            squidManagerHost.Close();
        }
    }
}
