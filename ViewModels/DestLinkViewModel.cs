﻿using BackupProgram.Models;
using BackupProgram.ViewModels.Base;
using BackupProgram.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BackupProgram.ViewModels
{
    public class DestLinkViewModel : BaseViewModel, ILinkViewModel
    {
        #region Fields
        private DestLinkModel _linkModel;
        public DestLinkModel LinkModel => _linkModel;
        
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

        public bool CloudDest
        {
            get => LinkModel.CloudDest;
            set => LinkModel.CloudDest = value;
        }

        public int AutoCopyFrequency
        {
            get => LinkModel.AutoCopyFrequency;
            set => LinkModel.AutoCopyFrequency = value;
        }

        public int AutoDeleteFrequency
        {
            get => LinkModel.AutoDeleteFrequency;
            set => LinkModel.AutoDeleteFrequency = value;
        }

        public DateTime LastAutoCopyDate
        {
            get => LinkModel.LastAutoCopyDate;
            set => LinkModel.LastAutoCopyDate = value;
        }

        public BitmapImage ImagePath => 
            new BitmapImage(
                IsEnabled == true ? new Uri("images/checkmark.jpg", UriKind.Relative) : new Uri("images/x.png", UriKind.Relative)
                );

        #endregion

        public DestLinkViewModel(DestLinkModel linkModel)
        {
            _linkModel = new DestLinkModel()
            {
                FilePath = linkModel.FilePath,
                IsEnabled = linkModel.IsEnabled,
                CloudDest = linkModel.CloudDest,
                AutoCopyFrequency = linkModel.AutoCopyFrequency,
                AutoDeleteFrequency = linkModel.AutoDeleteFrequency,
                LastAutoCopyDate = linkModel.LastAutoCopyDate
            };
            Name = _linkModel.Name;
        }

        public string ReturnLinkInfo()
        {
            var d = FilePath.Replace("\\", "/");
            return @$"\b Path: \b0 {d}, \b Is Enabled: \b0 {IsEnabled}, \b Cloud: \b0 {CloudDest}, \b Auto Copy Freq: \b0 {AutoCopyFrequency}, \b Auto Delete Freq: \b0 {AutoDeleteFrequency}";
        }
    }
}
