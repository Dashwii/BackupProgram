using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BackupProgram.ViewModels
{
    internal interface ILinkViewModel
    {
        public string Name { get; }
        public string FilePath { get; set; }
        public bool IsEnabled { get; set; }
        public BitmapImage ImagePath { get; }

        public string ReturnLinkInfo();
    }
}
