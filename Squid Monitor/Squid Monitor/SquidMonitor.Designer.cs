//-----------------------------------------------------------------------
// <copyright file="SquidMonitor.Designer.cs" company="_">
// Squid_Monitor
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace Squid_Monitor
{
    using System;

    /// <summary>
    /// SquidMonitor class is primary dialog derived from "Form"
    /// </summary>
    public partial class SquidMonitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadNewDomainsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ignoreAllNewToolStripMenuItem;
        private System.Windows.Forms.TextBox txtSearchBox;
        private System.Windows.Forms.Button btnClearSearch;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SquidMonitor));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadNewDomainsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ignoreAllNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSearchBox = new System.Windows.Forms.TextBox();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.connectingMessage = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Location = new System.Drawing.Point(12, 27);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(491, 666);
            this.treeView1.TabIndex = 0;
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeView1_ItemDrag);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeView1_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeView1_DragEnter);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.TreeView1_DragOver);
            this.treeView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TreeView1_KeyUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Enabled = false;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadToolStripMenuItem,
            this.reloadNewDomainsToolStripMenuItem,
            this.ignoreAllNewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(515, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.reloadToolStripMenuItem.Text = "Reload Squid";
            this.reloadToolStripMenuItem.ToolTipText = "Ask Squid service to reload trust and block list files and re-configure.";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.ReloadToolStripMenuItem_Click);
            // 
            // reloadNewDomainsToolStripMenuItem
            // 
            this.reloadNewDomainsToolStripMenuItem.Name = "reloadNewDomainsToolStripMenuItem";
            this.reloadNewDomainsToolStripMenuItem.Size = new System.Drawing.Size(141, 20);
            this.reloadNewDomainsToolStripMenuItem.Text = "Re-query NewDomains";
            this.reloadNewDomainsToolStripMenuItem.ToolTipText = "Get a list of un-categorized domains from SquidManager WCF service.";
            this.reloadNewDomainsToolStripMenuItem.Click += new System.EventHandler(this.ReloadNewDomainsToolStripMenuItem_Click);
            // 
            // ignoreAllNewToolStripMenuItem
            // 
            this.ignoreAllNewToolStripMenuItem.Name = "ignoreAllNewToolStripMenuItem";
            this.ignoreAllNewToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.ignoreAllNewToolStripMenuItem.Text = "Ignore all New";
            this.ignoreAllNewToolStripMenuItem.ToolTipText = "Move all new domains to ignore list (Blocked list->Global->Inactive)";
            this.ignoreAllNewToolStripMenuItem.Click += new System.EventHandler(this.IgnoreAllNewToolStripMenuItem_Click);
            // 
            // txtSearchBox
            // 
            this.txtSearchBox.Enabled = false;
            this.txtSearchBox.ForeColor = System.Drawing.Color.Gray;
            this.txtSearchBox.Location = new System.Drawing.Point(336, 4);
            this.txtSearchBox.Name = "txtSearchBox";
            this.txtSearchBox.Size = new System.Drawing.Size(141, 20);
            this.txtSearchBox.TabIndex = 2;
            this.txtSearchBox.TextChanged += new System.EventHandler(this.TxtSearchBox_TextChanged);
            this.txtSearchBox.GotFocus += new System.EventHandler(this.TxtSearchBox_GotFocus);
            this.txtSearchBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtSearchBox_KeyPress);
            this.txtSearchBox.LostFocus += new System.EventHandler(this.TxtSearchBox_LostFocus);
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Enabled = false;
            this.btnClearSearch.Location = new System.Drawing.Point(483, 4);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(32, 20);
            this.btnClearSearch.TabIndex = 3;
            this.btnClearSearch.Text = "X";
            this.btnClearSearch.UseVisualStyleBackColor = true;
            this.btnClearSearch.Click += new System.EventHandler(this.BtnClearSearch_Click);
            // 
            // connectingMessage
            // 
            this.connectingMessage.AutoSize = true;
            this.connectingMessage.BackColor = System.Drawing.SystemColors.Window;
            this.connectingMessage.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectingMessage.Location = new System.Drawing.Point(80, 135);
            this.connectingMessage.Name = "connectingMessage";
            this.connectingMessage.Size = new System.Drawing.Size(355, 29);
            this.connectingMessage.TabIndex = 4;
            this.connectingMessage.Text = "Waiting for SquidManager service..";
            // 
            // SquidMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 705);
            this.Controls.Add(this.connectingMessage);
            this.Controls.Add(this.btnClearSearch);
            this.Controls.Add(this.txtSearchBox);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SquidMonitor";
            this.Text = "Squid Manager - Connecting";
            this.Activated += new System.EventHandler(this.SquidMonitor_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SquidMonitor_FormClosed);
            this.Load += new System.EventHandler(this.SquidMonitor_Load);
            this.ResizeEnd += new System.EventHandler(this.SquidMonitor_ResizeEnd);
            this.Resize += new System.EventHandler(this.SquidMonitor_ResizeEnd);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label connectingMessage;
    }
}