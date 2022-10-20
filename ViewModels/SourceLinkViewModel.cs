using BackupProgram.Models;
using BackupProgram.ViewModels.Base;
using BackupProgram.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BackupProgram.ViewModels
{
    public class SourceLinkViewModel : BaseViewModel, ILinkViewModel
    {
        private SourceLinkModel _linkModel;
        public SourceLinkModel LinkModel => _linkModel;

        public string FilePath
        {
            get => LinkModel.FilePath;
            set 
            { 
                _linkModel.FilePath = value;
                Name = _linkModel.Name;
            }
        }

        public string Name { get; set; }

        public bool IsEnabled
        {
            get => LinkModel.IsEnabled;
            set => LinkModel.IsEnabled = value;
        }

        public bool IsAutoOnly
        {
            get => LinkModel.IsAutoOnly;
            set => LinkModel.IsAutoOnly = value;
        }

        public bool AutoCopyEnabled
        {
            get => LinkModel.AutoCopyEnabled; 
            set => LinkModel.AutoCopyEnabled = value;
        }

        public bool AutoDeleteEnabled
        {
            get => LinkModel.AutoDeleteEnabled; 
            set => LinkModel.AutoDeleteEnabled = value;
        }

        public BitmapImage ImagePath => 
            new BitmapImage(
                IsEnabled == true ? new Uri("images/checkmark.jpg", UriKind.Relative) : new Uri("images/x.png", UriKind.Relative)
                );
        public ObservableCollection<DestLinkViewModel> DestLinks { get; set; }

        public SourceLinkViewModel(SourceLinkModel linkModel)
        {
            _linkModel = new SourceLinkModel()
            {
                FilePath = linkModel.FilePath,
                IsEnabled = linkModel.IsEnabled,
                IsAutoOnly = linkModel.IsAutoOnly,
                AutoCopyEnabled = linkModel.AutoCopyEnabled,
                AutoDeleteEnabled = linkModel.AutoDeleteEnabled,
                DestLinks = linkModel.DestLinks
            };
            Name = _linkModel.Name;
            DestLinks = new();
            foreach (DestLinkModel link in _linkModel.DestLinks)
            {
                DestLinks.Add(new DestLinkViewModel(link));
            }
        }

        public void RemoveLinkIndex(int index)
        {
            DestLinks.RemoveAt(index);
            _linkModel.DestLinks.RemoveAt(index);
        }

        public void AddLink(DestLinkModel destLink)
        {
            DestLinks.Add(new DestLinkViewModel(destLink));
            _linkModel.DestLinks.Add(destLink);
        }

        /// <summary>
        /// Use when saving links to a file so viewmodel data and model data sync.
        /// </summary>
        public void UpdateModelDestLinks()
        {
            _linkModel.DestLinks = DestLinks.Select(x => x.LinkModel).ToList();
        }

        public string ReturnLinkInfo()
        {
            var d = FilePath.Replace("\\", "/");
            return @$"\b Path: \b0 {d}, \b Is Enabled: \b0 {IsEnabled}, \b Auto Only: \b0 {IsAutoOnly}, \b Auto Copy Enabled: \b0 {AutoCopyEnabled}, \b Auto Delete Enabled: \b0 {AutoDeleteEnabled}";
        }
    }
}
