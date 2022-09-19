using BackupProgram.Models;
using BackupProgram.Services.Interfaces;
using BackupProgram.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.Services
{
    public class DeleteService : IFileHandlingService
    {
        public void Delete(List<SourceLinkViewModel> sourceLinks)
        {
            foreach (var link in sourceLinks)
            {
                if (!link.AutoDeleteEnabled) { continue; }
                foreach (var destLink in link.DestLinks)
                {
                    if (!(destLink.AutoDeleteFrequency == 0))
                    {
                        DeleteOldFilesInDirectory(destLink.FilePath, destLink.AutoDeleteFrequency);
                    }
                }
            }
        }

        public void DeleteOldFilesInDirectory(string dir, int deleteFrequency)
        {
            foreach (var folder in Directory.GetDirectories(dir))
            {
                DateTime currentTime = DateTime.Now.Date;
                DateTime creationTime = Directory.GetCreationTime(folder);
                if (EligibleDeleteTime(currentTime, creationTime, deleteFrequency))
                {
                    if (Path.GetFileName(folder).Contains("꞉"))
                    {
                        Directory.Delete(folder, true);
                    }     
                }
            }
        }

        public bool EligibleDeleteTime(DateTime currentTime, DateTime creationTime, int deleteFrequency)
        {
            double fileAgeInSeconds = (currentTime - creationTime).TotalSeconds;
            double deleteFrequencyInSeconds = TimeSpan.FromDays(deleteFrequency).TotalSeconds;
            if (fileAgeInSeconds >= deleteFrequencyInSeconds)
            {
                return true;
            }
            else { return false; }
        }
    }
}
