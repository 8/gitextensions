﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GitCommands;

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
                RepositoryHistory.AddMostRecentRepository(From.Text);
                RepositoryHistory.AddMostRecentRepository(To.Text);

                //CloneDto dto = new CloneDto(From.Text, To.Text, CentralRepository.Checked);
                //GitCommands.Clone commit = new GitCommands.Clone(dto);
                //commit.Execute();

                FormProcess fromProcess;
                fromProcess = new FormProcess(Settings.GitDir + "git.cmd", GitCommands.GitCommands.CloneCmd(From.Text, To.Text, CentralRepository.Checked));

                if (!GitCommands.GitCommands.InTheMiddleOfConflictedMerge() && !GitCommands.GitCommands.InTheMiddleOfRebase() && !GitCommands.GitCommands.InTheMiddleOfPatch())
                    Close();
            }
            catch
            {
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

        private void From_DropDown(object sender, EventArgs e)
        {
            From.DataSource = RepositoryHistory.MostRecentRepositories.ToArray();
        }

        private void To_DropDown(object sender, EventArgs e)
        {
            To.DataSource = RepositoryHistory.MostRecentRepositories.ToArray();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LoadSSHKey_Click(object sender, EventArgs e)
        {
            new FormLoadPuttySSHKey().ShowDialog();
        }

        private void FormClone_Load(object sender, EventArgs e)
        {
            if (!GitCommands.GitCommands.Plink())
                LoadSSHKey.Visible = false;
        }
    }
}
