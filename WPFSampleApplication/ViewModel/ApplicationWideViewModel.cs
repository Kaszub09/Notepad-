using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Notepad.Model;
using System.Windows.Shell;

namespace Notepad.ViewModel {

    public class ApplicationWideViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public IApplicationSubtitles AppSubtitles { get; private set; }
        public IApplicationThemes AppThemes { get; private set; }
        public IGlobalSettings Settings { get; private set; }

        public BindingList<LanguageItem> AllLanguagesItems { get; private set; } = new BindingList<LanguageItem>();
        public BindingList<ThemeItem> AllThemesItems { get; private set; } = new BindingList<ThemeItem>();

        #region Commands
        public ICommand ChangeLanguage { get; private set; }
        public ICommand ChangeTheme { get; private set; }
        public ICommand RefreshLanguages { get; private set; }
        public ICommand RefreshThemes { get; private set; }
        public ICommand ExitApplication { get; private set; }
        #endregion

        public ApplicationWideViewModel(IApplicationSubtitles applicationSubtitles, IApplicationThemes applicationThemes, IGlobalSettings globalSettings) {
            AppSubtitles = applicationSubtitles;
            AppThemes = applicationThemes;
            Settings = globalSettings;

            #region Commands definition
            ChangeLanguage = new Command((obj) => {
                if (obj is string)
                    AppSubtitles.ChangeLanguage((string)obj);
            });

            ChangeTheme = new Command((obj) => {
                if (obj is string)
                    AppThemes.ChangeTheme((string)obj);
            });

            RefreshLanguages = new Command((obj) => {
                AppSubtitles.ReloadAllLanguages();
            });

            RefreshThemes = new Command((obj) => {
                AppThemes.ReloadAllThemes();
            });

            ExitApplication = new Command((obj) => {
                App.Current.Shutdown();
            });
            #endregion

            AppSubtitles.LanguageChanged += ApplicationSubtitles_LanguageChanged;
            AppSubtitles.LanguagesReloaded += ApplicationSubtitles_LanguagesReloaded;
            AppThemes.ThemesReloaded += ApplicationThemes_ThemesReloaded;
            AppThemes.ThemeChanged += ApplicationThemes_ThemeChanged;

            ReloadLanguages();
            ReloadThemes();
        }


        private void ApplicationThemes_ThemeChanged(object sender, string e) {
            Settings.WindowsUI.ThemeID = e;
        }

        private void ApplicationSubtitles_LanguagesReloaded(object sender, EventArgs e) {
            ReloadLanguages();
        }

        private void ApplicationThemes_ThemesReloaded(object sender, EventArgs e) {
            ReloadThemes();
        }

        private void ApplicationSubtitles_LanguageChanged(object sender, string e) {
            ReloadThemes();
            Settings.WindowsUI.LanguageID = e;
        }

        private void ReloadLanguages() {
            var currentLanguage = Settings.WindowsUI.LanguageID;
            var languages = AppSubtitles.GetAvailableLanguages().Select(
                l => {
                    return new LanguageItem {
                        ID = l.langID, DisplayName = l.langName,
                        ChangeLanguage = new Command(obj => { AppSubtitles.ChangeLanguage(l.langID); })
                    };
                })
                .ToList();
            AllLanguagesItems.Clear();
            foreach (var item in languages) {
                AllLanguagesItems.Add(item);
            }
            Settings.WindowsUI.LanguageID = currentLanguage;
        }

        private void ReloadThemes() {
            var curentTheme = Settings.WindowsUI.ThemeID;
            var themes = AppThemes.GetAvailableThemes().Select(
                themeID => {
                    return new ThemeItem {
                        ID = themeID, DisplayName = AppSubtitles["theme_" + themeID],
                        ChangeTheme = new Command(obj => { AppThemes.ChangeTheme(themeID); })
                    };
                })
                .ToList();
            AllThemesItems.Clear();
            foreach (var item in themes) {
                AllThemesItems.Add(item);
            }
            Settings.WindowsUI.ThemeID = curentTheme;
        }

    }
}