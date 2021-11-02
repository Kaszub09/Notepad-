using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Notepad.Model;

namespace Notepad.ViewModel {
    public class DocumentViewModel : BaseViewModel {

        #region Content
        public string Text { get; set; } = "";
        public int LinesNumber { get; set; } = 1;
        public int CharactersNumber { get; set; } = 0;
        public string InfoText { get; set; }
        public bool AreChangesSaved { get; set; }
        #endregion

        #region Caret
        public int CaretPosition { get; set; } = 1;
        public int CaretLine { get; set; } = 1;
        public int CaretColumn { get; set; } = 1;
        public string InfoCaret { get; set; }
        #endregion

        #region File
        public string FileName { get; set; }
        public string InfoEncoding { get; set; }
        public string InfoFileType { get; set; }
        #endregion


        #region Lines
        public string AllLinesNumbers { get; set; } = "1";
        private static string _linesNumberBase;
        #endregion

        public ImageSource SaveIcon { get; set; }

        private IDocumentModel _document;

        public DocumentViewModel(IDocumentModel document) {
            _document = document;
            FileName = _document.GetFilename();
            Text = _document.GetText();
            UpdateSaveIcon();

            if (_linesNumberBase == null) {
                _linesNumberBase = string.Join<int>(Environment.NewLine, Enumerable.Range(1, 1000000));
            }

            this.PropertyChanged += SingleFileItem_PropertyChanged;       
        }

        private void SingleFileItem_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "LinesNumber" || e.PropertyName == "CharactersNumber") {
                InfoText = AppViewModel.AppSubtitles.GetText("text_info", CharactersNumber, LinesNumber);
            } else if (e.PropertyName == "CaretPosition" || e.PropertyName == "CaretLine" || e.PropertyName == "CaretColumn") {
                InfoCaret = AppViewModel.AppSubtitles.GetText("text_caret_info", CaretPosition, CaretLine, CaretColumn);
            }
            if (e.PropertyName == "LinesNumber") {
                if (LinesNumber > 1) {
                    AllLinesNumbers = _linesNumberBase.Substring(0, _linesNumberBase.IndexOf(Environment.NewLine+ (LinesNumber+1).ToString()+ Environment.NewLine));
                } else {
                    AllLinesNumbers = "1";
                }     
            }
            if (e.PropertyName == "Text") {
                _document.SetText(Text);
                UpdateSaveIcon();
            }
        }

        private void UpdateSaveIcon() {
            if (_document.IsSaved) {
                SaveIcon = new BitmapImage(new Uri("pack://application:,,,/Resources/saved_file.png", UriKind.Absolute));
            } else {
                SaveIcon = new BitmapImage(new Uri("pack://application:,,,/Resources/unsaved_file.png", UriKind.Absolute));
            }
        }

        public void Save() {
            if(_document.HasFilepath()) {
                _document.Save();
            } else{
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.AddExtension = true;
                saveFileDialog.ValidateNames = true;
                saveFileDialog.Filter = "Plik tekstowy (*.txt, *.csv) | *.txt; *.csv | Wszystkie pliki (*.*) | *.* ";
                if (_document.GetFilepath() != null)
                    saveFileDialog.FileName = _document.GetFilepath();

                if (saveFileDialog.ShowDialog() == true) {
                    _document.ChangeFilepath(saveFileDialog.FileName);
                    _document.Save();
                    FileName = _document.GetFilename();
                }
            }
            UpdateSaveIcon();
        }

        public bool Close() {
            if (!_document.IsSaved) {
                var ans = MessageBox.Show(AppViewModel.AppSubtitles.GetText("close_unsaved_file_text",_document.GetFilepath()==null? FileName: _document.GetFilepath()),
                    AppViewModel.AppSubtitles["close_unsaved_file_caption"],MessageBoxButton.YesNoCancel);
                if(ans == MessageBoxResult.Yes) {
                    Save();
                    return true;
                } else if(ans == MessageBoxResult.No) {
                    return true;
                } else {
                    return false;
                }
            }
            return true;
        }


    }
}
