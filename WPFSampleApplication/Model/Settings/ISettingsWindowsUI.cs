using System.ComponentModel;

namespace Notepad.Model {
    public interface ISettingsWindowsUI : INotifyPropertyChanged {
        bool AutosaveChanges { get; set; }
        string LanguageID { get; set; }
        string ThemeID { get; set; }
        int MainFontSize { get; set; }
        int MenuAndStatusbarFontSize { get; set; }
        bool ShowNotifications { get; set; }
        bool UpdateTaskbarIcon { get; set; }
}
}