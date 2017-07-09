//-----------------------------------------------------------------------
// <copyright file="SquidManagerInstaller.cs" company="_">
// SquidManagerInstaller
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace SquidManager
{
    using System.ComponentModel;

    /// <summary>
    /// SquidManager service installer
    /// </summary>
    [RunInstaller(true)]
    public partial class SquidManagerInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SquidManagerInstaller"/> class.
        /// </summary>
        public SquidManagerInstaller()
        {
            this.InitializeComponent();
        }
    }
}
