using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Notepad.View;
using Microsoft.Extensions.DependencyInjection;

namespace Notepad.ViewModel {
    public class SettingsWindowViewModel:BaseViewModel {
        public BindingList<Page> AllAvailablePages { get; set; } = new BindingList<Page>();
        public Page SelectedPage { get; set; }

        public SettingsWindowViewModel() : base() {

            AllAvailablePages.Add(App.Current.Services.GetRequiredService<ApplicationSettingsPage>());
            AllAvailablePages.Add(App.Current.Services.GetRequiredService<TextSettingsPage>());
           
            SelectedPage = AllAvailablePages[0];
        }


    }
}
