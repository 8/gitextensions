﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitCommands
{
    public class GitItem : IGitItem
    {
        public GitItem()
        {
        }

        public string Guid { get; set; }
        public string ItemType{ get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Mode { get; set; }

        protected List<IGitItem> subItems;
        public List<IGitItem> SubItems
        {
            get
            {
                if (subItems == null)
                {
                    GitCommands gitCommands = new GitCommands();

                    subItems = gitCommands.GetTree(Guid);

                    foreach (GitItem item in subItems)
                    {
                        item.FileName = FileName + "\\" + item.FileName;
                    }
                }

                return subItems;
            }
        }
    }
}
