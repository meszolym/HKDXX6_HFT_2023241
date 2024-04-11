using HKDXX6_GUI_2023242.WpfClient.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HKDXX6_GUI_2023242.WpfClient.Services
{
    public class ErrorServiceMessageBox : IErrorService
    {
        public void ShowError(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            MessageBox.Show(message, caption, button, icon);
        }
    }
}
