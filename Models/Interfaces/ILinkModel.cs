using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.Models.Interfaces
{
    internal interface ILinkModel
    {
        public string FilePath { get; set; }
        public string Name { get; }
        public bool IsEnabled { get; set; }
    }
}
