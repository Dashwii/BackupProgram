using BackupProgram.Services.Interfaces;
using BackupProgram.ViewModels;
using BackupProgram.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.Services
{
    public class CopyService : IFileHandlingService
    {
        public async Task CopyAsync(List<SourceLinkViewModel> sourceLinks)
        {
            var copyTasks = new List<Task>();
            foreach (var link in sourceLinks)
            {
                if (!link.IsEnabled) { continue; }
                foreach (var dest in link.DestLinks)
                {
                    if (!dest.IsEnabled) { continue; }
                    if (dest.DestType != Models.TargetType.LOCAL) { continue; }
                    copyTasks.Add(HandleCopyingAsync(link.FilePath, dest.FilePath));
                }
            }
            await Task.WhenAll(copyTasks);
        }

        public async Task AutoCopyAsync(List<SourceLinkViewModel> sourceLinks)
        {
            var copyTasks = new List<Task>();
            foreach (var link in sourceLinks)
            {
                if (!link.IsEnabled) { continue; }
                foreach (var dest in link.DestLinks)
                {
                    if (!dest.IsEnabled) { continue; }
                    if (dest.DestType != Models.TargetType.LOCAL) { continue; }
                    if (!EligibleCopyTime(DateTime.Now.Date, dest.LastAutoCopyDate, dest.AutoCopyFrequency)) { continue; }
                    copyTasks.Add(HandleCopyingAsync(link.FilePath, dest.FilePath));
                    dest.LastAutoCopyDate = DateTime.Now.Date;
                }
            }
            await Task.WhenAll(copyTasks);
        }

        private async Task HandleCopyingAsync(string source, string destination)
        {
            try
            {
                await Task.Run(() => CopyDirectory(source, destination, false));
            }
            catch (Exception e)
            {
                // TODO: Log exceptions
                return;
            }
        }

        private void CopyDirectory(string source, string destination, bool recursive)
        {
            var info = new DirectoryInfo(source);
            if (!info.Exists) { throw new DirectoryNotFoundException($"Source directory {info.FullName} not found."); }

            if (recursive)
            {
                Directory.CreateDirectory(destination);
                foreach (FileInfo file in info.GetFiles())
                {
                    string targetFileName = Path.Combine(destination, file.Name);
                    file.CopyTo(targetFileName);
                }
                foreach (DirectoryInfo subDir in info.GetDirectories())
                {
                    string newDestinationDir = Path.Combine(destination, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
            else
            {
                string newDirectory = CreateNewDirectory(source, destination);
                foreach (FileInfo file in info.GetFiles())
                {
                    string targetFileName = Path.Combine(newDirectory, file.Name);
                    file.CopyTo(targetFileName);
                }
                foreach (DirectoryInfo subDir in info.GetDirectories())
                {
                    string newDestinationDir = Path.Combine(newDirectory, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        private string CreateNewDirectory(string source, string dest)
        {
            string newFolderName;
            try
            {
                newFolderName = $"{Path.GetFileName(source)} {DateTime.Now.ToString("yyyy-MM-dd hh꞉mm꞉ss")}";
                if (newFolderName is null) { throw new Exception("Cannot copy from root path."); }
            }
            catch (Exception)
            {
                throw;
            }
            string newDirectory = Path.Combine(dest, newFolderName);
            Directory.CreateDirectory(newDirectory);
            return newDirectory;
        }

        public bool EligibleCopyTime(DateTime currentTime, DateTime lastAutoCopyDate, int copyFrequency)
        {
            if (copyFrequency == 0) { return false; }
            double timePassedSinceLastCopyInSeconds = (currentTime - lastAutoCopyDate).TotalSeconds;
            double copyFrequencyInSeconds = TimeSpan.FromDays(copyFrequency).TotalSeconds;
            if (timePassedSinceLastCopyInSeconds >= copyFrequencyInSeconds)
            {
                return true;
            }
            else { return false; }
        }
    }
}
