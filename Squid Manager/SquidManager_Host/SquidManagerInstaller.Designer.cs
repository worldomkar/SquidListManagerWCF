//-----------------------------------------------------------------------
// <copyright file="SquidManagerInstaller.Designer.cs" company="_">
// SquidManagerInstaller
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace SquidManager
{
    /// <summary>
    /// SquidManager service installer
    /// </summary>
    public partial class SquidManagerInstaller
    {
        /// <summary>
        /// Required designer variable
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// SquidManager ServiceProcessInstaller
        /// </summary>
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;

        /// <summary>
        /// SquidManager ServiceInstaller
        /// </summary>
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "Squid Manager monitors squid log for unique domains and helps them categorize.";
            this.serviceInstaller1.DisplayName = "Squid Manager";
            this.serviceInstaller1.ServiceName = "SquidManager";
            this.serviceInstaller1.ServicesDependedOn = new string[] {
        ""};
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // SquidManagerInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

        }

        #endregion
    }
}