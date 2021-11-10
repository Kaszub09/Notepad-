using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Notepad.Model {

    public interface ISettingsEditor : INotifyPropertyChanged {
        bool AutosaveChanges { get; set; }
        Color EditorBackgroundColor { get; set; }
        Color EditorFontColor { get; set; }
        FontFamily EditorFontFamily { get; set; }
        int EditorFontSize { get; set; }
        public FamilyTypeface EditorFontTypeface { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}