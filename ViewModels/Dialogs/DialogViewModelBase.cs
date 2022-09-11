using BackupProgram.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.ViewModels.Dialogs
{
    internal class DialogViewModelBase : ViewModelBase
    {
        public int Width { get; set; } = 450;
        public int Height { get; set; } = 800;
    }
}
