using BackupProgram.Services.Interfaces;
using BackupProgram.ViewModels;
using BackupProgram.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackupProgram.Services
{
    public class CopyProgressModel
    {
        public int CompletionPercentage { get; set; } = 0;
    }

    public class CopyService : IFileHandlingService
    {
        private int _totalFilesNeededCount = 0;
        private int _filesCopied = 0;
        private List<string> _inProgressDirectories = new();
        public IProgress<CopyProgressModel> CopyProgressTracker;
        public CancellationToken CancellationToken { get; set; }

        public CopyService(IProgress<CopyProgressModel> CopyProgressTracker, CancellationToken cancellationToken)
        {
            this.CopyProgressTracker = CopyProgressTracker;
            CancellationToken = cancellationToken;
        }

        public async Task CopyAsync(List<SourceLinkViewModel> sourceLinks)
        {
            var copyTasks = new List<Task>();

            GatherFilesForProgress(sourceLinks);

            foreach (var link in sourceLinks)
            {
                if (!EligibleCopy(link)) { continue; }
                foreach (var dest in link.DestLinks)
                {
                    if (!EligibleCopy(link)) { continue; }
                    copyTasks.Add(HandleCopyingAsync(link.FilePath, dest.FilePath));
                }
            }
            try
            {
                await Task.WhenAll(copyTasks);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    await Task.Run(() => CleanupIncompleteDirectories());
                    throw;
                }
                else { return; }
            }
        }

        public async Task AutoCopyAsync(List<SourceLinkViewModel> sourceLinks)
        {
            var copyTasks = new List<Task>();

            GatherFilesForProgress(sourceLinks);

            foreach (var link in sourceLinks)
            {
                if (!EligibleAutoCopy(link)) { continue; }
                foreach (var dest in link.DestLinks)
                {
                    if (!EligibleCopy(dest)) { continue; }
                    if (!EligibleAutoCopyTime(DateTime.Now.Date, dest.LastAutoCopyDate, dest.AutoCopyFrequency)) { continue; }
                    copyTasks.Add(HandleCopyingAsync(link.FilePath, dest.FilePath));
                    dest.LastAutoCopyDate = DateTime.Now.Date;
                }
            }
            try
            {
                await Task.WhenAll(copyTasks);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) 
                {
                    await Task.Run(() => CleanupIncompleteDirectories());
                    throw;
                }
                else { return; }
            }
        }

        private void CleanupIncompleteDirectories()
        {
            Action<string> hardDelete = null!;
            hardDelete = new Action<string>(directory =>
            {
                string[] files = Directory.GetFiles(directory);
                string[] dirs = Directory.GetDirectories(directory);

                foreach (string file in files)
                {
                    // Set file attributes to normal due to some files denying permission when deleting. 
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
                foreach (string dir in dirs)
                {
                    hardDelete(dir);
                }
                Directory.Delete(directory);
            });


            for (int i = _inProgressDirectories.Count - 1; i >= 0; i--)
            {
                hardDelete(_inProgressDirectories[i]);
                _inProgressDirectories.RemoveAt(i);
            }

        }

        private async Task HandleCopyingAsync(string source, string destination)
        {
            try
            {
                // Create new folder for files to reside in. 
                string copyFolder = await Task.Run(() => CreateCopyFolder(source, destination));
                _inProgressDirectories.Add(copyFolder);
                await Task.Run(() => CopyDirectory(source, copyFolder));
                _inProgressDirectories.Remove(copyFolder);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) { throw; }
                else
                {
                    // TODO: Log error.
                    return;
                }
            }
        }

        private void CopyDirectory(string source, string copyFolder)
        {
            var info = new DirectoryInfo(source);
            foreach (FileInfo file in info.GetFiles())
            {
                string targetFilename = Path.Combine(copyFolder, file.Name);
                file.CopyTo(targetFilename);
                _filesCopied += 1;
                UpdateProgress();
                CancellationToken.ThrowIfCancellationRequested();
            }
            foreach (DirectoryInfo subDir in info.GetDirectories())
            {
                // Create new sub directory in destination.
                string newSubDirDest = Path.Combine(copyFolder, subDir.Name);
                Directory.CreateDirectory(newSubDirDest);

                CopyDirectory(subDir.FullName, newSubDirDest);
            }

        }
        
        private void UpdateProgress()
        {
            CopyProgressModel report = new CopyProgressModel();
            if (_totalFilesNeededCount == 0)
            {
                report.CompletionPercentage = 0;
            }
            else
            {
                report.CompletionPercentage = (_filesCopied * 100) / _totalFilesNeededCount;
            }
            CopyProgressTracker.Report(report);
        }

        public void ResetProgress()
        {
            _filesCopied = 0;
            _totalFilesNeededCount = 0;
            UpdateProgress();
        }

        private void GatherFilesForProgress(List<SourceLinkViewModel> sourceLinks)
        {
            foreach (var link in sourceLinks)
            {
                if (!link.IsEnabled) { continue; }
                int multiplierCount = 0;

                foreach (var destLink in link.DestLinks)
                {
                    if (!destLink.IsEnabled) { continue; }
                    multiplierCount += 1;
                }

                var linkTotalFileCount = Directory.EnumerateFiles(link.FilePath, "*.*", SearchOption.AllDirectories).Count();
                linkTotalFileCount *= multiplierCount;
                _totalFilesNeededCount += linkTotalFileCount;
            }
        }

        private bool EligibleCopy(ILinkViewModel link)
        {  
            if (!Directory.Exists(link.FilePath))
            {
                // TODO: Alert user.
                return false;
            }
            if (!link.IsEnabled) { return false; }
            if (link is DestLinkViewModel)
            {
                var d = (DestLinkViewModel)link;
                if (d.DestType != Models.TargetType.LOCAL) { return false; }
            }
            else
            {
                var d = (SourceLinkViewModel)link;
                if (d.IsAutoOnly) { return false; }
            }
            return true;
        }

        private bool EligibleAutoCopy(ILinkViewModel link)
        {

            if (!EligibleCopy(link)) { return false; }
            if (link is SourceLinkViewModel)
            {
                var s = (SourceLinkViewModel)link;
                if (!s.AutoCopyEnabled) { return false; }
            }
            return true;
        }

        private string CreateCopyFolder(string source, string dest)
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

        public bool EligibleAutoCopyTime(DateTime currentTime, DateTime lastAutoCopyDate, int copyFrequency)
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
