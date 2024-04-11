using HKDXX6_GUI_2023242.WpfClient.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HKDXX6_GUI_2023242.WpfClient.Services
{
    public class ErrorViaMessageBox : IErrorService
    {
        public const MessageBoxButton button = MessageBoxButton.OK;
        public const MessageBoxImage icon = MessageBoxImage.Error;
        public void ShowError(string message, string caption)
        {
            MessageBox.Show(message, caption, button, icon);
        }
    }
}
