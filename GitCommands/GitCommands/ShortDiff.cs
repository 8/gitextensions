﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitCommands
{
    public class ShortDiffDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Result { get; set; }

        public ShortDiffDto(string from, string to)
        {
            this.From = from;
            this.To = to;
        }
    }

    public class ShortDiff
    {
        public ShortDiffDto Dto { get; set; }
        public ShortDiff(ShortDiffDto dto)
        {
            this.Dto = dto;
        }

        public void Execute()
        {
            GitCommands gitCommands = new GitCommands();

            Dto.Result = gitCommands.RunCmd(Settings.GitDir + "git.exe", "diff " + Dto.From + " " + Dto.To + " --shortstat");
        }
    }
}
