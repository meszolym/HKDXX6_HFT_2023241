using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.ViewModels;
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
using System.Windows.Shapes;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows
{
    /// <summary>
    /// Interaction logic for PrecinctEditor.xaml
    /// </summary>
    public partial class PrecinctEditorPopUp : Window
    {
        public PrecinctEditorPopUp(PrecinctModel p)
        {
            InitializeComponent();
            this.DataContext = new PrecinctEditorPopUpViewModel();
            (this.DataContext as PrecinctEditorPopUpViewModel).Init(p);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in grid.Children)
            {
                if (item is TextBox tb)
                {
                    try
                    {
                        tb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                        tb.Background = default;
                    }
                    catch
                    {
                        tb.Background = Brushes.OrangeRed;
                    }
                }
            }
            this.DialogResult = true;
        }
    }
}
