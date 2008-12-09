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
    public partial class FormCommit : Form
    {
        public FormCommit()
        {
            InitializeComponent();
        }

        private void FormCommit_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            GitCommands.GitCommands gitCommands = new GitCommands.GitCommands();

            List<GitItemStatus> itemStatusList = gitCommands.GitStatus();

            List<GitItemStatus> untrackedItemStatus = new List<GitItemStatus>();
            List<GitItemStatus> trackedItemStatus = new List<GitItemStatus>();
            foreach (GitItemStatus itemStatus in itemStatusList)
            {
                if (itemStatus.IsTracked == false)
                    untrackedItemStatus.Add(itemStatus);
                else
                    trackedItemStatus.Add(itemStatus);
            }

            Tracked.DataSource = trackedItemStatus;
            Untracked.DataSource = untrackedItemStatus;
        }

        protected void ShowChanges(GitItemStatus item)
        {
            EditorOptions.SetSyntax(SelectedDiff, item.Name);
            GitCommands.GitCommands gitCommands = new GitCommands.GitCommands();

            SelectedDiff.Text = gitCommands.GetCurrentChanges(item.Name);
            SelectedDiff.Refresh();
        }

        private void Tracked_SelectionChanged(object sender, EventArgs e)
        {
            if (Tracked.SelectedRows.Count == 0) return;

            if (Tracked.SelectedRows[0].DataBoundItem is GitItemStatus)
            {
                ShowChanges((GitItemStatus)Tracked.SelectedRows[0].DataBoundItem);
            }
        }

        private void Untracked_SelectionChanged(object sender, EventArgs e)
        {
            if (Untracked.SelectedRows.Count == 0) return;

            if (Untracked.SelectedRows[0].DataBoundItem is GitItemStatus)
            {
                ShowChanges((GitItemStatus)Untracked.SelectedRows[0].DataBoundItem);
            }
        }

        private void Commit_Click(object sender, EventArgs e)
        {
            if (Message.Text.Length == 0)
            {
                MessageBox.Show("Please enter commit message");
                return;
            }

            try
            {
                OutPut.Text = "";

                CommitDto dto = new CommitDto(Message.Text);
                GitCommands.Commit commit = new GitCommands.Commit(dto);
                commit.Execute();

                if (OutPut.Text.Length == 0)
                    OutPut.Text = "Command executed \n";

                OutPut.Text += dto.Result;

                Initialize();
            }
            catch
            {
            }
        }

        private void Scan_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Stage_Click(object sender, EventArgs e)
        {

            string result = "";
            foreach (DataGridViewRow row in Untracked.SelectedRows)
            {
                if (row.DataBoundItem is GitItemStatus)
                {
                    GitItemStatus item = (GitItemStatus)row.DataBoundItem;

                    AddFilesDto dto = new AddFilesDto(item.Name);
                    AddFiles addFiles = new AddFiles(dto);
                    addFiles.Execute();

                    result += dto.Result + "\n";

                }
            }

            if (result.Length > 0)
                OutPut.Text = result;

            Initialize();
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset all changes in the working dir?\nAll changes made to all files in the workin dir will be overwritten by the files from the current HEAD!", "WARNING!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (MessageBox.Show("Are you really sure you want to DELETE all changes?", "WARNING! WARNING!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    GitCommands.GitCommands gitCommands = new GitCommands.GitCommands();

                    OutPut.Text = gitCommands.Reset();
                }
            }
        }
    }
}
