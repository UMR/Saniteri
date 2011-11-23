using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace UMR.Saniteri.Common
{
    public class DialogManager
    {
        public static Action<string> popup = (Action<string>)(msg => MessageBox.Show(msg));
        public static Func<string, string, bool> confirm = (Func<string, string, bool>)((msg, capt) =>
                 MessageBox.Show(msg, capt, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes);
        public static Func<string, string> openFile = (Func<string, string>)((filter) =>
        {
            if (string.IsNullOrEmpty(filter)) filter = "Firmware Files (*.bin)|*.bin";
            var fileName = string.Empty;
            var dialog = new OpenFileDialog();
            dialog.CheckPathExists = true;
            dialog.Multiselect = false;
            dialog.Filter = filter;
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
                fileName = dialog.FileNames.FirstOrDefault();
            return fileName;
        });
    }
}
