﻿using System;
using System.ComponentModel;

namespace Notepad.Model {
    public interface ISettingsProcess : INotifyPropertyChanged {
        bool AutosaveChanges { get; set; }
        DateTime ChosedDate { get; set; }
        string EmailCredName { get; set; }
        double IntervalSec { get; set; }
        string XServiceCredName { get; set; }
        bool RunProcessImmediately { get; set; }
    }
}