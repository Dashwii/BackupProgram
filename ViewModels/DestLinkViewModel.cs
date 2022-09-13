﻿using BackupProgram.Models;
using BackupProgram.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BackupProgram.ViewModels
{
    internal class DestLinkViewModel : ViewModelBase, ILinkViewModel
    {
        private DestLinkModel _linkModel;
        public DestLinkModel LinkModel => _linkModel;
        
        public string FilePath
        {
            get { return _linkModel.FilePath; }
            set 
            { 
                _linkModel.FilePath = value;
                Name = _linkModel.Name;
            }
        }

        public string Name { get; set; }

        public bool IsEnabled
        {
            get { return _linkModel.IsEnabled; }
            set { _linkModel.IsEnabled = value; }
        }

        public bool CloudDest
        {
            get { return _linkModel.CloudDest; }
            set { _linkModel.CloudDest = value; }
        }

        public int AutoCopyFrequency
        {
            get { return _linkModel.AutoCopyFrequency; }
            set { _linkModel.AutoCopyFrequency = value; }
        }

        public int AutoDeleteFrequency
        {
            get { return _linkModel.AutoDeleteFrequency; }
            set { _linkModel.AutoDeleteFrequency = value; }
        }

        public BitmapImage ImagePath => new BitmapImage(IsEnabled == true ? new Uri("images/checkmark.jpg", UriKind.Relative) : new Uri("images/x.png", UriKind.Relative));

        public DestLinkViewModel(DestLinkModel linkModel)
        {
            _linkModel = linkModel;
            Name = linkModel.Name;
        }

        public string ReturnLinkInfo()
        {
            var d = FilePath.Replace("\\", "/");
            return @$"\b Path: \b0 {d}, \b Is Enabled: \b0 {IsEnabled}, \b Cloud: \b0 {CloudDest}, \b Auto Copy Freq: \b0 {AutoCopyFrequency}, \b Auto Delete Freq: \b0 {AutoDeleteFrequency}";
        }
    }
}
