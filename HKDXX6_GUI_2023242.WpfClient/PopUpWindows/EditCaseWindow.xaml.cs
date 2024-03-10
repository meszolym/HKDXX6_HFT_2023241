using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Xceed.Wpf.Toolkit;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows
{
    /// <summary>
    /// Interaction logic for EditCaseWindow.xaml
    /// </summary>
    public partial class EditCaseWindow : Window
    {
        public EditCaseWindow(FullCaseModel @case, bool editable=true)
        {
            InitializeComponent();
            this.DataContext = new EditCaseWindowViewModel(@case);
            if (!editable)
            {
                foreach(var item in dockpanel.Children)
                {
                    if (item is not Label)
                    {
                        (item as Control).IsEnabled = false;
                    }
                }
                bt_save.Visibility = Visibility.Hidden;
            }
        }

        public EditCaseWindow(ref FullCaseModel @case)
        {
            InitializeComponent();
            this.DataContext = new EditCaseWindowViewModel(@case);
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in dockpanel.Children)
            {
                if (item is TextBox tb && tb.IsEnabled)
                {
                    tb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    continue;
                }
                if (item is DateTimePicker dtp && dtp.IsEnabled)
                {
                    dtp.GetBindingExpression(DateTimePicker.ValueProperty).UpdateSource();
                    continue;
                }
                if (item is ComboBox cmb && cmb.IsEnabled)
                {
                    cmb.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
                    continue;
                }
            }

            this.DialogResult = true;

        }
    }
}
