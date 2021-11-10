using System.ComponentModel;

namespace Notepad.Model {
    public interface ISettingsApplicationUI : INotifyPropertyChanged {
        bool AutosaveChanges { get; set; }
        string LanguageID { get; set; }
        string ThemeID { get; set; }
        int MainFontSize { get; set; }
}
}