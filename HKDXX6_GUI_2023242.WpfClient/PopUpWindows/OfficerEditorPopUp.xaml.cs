using CommunityToolkit.Mvvm.Messaging;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels;
using HKDXX6_GUI_2023242.WpfClient.Tools;
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
    /// Interaction logic for OfficerEditorPopUp.xaml
    /// </summary>
    public partial class OfficerEditorPopUp : Window
    {

        OfficerEditorPopUpViewModel viewModel;

        public OfficerEditorPopUp(FullOfficerModel o, IMessenger messenger)
        {
            InitializeComponent();
            viewModel = new OfficerEditorPopUpViewModel();
            this.DataContext = viewModel;
            viewModel.Init(o, () => { this.DialogResult = true; }, messenger); 
        }
    }
}
