using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Notepad.Model;
using Notepad.ViewModel;
using Notepad.View;
using System.Windows.Controls;

namespace Notepad {
    public enum ApplicationView { ShowingMainWindow, MinimizedToTray }


    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application {
        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public static new App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }
        public ApplicationWideViewModel AppViewModel { get; private set; }


        public App() {
            Services = ConfigureServices();
        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            AppViewModel = Services.GetRequiredService<ApplicationWideViewModel>();
            this.MainWindow = Services.GetRequiredService<MainWindow>();

            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            this.MainWindow.Show();          
        }


        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices() {
            var services = new ServiceCollection();

            services.AddSingleton<ISettingsEditor, SettingsEditor>();
            services.AddSingleton<ISettingsApplicationUI, SettingsApplicationUI>();
            services.AddSingleton<IGlobalSettings, GlobalSettings>();
            services.AddSingleton<IApplicationSubtitles, ApplicationSubtitles>();
            services.AddSingleton<IApplicationThemes, ApplicationThemes>();

            services.AddSingleton<ApplicationWideViewModel, ApplicationWideViewModel>();
            services.AddSingleton<BaseViewModel, BaseViewModel>();
            services.AddSingleton<MainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<SettingsWindowViewModel, SettingsWindowViewModel>();

            services.AddTransient<SettingsWindow, SettingsWindow>();
            services.AddSingleton<MainWindow, MainWindow>();

            services.AddSingleton<ApplicationSettingsPage, ApplicationSettingsPage>();
            services.AddSingleton<TextSettingsPage, TextSettingsPage>();

            services.AddTransient<DocumentViewModel, DocumentViewModel>();
            services.AddTransient<IDocumentModel, DocumentModel>();

            return services.BuildServiceProvider();
        }
    }
}
