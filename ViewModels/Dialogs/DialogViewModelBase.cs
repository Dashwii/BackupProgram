using BackupProgram.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.ViewModels.Dialogs
{
    public class DialogViewModelBase : BaseViewModel
    {
        public int Width { get; set; } = 450;
        public int Height { get; set; } = 800;
    }
}
