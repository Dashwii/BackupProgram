using BackupProgram.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.Models
{
    public class SourceLinkModel : ILinkModel
    {
        public string FilePath { get; set; }
        public string Name => Path.GetFileName(FilePath);
        public bool IsEnabled { get; set; }
        public bool IsAutoOnly { get; set; }
        public bool AutoCopyEnabled { get; set; }
        public bool AutoDeleteEnabled { get; set; }
        public List<DestLinkModel> DestLinks { get; set; }
    }

}
