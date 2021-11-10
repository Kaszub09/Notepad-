using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Input;
using Notepad.Model;


namespace Notepad.ViewModel {
    public class TextSettingsViewModel:BaseViewModel {

        public ICommand PickColor { get; private set; }
        public BindingList<int> AvailableFontSizes { get; set; } = new BindingList<int>() 
            { 8, 9 ,10,11,12,14,16,18,20,22,24,26,36,48,72};

        public TextSettingsViewModel():base() {
            PickColor = new Command((obj) => {
                Color color = new Color();
                ColorDialog colorDialog = new ColorDialog();
                string colorName = obj as string;

                color = colorName switch {
                    "EditorFontColor" => AppViewModel.Settings.Editor.EditorFontColor,
                    "EditorBackgroundColor" => AppViewModel.Settings.Editor.EditorBackgroundColor,
                    _ => color
                };

                colorDialog.Color = ConvertColor(color);
                if (colorDialog.ShowDialog() == DialogResult.OK) {
                    color = ConvertColor(colorDialog.Color);
                    if(colorName == "EditorFontColor") {
                        AppViewModel.Settings.Editor.EditorFontColor = color;
                    }else if (colorName == "EditorBackgroundColor") {
                        AppViewModel.Settings.Editor.EditorBackgroundColor = color;
                    }
                }  
            });
        }

        public System.Drawing.Color ConvertColor(Color c) {
            return System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public Color ConvertColor(System.Drawing.Color c) {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
    }
}
