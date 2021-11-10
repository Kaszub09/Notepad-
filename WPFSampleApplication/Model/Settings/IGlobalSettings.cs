
using System;

namespace Notepad.Model {
    public interface IGlobalSettings {
        bool AutosaveAnyChanges { get; set; }
        ISettingsEditor Editor { get; }
        ISettingsApplicationUI WindowsUI { get; }

        event EventHandler AnySettingsChanged;

        void LoadFromFiles(object oldSettings = null);
        void ResetToDefault(object oldSettings = null);
        void SaveToFiles(object settingsToSave = null);
    }
}