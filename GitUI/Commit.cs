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
            List<GitItemStatus> changedFiles = new GitCommands.GitCommands().GetAllChangedFiles();

            List<GitItemStatus> stagedFiles = new GitCommands.GitCommands().GetStagedFiles();

            Staged.DataSource = stagedFiles;

            Unstaged.DataSource = changedFiles;
        }

        protected void ShowChanges(GitItemStatus item, bool staged)
        {
            EditorOptions.SetSyntax(SelectedDiff, item.Name);
            SelectedDiff.Text = new GitCommands.GitCommands().GetCurrentChanges(item.Name, staged);
            SelectedDiff.Refresh();
        }

        private void Tracked_SelectionChanged(object sender, EventArgs e)
        {
            if (Staged.SelectedRows.Count == 0) return;

            if (Staged.SelectedRows[0].DataBoundItem is GitItemStatus)
            {
                ShowChanges((GitItemStatus)Staged.SelectedRows[0].DataBoundItem, true);
            }
        }

        private void Untracked_SelectionChanged(object sender, EventArgs e)
        {
            if (Unstaged.SelectedRows.Count == 0) return;

            if (Unstaged.SelectedRows[0].DataBoundItem is GitItemStatus)
            {
                ShowChanges((GitItemStatus)Unstaged.SelectedRows[0].DataBoundItem, false);
            }
        }

        private void Commit_Click(object sender, EventArgs e)
        {
            if (GitCommands.GitCommands.InTheMiddleOfConflictedMerge())
            {
                MessageBox.Show("There are unresolved mergeconflicts, solve mergeconflicts before committing", "Merge conflicts");
                return;
            }
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
            List<string> files = new List<string>();
            foreach (DataGridViewRow row in Unstaged.SelectedRows)
            {
                if (row.DataBoundItem is GitItemStatus)
                {
                    GitItemStatus item = (GitItemStatus)row.DataBoundItem;

                    files.Add(item.Name);
                }
            }

            result = GitCommands.GitCommands.StageFiles(files);

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
                    OutPut.Text = GitCommands.GitCommands.Reset();
                    Initialize();
                }
            }
        }

        private void UnstageFiles_Click(object sender, EventArgs e)
        {
            string result = "";
            List<string> files = new List<string>();
            foreach (DataGridViewRow row in Staged.SelectedRows)
            {
                if (row.DataBoundItem is GitItemStatus)
                {
                    GitItemStatus item = (GitItemStatus)row.DataBoundItem;

                    files.Add(item.Name);
                }
            }

            result = GitCommands.GitCommands.UnstageFiles(files);

            if (result.Length > 0)
                OutPut.Text = result;

            Initialize();
        }

        private void splitContainer8_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void Staged_Click(object sender, EventArgs e)
        {
            Tracked_SelectionChanged(sender, e);
        }

        private void Unstaged_Click(object sender, EventArgs e)
        {
            Untracked_SelectionChanged(sender, e);
        }

        private void AddManyFiles_Click(object sender, EventArgs e)
        {
            new FormAddFiles().ShowDialog();
            Initialize();
        }
    }
}
