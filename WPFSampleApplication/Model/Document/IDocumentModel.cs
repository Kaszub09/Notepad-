using System.Text;

namespace Notepad.Model {
    public interface IDocumentModel {
        bool IsSaved { get; }
        Encoding WriteEncoding { get; set; }

        void ChangeFilepath(string newFilepath);
        string GetFilename();
        string GetFilepath();
        string GetText();
        bool HasFilepath();
        void LoadFromFile(string filepath);
        void Save();
        void SetText(string value);
    }
}