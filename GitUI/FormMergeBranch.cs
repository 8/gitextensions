﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace GitUI
{
    public partial class FormMergeBranch : Form
    {
        public FormMergeBranch()
        {
            InitializeComponent();
        }

        private void FormMergeBranch_Load(object sender, EventArgs e)
        {
            string selectedHead = new GitCommands.GitCommands().GetSelectedBranch();
            Currentbranch.Text = "Current branch: " + selectedHead;

            Branches.DisplayMember = "Name";
            Branches.DataSource = new GitCommands.GitCommands().GetHeads(true);
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Command executed \n" + new GitCommands.GitCommands().MergeBranch(Branches.Text), "Merge");

            if (new GitCommands.GitCommands().InTheMiddleOfConflictedMerge())
            {
                if (MessageBox.Show("There are unresolved mergeconflicts, run mergetool now?", "Merge conflicts", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    new GitCommands.GitCommands().RunRealCmd(GitCommands.Settings.GitDir + "git.exe", "mergetool");
                    if (MessageBox.Show("When all mergeconflicts are resolved, you can commit.\nDo you want to commit now?", "Commit", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        new FormCommit().ShowDialog();
                    }
                }
            }
        }
    }
}
