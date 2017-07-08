//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="_">
// Squid_Monitor
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace Squid_Monitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Entry point of Squid Monitor
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SquidMonitor());
        }
    }
}
