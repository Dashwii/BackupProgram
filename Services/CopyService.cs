using BackupProgram.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.Services
{
    internal class CopyService : ICopyService
    {
        private LinkCollection _linkCollection;

        public CopyService(LinkCollection linkCollection)
        {
            _linkCollection = linkCollection;
        }

        public void Copy()
        {
            foreach (var link in _linkCollection.SourceLinks)
            {
                if (!link.IsEnabled) { continue; }
                foreach (var dest in link.DestLinks)
                {
                    if (!dest.IsEnabled) { continue; }
                    if (dest.CloudDest) { continue; }
                    else
                    {
                        try
                        {
                            CopyDirectory(link.FilePath, dest.FilePath, false);
                        }
                        catch (Exception ex)
                        {
                            if (ex is IOException)
                            {
                                continue;
                            }
                            else { throw; }
                        }
                    }
                }
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
    }
}
