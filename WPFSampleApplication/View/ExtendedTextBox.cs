using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Notepad.ViewModel;

namespace Notepad.View {
    public class ExtendedTextBox : TextBox,INotifyPropertyChanged{

        private double _prevVerticalOfffset;
        private double _prevWidth;

        public event PropertyChangedEventHandler PropertyChanged;

        public Thickness TextBoxNumbersPadding { get; set; }
        public Thickness TextBoxMargin { get; set; }
        public Thickness TopMarginForOverlay { get; set; }

        public ExtendedTextBox() : base() {
            base.LayoutUpdated += ExtendedTextBox_LayoutUpdated; ;
            base.TextChanged += ExtendedTextBox_TextChanged;
            base.SelectionChanged += ExtendedTextBox_SelectionChanged;

            ExtendedTextBox_SelectionChanged(null, null);
            ExtendedTextBox_TextChanged(null, null);
        }


        private void ExtendedTextBox_SelectionChanged(object sender, RoutedEventArgs e) {
            if (base.IsEnabled) {
                var dt = this.DataContext as DocumentViewModel;
                if (dt != null) {
                    dt.CaretPosition = base.CaretIndex+1;
                    dt.CaretLine = base.GetLineIndexFromCharacterIndex(base.CaretIndex) + 1;
                    dt.CaretColumn = base.CaretIndex - base.GetCharacterIndexFromLineIndex(base.GetLineIndexFromCharacterIndex(base.CaretIndex)) + 1;
                    TopMarginForOverlay = new Thickness() { Top = (dt.CaretLine-base.GetFirstVisibleLineIndex()-1)*(base.FontSize*1.15) };
                }
            }       
        }

        private void ExtendedTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (base.IsEnabled) {
                var dt = this.DataContext as DocumentViewModel;
                if (dt != null) {
                    dt.LinesNumber = base.LineCount;
                    dt.CharactersNumber = base.Text.Length;
                }
            }
        }

        private void ExtendedTextBox_LayoutUpdated(object sender, EventArgs e) {
            if(_prevVerticalOfffset!= base.VerticalOffset) {
                _prevVerticalOfffset = base.VerticalOffset;
                TextBoxNumbersPadding = new Thickness() { Top = -1 * VerticalOffset };
            }
            if (_prevWidth != base.ActualWidth) {
                _prevWidth = base.ActualWidth;
                TextBoxMargin = new Thickness() { Left = base.ActualWidth ,Top=-1};
            }
        }
    }
}
