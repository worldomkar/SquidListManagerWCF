//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="_">
// Program
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace SquidManager
{
    using System.ServiceProcess;

    /// <summary>
    /// Entry point to SquidManager_Host
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            ServiceBase[] serviceToRun;
            serviceToRun = new ServiceBase[] 
            { 
                new SquidManager_Host.SquidManager_Host()
            };
            ServiceBase.Run(serviceToRun);
        }
    }
}
