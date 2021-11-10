using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Model {
    public class SettingsApplicationUI : ISettingsApplicationUI {
        public event PropertyChangedEventHandler PropertyChanged;
        [DoNotNotify]
        public bool AutosaveChanges { get; set; } = true;

        //Actual settings
        public string ThemeID { get; set; } = "Default";
        public string LanguageID { get; set; } = "pol";
        public int MainFontSize { get; set; } = 12;
    }
}
