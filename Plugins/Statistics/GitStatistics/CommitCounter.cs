﻿using System;
using System.Collections.Generic;
using System.Text;
using GitUIPluginInterfaces;

namespace GitStatistics
{
    public class CommitCounter
    {
        public CommitCounter(IGitUIEventArgs gitUIEventArgs)
        {
            GitUIEventArgs = gitUIEventArgs;
        }

        private IGitUIEventArgs GitUIEventArgs;

        public Dictionary<string, int> UserCommitCount = new Dictionary<string, int>();
        public int TotalCommits = 0;

        public void Count()
        {
            string[] userCommitCounts = GitUIEventArgs.GitUICommands.CommandLineCommand("cmd.exe", "/c \"\"" + GitUIEventArgs.GitCommand + "\" log --all --pretty=short | \"" + GitUIEventArgs.GitCommand + "\" shortlog --all -s -n\"").Split('\n');

            foreach (string userCommitCount in userCommitCounts)
            {
                string commitCount = userCommitCount.Trim(); //remove whitespaces at start and end
                int tab = commitCount.IndexOfAny(new char[] { ' ', '\t' });//find space or tab

                if (tab > 0)
                {
                    int count = 0;
                    string user;
                    if (int.TryParse(commitCount.Substring(0, tab), out count))
                    {
                        user = commitCount.Substring(tab+1);
                        TotalCommits += count;
                        UserCommitCount.Add(user, count);
                    }
                }
            }
        }
    }
}
