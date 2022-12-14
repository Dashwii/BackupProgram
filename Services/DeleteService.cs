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
        public async Task DeleteAsync(List<SourceLinkViewModel> sourceLinks)
        {
            var deleteTasks = new List<Task>();
            foreach (var link in sourceLinks)
            {
                if (!link.AutoDeleteEnabled) { continue; }
                foreach (var destLink in link.DestLinks)
                {
                    if (destLink.IsEnabled)
                    {
                        var task = new Task(() => DeleteOldFilesInDirectory(destLink.FilePath, destLink.AutoDeleteFrequency));
                        deleteTasks.Add(task);
                        task.Start();
                    }
                }
            }
            await Task.WhenAll(deleteTasks);
        }

        public void DeleteOldFilesInDirectory(string dir, int deleteFrequency)
        {
            DateTime currentTime = DateTime.Now.Date;
            foreach (var folder in Directory.GetDirectories(dir))
            {
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
            if (deleteFrequency == 0) { return false; }
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
