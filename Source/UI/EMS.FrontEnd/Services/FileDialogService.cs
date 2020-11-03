namespace EMS.FrontEnd.Services {
    using Microsoft.Win32;

    public class FileDialogService : IFileDialogService {
        public string OpenDialog() {
            var dialog = new SaveFileDialog();
            var result = dialog.ShowDialog() ?? false;
            return result ? dialog.FileName : null;
        }
    }
}