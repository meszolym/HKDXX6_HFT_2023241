using CommunityToolkit.Mvvm.Messaging;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.Services.Interfaces
{
    public interface ICaseEditor
    {
        public bool Add(FullCaseModel caseModel, IMessenger messenger);
        public bool Edit(FullCaseModel caseModel, IMessenger messenger);
        public bool AutoAssign(FullCaseModel caseModel, IMessenger messenger);
        void ShowDetails(FullCaseModel selectedItem);
    }
}
