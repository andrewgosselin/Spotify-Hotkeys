﻿namespace SpotifyHotkeys.Views
{
    partial class Initialization
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.webBrowser_authentication = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowser_authentication
            // 
            this.webBrowser_authentication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser_authentication.Location = new System.Drawing.Point(0, 0);
            this.webBrowser_authentication.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser_authentication.Name = "webBrowser_authentication";
            this.webBrowser_authentication.Size = new System.Drawing.Size(800, 450);
            this.webBrowser_authentication.TabIndex = 0;
            this.webBrowser_authentication.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser_authentication_Navigated);
            // 
            // Initialization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.webBrowser_authentication);
            this.Name = "Initialization";
            this.Text = "Initialization";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.WebBrowser webBrowser_authentication;
    }
}