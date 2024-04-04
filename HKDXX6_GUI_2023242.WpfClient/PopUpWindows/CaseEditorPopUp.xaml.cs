using CommunityToolkit.Mvvm.Messaging;
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
using Xceed.Wpf.Toolkit;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows
{
    /// <summary>
    /// Interaction logic for CaseEditorPopUp.xaml
    /// </summary>
    public partial class CaseEditorPopUp : Window
    {
        public CaseEditorPopUp(FullCaseModel c, IMessenger messenger, bool edit = true)
        {
            InitializeComponent();
            CaseEditorPopUpViewModel viewModel = new CaseEditorPopUpViewModel();
            this.DataContext = viewModel;
            viewModel.Init(c, edit, () => { this.DialogResult = true; }, messenger);

        }
    }
}
