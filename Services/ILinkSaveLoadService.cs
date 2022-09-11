using BackupProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.Services
{
    internal interface ILinkSaveLoadService
    {
        public void SaveLinksJson(List<SourceLinkModel> links)
        {
            throw new NotImplementedException();
        }

        public List<SourceLinkModel> LoadLinksJson()
        {
            throw new NotImplementedException();
        }
    }
}
