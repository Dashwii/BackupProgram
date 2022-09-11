using BackupProgram.Models;
using BackupProgram.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BackupProgram.ViewModels
{
    internal class SourceLinkViewModel : ViewModelBase
    {
        private SourceLinkModel _linkModel;
        public SourceLinkModel LinkModel => _linkModel;

        public string Name => _linkModel.Name;

        public string FilePath
        {
            get { return _linkModel.FilePath; }
            set { _linkModel.FilePath = value; }
        }

        public bool IsEnabled
        {
            get { return _linkModel.IsEnabled; }
            set { _linkModel.IsEnabled = value; }
        }

        public bool AutoCopyEnabled
        {
            get { return _linkModel.AutoCopyEnabled; }
            set { _linkModel.AutoCopyEnabled = value; }
        }

        public bool AutoDeleteEnabled
        {
            get { return _linkModel.AutoDeleteEnabled; }
            set { _linkModel.AutoDeleteEnabled = value; }
        }

        public BitmapImage ImagePath => new BitmapImage(IsEnabled == true ? new Uri("images/checkmark.jpg", UriKind.Relative) : new Uri("images/x.png", UriKind.Relative));
        public ObservableCollection<DestLinkViewModel> DestLinks { get; set; }

        public SourceLinkViewModel(SourceLinkModel linkModel)
        {
            _linkModel = linkModel;
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
    }
}
