﻿using System;
using System.Collections.Generic;

using System.Text;

namespace GitCommands
{
    public class PushDto
    {
        public string Result { get; set; }

        public PushDto()
        {
        }
    }

    public class Push
    {
        public PushDto Dto { get; set; }
        public Push(PushDto dto)
        {
            this.Dto = dto;
        }

        public void Execute()
        {
            Dto.Result = new GitCommands().RunCmd(Settings.GitDir + "git.exe", "push");
        }
    }
}
