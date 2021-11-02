using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Notepad.Model {
    public class DocumentModel : IDocumentModel {

        public Encoding WriteEncoding { get; set; }
        public bool IsSaved { get; private set; }

        private string _text;
        private string _filepath;


        public DocumentModel() {
            WriteEncoding = Encoding.Default;
            IsSaved = true;
        }

        public void LoadFromFile(string filepath) {
            _filepath = filepath;
            SetText(File.ReadAllText(_filepath));
            IsSaved = true;
        }

        public void ChangeFilepath(string newFilepath) {
            _filepath = newFilepath;
            IsSaved = false;
        }

        public void Save() {
            File.WriteAllText(_filepath, GetText(), WriteEncoding);
            IsSaved = true;
        }

        public void SetText(string value) {
            _text = value;
            IsSaved = false;
        }

        public string GetText() {
            return _text;
        }
        public bool HasFilepath() {
            return _filepath != null ? true : false;
        }
        public string GetFilepath() {
            return _filepath;
        }
        public string GetFilename() {
            if (_filepath == null) {
                return null;
            } else {
                return Path.GetFileName(_filepath);
            }
        }


    }
}
