﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels
{
    public class CaseAutoAssignPopUpViewModel:ObservableObject
    {
        public List<PrecinctModel> Precincts { get; private set; }

        public string CaseName { get; private set; }

        PrecinctModel selectedItem;

        public PrecinctModel SelectedItem {
            get
            {
                return selectedItem;
            }
            set
            {
                SetProperty(ref selectedItem, value);
                assignerModel.PrecinctID = selectedItem.ID;
            }
        }

        AutoAssignCaseModel assignerModel;

        ICommand SaveCommand { get; set; }

        public CaseAutoAssignPopUpViewModel()
        {
            Precincts = new RestService("http://localhost:33410/", "Precinct").Get<PrecinctModel>("Precinct");
        }

        public void Init(string CaseName, AutoAssignCaseModel model)
        {
            this.CaseName = CaseName;
            assignerModel = model;
        }


    }
}
