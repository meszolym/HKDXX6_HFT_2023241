using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels;
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
        public PrecinctEditorPopUp(PrecinctModel p, bool addition = false)
        {
            InitializeComponent();
            this.DataContext = new PrecinctEditorPopUpViewModel();
            (this.DataContext as PrecinctEditorPopUpViewModel).Init(p);

            if (!addition)
            {
                tb_ID.IsEnabled= false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool fail = false;
            if (!int.TryParse(tb_ID.Text, out int id) || id < 1 || id >140)
            {
                tb_ID.Background = Brushes.OrangeRed;
                fail = true;
            }

            if (tb_Address.Text.Length < 10)
            {
                tb_Address.Background = Brushes.OrangeRed;
                fail = true;
            }

            if (fail)
            {
                return;
            }    

            foreach (var item in grid.Children)
            {
                if (item is TextBox tb)
                {

                    tb.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                }
            }
            this.DialogResult = true;
        }
    }
}
