using BackupProgram.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.Models
{
    public enum TargetType
    {
        LOCAL,
        GOOGLE,
        DROPBOX,
        MEGA
    }

    public class DestLinkModel : ILinkModel
    {
        public string FilePath { get; set; }
        public string Name => Path.GetFileName(FilePath);
        public bool IsEnabled { get; set; }

        public TargetType DestType { get; set; }
        public int AutoCopyFrequency { get; set; }
        public int AutoDeleteFrequency { get; set; }
        public DateTime LastAutoCopyDate { get; set; }
    }
}
