using HKDXX6_GUI_2023242.WpfClient.Controls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HKDXX6_GUI_2023242.WpfClient.Controls
{
    /// <summary>
    /// Interaction logic for CasesControl.xaml
    /// </summary>
    public partial class CasesControl : UserControl
    {
        public CasesControl()
        {
            InitializeComponent();
        }

        public void RefreshLists()
        {
            (this.DataContext as IUserControlViewModel).RefreshLists();
        }
    }
}
