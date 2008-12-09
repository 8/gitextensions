﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GitCommands;
using System.Diagnostics;

// This delegate enables asynchronous calls for setting
// the text property on a TextBox control.
delegate void SetTextCallback(string text);
delegate void DoneCallback();

namespace GitUI
{
    public partial class FormClone : Form
    {
        public FormClone()
        {
            InitializeComponent();
            From.Text = Settings.WorkingDir;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            try
            {
                OutPut.Text = "";
                
                Ok.Enabled = false;
                FromBrowse.Enabled = false;
                ToBrowse.Enabled = false;

                GitCommands.GitCommands gitCommands = new GitCommands.GitCommands();
                gitCommands.DataReceived += new DataReceivedEventHandler(gitCommands_DataReceived);
                gitCommands.Exited += new EventHandler(gitCommands_Exited);
                gitCommands.CloneAsync(From.Text, To.Text);
                
                OutPut.Text = "Start clone \n";
            }
            catch
            {
            }
        }

        void gitCommands_Exited(object sender, EventArgs e)
        {
            DoneCallback d = new DoneCallback(Done);
            this.Invoke(d, new object[] {});
        }

        private void Done()
        {
            this.Ok.Enabled = true;
            this.FromBrowse.Enabled = true;
            this.ToBrowse.Enabled = true;
        }

        // This method is passed in to the SetTextCallBack delegate
        // to set the Text property of textBox1.
        private void SetText(string text)
        {
            this.OutPut.Text += "\n" + text;
        }

        void gitCommands_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OutPut.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { e.Data });
            }
            else
            {
                SetText(e.Data);
            }
        }

        private void FromBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                From.Text = dialog.SelectedPath;

        }

        private void ToBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                To.Text = dialog.SelectedPath;

        }
    }
}
