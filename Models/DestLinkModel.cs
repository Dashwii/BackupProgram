using BackupProgram.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.Models
{
    public class DestLinkModel : ILinkModel
    {
        public string FilePath { get; set; }
        public string Name => Path.GetFileName(FilePath);
        public bool IsEnabled { get; set; }

        // CloudDest will soon be another type. For now it's a bool.
        public bool CloudDest { get; set; }
        public int AutoCopyFrequency { get; set; }
        public int AutoDeleteFrequency { get; set; }
        public DateTime LastAutoCopyDate { get; set; }
    }
}
