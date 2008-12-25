﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace GitUI
{
    public partial class FormPush : Form
    {
        public FormPush()
        {
            InitializeComponent();
        }

        private void BrowseSource_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                PushDestination.Text = dialog.SelectedPath;
            
        }

        private void Push_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PushDestination.Text))
            {
                MessageBox.Show("Please select a destination directory");
                return;
            }

            RepositoryHistory.AddMostRecentRepository(PushDestination.Text);

            Process process = new GitCommands.GitCommands().PushAsync(PushDestination.Text);

            process.WaitForExit();
            
        }

        //void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    if (e.Data == null) return;
        //    string data = e.Data;
        //    if (data.StartsWith("Enter passphrase"))
        //        ((Process)sender).StandardInput.WriteLine("achttien");
        //}

        //void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    if (e.Data == null) return;
        //    string data = e.Data;
        //    if (data.StartsWith("Enter passphrase"))
        //        ((Process)sender).StandardInput.WriteLine("achttien");
        //}

        private void PushDestination_DropDown(object sender, EventArgs e)
        {
            PushDestination.DataSource = RepositoryHistory.MostRecentRepositories;
        }
    }
}
