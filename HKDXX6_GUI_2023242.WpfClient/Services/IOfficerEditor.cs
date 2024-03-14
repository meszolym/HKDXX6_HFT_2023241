using HKDXX6_GUI_2023242.WpfClient.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.Services
{
    public interface IOfficerEditor
    {
        public bool Add(FullOfficerModel officerModel);
        public bool Edit(FullOfficerModel officerModel);
    }
}
