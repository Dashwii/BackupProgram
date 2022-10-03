using BackupProgram.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BackupProgram.Models;
using System.Windows;

namespace BackupProgram.ViewModels.Dialogs
{
    public class MainCreateDestLinkViewModel : BaseViewModel, IDialogWindowViewModel
    {
        #region Fields

        private Action<DestLinkViewModel>? _myAction;
        private Window _window;
        private bool _editMode;
        DestLinkViewModel _destLinkViewModel;

        public DestLinkViewModel DestLinkViewModel
        {
            get => _destLinkViewModel;
            set => _destLinkViewModel = value;
        }

        SelectDestLinkTypeViewModel _linkTypeViewModel;
        CreateDestLinkViewModel _addDestLinkDialogViewModel;
        public BaseViewModel CurrentViewModel { get; private set; }

        #endregion 

        public MainCreateDestLinkViewModel(Window Window, params object?[] paramList)
        {
            _window = Window;
            _linkTypeViewModel = new(this);
            _addDestLinkDialogViewModel = new(this);
            _myAction = (Action<DestLinkViewModel>?)paramList[0]!;
            _editMode = (bool)paramList[1]!;
            if (!_editMode)
            {
                _destLinkViewModel = new(new DestLinkModel()
                {
                    FilePath = string.Empty,
                    IsEnabled = true,
                    LastAutoCopyDate = DateTime.Now.Date
                });
                CurrentViewModel = _linkTypeViewModel;
            }
            else
            {
                _destLinkViewModel = (DestLinkViewModel)paramList[2]!;
                CurrentViewModel = _addDestLinkDialogViewModel;
            }    
        }

        public void SwitchViewModel<TViewModel>()
        {
            if (typeof(TViewModel) == typeof(SelectDestLinkTypeViewModel))
            {
                CurrentViewModel = _linkTypeViewModel;
            }
            else if (typeof(TViewModel) == typeof(CreateDestLinkViewModel))
            {
                CurrentViewModel = _addDestLinkDialogViewModel;
            }
        }

        public void CloseWindow()
        {
            _window.Close();
        }

        public void Finished()
        {
            if (_myAction is not null)
            {
                _myAction(DestLinkViewModel);
            }
            CloseWindow();
        }
    }
}
