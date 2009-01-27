﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GitUI
{
    public partial class MergePatch : Form
    {
        public MergePatch()
        {
            InitializeComponent();
            EnableButtons();
        }

        public void SetPatchFile(string name)
        {
            PatchFile.Text = name;
        }

        private void EnableButtons()
        {
            if (GitCommands.GitCommands.InTheMiddleOfRebase())
            {
                BrowsePatch.Enabled = false;
                Apply.Enabled = false;
                PatchFile.ReadOnly = true;
                AddFiles.Enabled = true;
                Resolved.Enabled = true;
                Mergetool.Enabled = true;
                Skip.Enabled = true;
                Abort.Enabled = true;
            }
            else
            {
                BrowsePatch.Enabled = true;
                Apply.Enabled = true;
                PatchFile.ReadOnly = false;
                AddFiles.Enabled = false;
                Resolved.Enabled = false;
                Mergetool.Enabled = false;
                Skip.Enabled = false;
                Abort.Enabled = false;
            }
            patchGrid1.Initialize();
        }

        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private string SelectPatchFile(string initialDirectory)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "Patch file (*.Patch)|*.Patch";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Select patch file";
            return (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : PatchFile.Text;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            PatchFile.Text = SelectPatchFile(@".");
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PatchFile.Text))
            {
                MessageBox.Show("Please select a patch file");
                return;
            }

            new FormProcess(GitCommands.GitCommands.PatchCmd(PatchFile.Text));

            EnableButtons();
        }

        private void Mergetool_Click(object sender, EventArgs e)
        {
            GitCommands.GitCommands.RunRealCmd(GitCommands.Settings.GitDir + "git.cmd", "mergetool");

            if (MessageBox.Show("Resolved all conflicts? Run resolved?", "Conflicts solved", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                new FormProcess(GitCommands.GitCommands.ResolvedCmd());
                EnableButtons();
            }
        }

        private void Skip_Click(object sender, EventArgs e)
        {
            
            new FormProcess(GitCommands.GitCommands.SkipCmd());
            EnableButtons();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            new FormProcess(GitCommands.GitCommands.ResolvedCmd());
            EnableButtons();
        }

        private void Abort_Click(object sender, EventArgs e)
        {
            new FormProcess(GitCommands.GitCommands.AbortCmd());
            EnableButtons();
        }

        private void AddFiles_Click(object sender, EventArgs e)
        {
            FormAddFiles form = new FormAddFiles();
            form.ShowDialog();
        }

        private void MergePatch_Load(object sender, EventArgs e)
        {
            this.Text = "Apply patch (" + GitCommands.Settings.WorkingDir + ")";
        }

        private void MergePatch_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (GitCommands.GitCommands.InTheMiddleOfRebase())
            {
                if (MessageBox.Show("You are in the middle of a patch apply. You need to resolve, skip or abort this patch.\nAre you sure to exit now?", "Exit", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
