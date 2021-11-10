using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Windows.Markup;

namespace Notepad.Model {
    [Serializable]
    public class SettingsEditor : ISettingsEditor, IXmlSerializable {
        public event PropertyChangedEventHandler PropertyChanged;
        [DoNotNotify]
        public bool AutosaveChanges { get; set; } = true;

        public FontFamily EditorFontFamily { get; set; } = new FontFamily();
        public FamilyTypeface EditorFontTypeface { get; set; } = new FamilyTypeface();
        public int EditorFontSize { get; set; } = 12;

        public Color EditorFontColor { get; set; } = Color.FromRgb(230, 230, 230);
        public Color EditorBackgroundColor { get; set; } = Color.FromRgb(0, 0, 0);

       

        public XmlSchema GetSchema() {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader) {
            //while (reader.EOF == false) {

            //    if (reader.MoveToContent() == XmlNodeType.Element) {
            //        try {
            //            if (reader.LocalName == "EditorFontFamily") {
            //                EditorFontFamily = (FontFamily)new FontFamilyConverter().ConvertFromString(reader.ReadElementContentAsString());
            //            } else if (reader.LocalName == "EditorFontTypeface") {
            //                var typeface = reader.ReadElementContentAsString();
            //                EditorFontTypeface = EditorFontFamily.FamilyTypefaces.FirstOrDefault(
            //                    tp => tp.AdjustedFaceNames[XmlLanguage.GetLanguage("en-US")] == typeface);
            //            } else if (reader.LocalName == "EditorFontSize") {
            //                EditorFontSize = int.Parse(reader.ReadElementContentAsString());
            //            } else if (reader.LocalName == "EditorFontColor") {
            //                EditorFontColor = (Color)ColorConverter.ConvertFromString(reader.ReadElementContentAsString());
            //            } else if (reader.LocalName == "EditorBackgroundColor") {
            //                EditorBackgroundColor = (Color)ColorConverter.ConvertFromString(reader.ReadElementContentAsString());
            //            } else {
            //                reader.Read();
            //            }
            //        }
            //        catch(Exception e) {
            //        }
            //    } else {
            //        reader.Read();
            //    }
            //}
            try {
                reader.MoveToContent();
                reader.Read();

                    EditorFontFamily = (FontFamily)new FontFamilyConverter().ConvertFromString(reader.ReadElementContentAsString());
 
                    var typeface = reader.ReadElementContentAsString();
                    EditorFontTypeface = EditorFontFamily.FamilyTypefaces.FirstOrDefault(
                        tp => tp.AdjustedFaceNames[XmlLanguage.GetLanguage("en-US")] == typeface);
                  
                    EditorFontSize = int.Parse(reader.ReadElementContentAsString());
                     
                    EditorFontColor = (Color)ColorConverter.ConvertFromString(reader.ReadElementContentAsString());
                      
                    EditorBackgroundColor = (Color)ColorConverter.ConvertFromString(reader.ReadElementContentAsString());
       
            } catch (Exception e) {
            }
        }

        public void WriteXml(XmlWriter writer) {
            writer.WriteElementString("EditorFontFamily", new FontFamilyConverter().ConvertToString(EditorFontFamily));
            writer.WriteElementString("EditorFontTypeface", EditorFontTypeface.AdjustedFaceNames[XmlLanguage.GetLanguage("en-US")]);
            writer.WriteElementString("EditorFontSize", EditorFontSize.ToString());
            writer.WriteElementString("EditorFontColor", EditorFontColor.ToString());
            writer.WriteElementString("EditorBackgroundColor", EditorBackgroundColor.ToString());
        }
    }
}
