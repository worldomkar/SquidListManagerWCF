//-----------------------------------------------------------------------
// <copyright file="SquidManager_Host.cs" company="_">
// SquidManager_Host
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace SquidManager_Host
{
    using System;
    using System.IO;
    using System.ServiceModel;
    using System.ServiceProcess;

    /// <summary>
    /// SquidManager_Host uses a WCF self-hosting feature to create service instance implemented as a windows service
    /// </summary>
    public partial class SquidManager_Host : ServiceBase
    {
        /// <summary>
        /// Host for the SquidManager service
        /// </summary>
        private static ServiceHost squidManagerHost = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SquidManager_Host"/> class.
        /// </summary>
        public SquidManager_Host()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Windows Service OnStart event
        /// </summary>
        /// <param name="args">Arguments from environment</param>
        protected override void OnStart(string[] args)
        {
            if (squidManagerHost != null)
            {
                squidManagerHost.Close();
            }

            try
            {
                squidManagerHost = new ServiceHost(typeof(global::SquidManager.SquidManager));
            }
            catch
            {
                Uri baseAddress = new Uri("http://127.0.0.1:5656/squid_manager/");
                squidManagerHost = new ServiceHost(typeof(global::SquidManager.SquidManager), baseAddress);
            }

            try
            {
                squidManagerHost.Open();
            }
            catch (Exception e)
            {
                try
                {
                    FileStream of = new FileStream(
                        "C:\\Users\\Omkar\\Documents\\Visual Studio 2012\\Projects\\Squid Manager\\SquidManager\\bin\\Debug\\errorlog",
                        FileMode.Open,
                        FileAccess.Write,
                        FileShare.ReadWrite);
                    StreamWriter swF = new StreamWriter(of);
                    swF.WriteLine(e.Message);
                    swF.Flush();
                    swF.Close();
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Windows Service OnStop event
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                if (squidManagerHost != null)
                {
                    squidManagerHost.Close();
                    squidManagerHost = null;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
