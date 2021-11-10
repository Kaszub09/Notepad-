using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.ViewModel {

    public class BaseViewModel :INotifyPropertyChanged{
        public event PropertyChangedEventHandler PropertyChanged;
        public ApplicationWideViewModel AppViewModel { get; private set; }

        public BaseViewModel() {
            AppViewModel = App.Current.AppViewModel;
           // AppViewModel.Settings.Editor.EditorFontFamily.
        }

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}