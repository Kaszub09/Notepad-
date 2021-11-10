
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Notepad.Model;
using Notepad.View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace Notepad.ViewModel {
    public class MainWindowViewModel : BaseViewModel {
        public double ProgressBarValue { get; set; }
        public string LeftStatusBar { get; set; }
        public int  MiddleStatusBar { 
            get; 
            set; }

        public BindingList<DocumentViewModel> AllFilesItems { get; set; } = new BindingList<DocumentViewModel>();
        public DocumentViewModel SelectedFileItem { get; set; }

        #region Commands
        public ICommand CreateNewFile { get; }
        public ICommand OpenFile { get; }
        public ICommand SaveFile { get; }
        public ICommand CloseFile { get; }

        public ICommand ChangeOptions { get; }

        public ICommand NextFile { get => _nextFile; }
        public ICommand PreviousFile { get => _previousFile; }
        private Command _nextFile;
        private Command _previousFile;
        #endregion

        private bool _isSettingsWindowShown;

        public MainWindowViewModel() : base() {
            _isSettingsWindowShown = false;

            #region Commands
            CreateNewFile = new Command((obj) => {
                var DocumentM = App.Current.Services.GetRequiredService<IDocumentModel>();
                var newDocumentVM = new DocumentViewModel(DocumentM);
                newDocumentVM.FileName = GetNextFreeFilename();

                AllFilesItems.Insert(AllFilesItems.IndexOf(SelectedFileItem) + 1, newDocumentVM);
                SelectedFileItem = newDocumentVM;
            });

            OpenFile = new Command((obj) => {
                var openDialog = new OpenFileDialog();
                if(openDialog.ShowDialog() == true) {
                    var DocumentM = App.Current.Services.GetRequiredService<IDocumentModel>();
                    DocumentM.LoadFromFile(openDialog.FileName);
                    var newDocumentVM = new DocumentViewModel(DocumentM);

                    AllFilesItems.Insert(AllFilesItems.IndexOf(SelectedFileItem) + 1, newDocumentVM);
                    SelectedFileItem = newDocumentVM;
                }
            });

            SaveFile = new Command((obj) => {
                SelectedFileItem.Save();
            });

            CloseFile = new Command((obj) => {
                var parameter = obj as DocumentViewModel;
                if (parameter != null) {
                    SelectedFileItem = parameter;
                    if (SelectedFileItem.Close()) {
                        AllFilesItems.Remove(SelectedFileItem);
                    }
                }
            });

            ChangeOptions = new Command((obj) => {
                if (!_isSettingsWindowShown) {
                    var settingsWindow = App.Current.Services.GetRequiredService<SettingsWindow>();
                    settingsWindow.Owner = App.Current.MainWindow;
                    settingsWindow.Closed += (sender,e)=> _isSettingsWindowShown = false;
                    settingsWindow.Show();
                    _isSettingsWindowShown = true;
                }
            });
            
            _nextFile = new Command((obj) => SelectedFileItem = AllFilesItems[AllFilesItems.IndexOf(SelectedFileItem) + 1],
                (obj) => AllFilesItems.IndexOf(SelectedFileItem) < AllFilesItems.Count() - 1);

            _previousFile = new Command((obj) => SelectedFileItem = AllFilesItems[AllFilesItems.IndexOf(SelectedFileItem) - 1],
            (obj) => AllFilesItems.IndexOf(SelectedFileItem) >0);
            #endregion

            this.PropertyChanged += (sender, e) => {
                if (e.PropertyName == "SelectedFileItem") {
                    _previousFile.RaiseCanExecutedChanged();
                    _nextFile.RaiseCanExecutedChanged();
                }
            };

            CreateNewFile.Execute(null);
        }

        private string GetNextFreeFilename() {
            int freeNumber = 1;
            string filename = AppViewModel.AppSubtitles.GetText("new_file_default_filename", freeNumber);
            bool filenameDoesntExists = false;

            //Try to find number which yields filename not already in use. Arbitratry limit of 10 000 tries/numbers, in case GetText() doenst use number and returns same value
            for (int i = 0; i < 10000; i++) {
                if (AllFilesItems.Where(item => item.FileName == filename).Count() > 0) {
                    freeNumber++;
                    filename = AppViewModel.AppSubtitles.GetText("new_file_default_filename", freeNumber);
                } else {
                    filenameDoesntExists = true;
                    break;
                }
            }

            //Some arbitrary name which shouldn't already exists
            if (!filenameDoesntExists) {
                filename = DateTime.Now.Ticks.ToString();
            }

            return filename;
        }
    }
}
