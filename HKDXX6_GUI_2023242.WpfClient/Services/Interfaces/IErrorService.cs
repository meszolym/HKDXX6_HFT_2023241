using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HKDXX6_GUI_2023242.WpfClient.Services.Interfaces
{
    public interface IErrorService
    {
        public void ShowError(string message, string caption);
    }
}
