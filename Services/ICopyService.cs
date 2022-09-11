using BackupProgram.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.Services
{
    internal interface ICopyService
    {
        public void Copy()
        {
            throw new NotImplementedException();
        }

        private void CopyDirectory(string source, string destination, bool recursive)
        {
            throw new NotImplementedException();
        }

        private string CreateNewDirectory(string sourceDir, string destinationDir)
        {
            throw new NotImplementedException();
        }
    }
}
