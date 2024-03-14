using HKDXX6_GUI_2023242.WpfClient.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.Services.Interfaces
{
    public interface IPrecinctEditor
    {
        public bool Add(PrecinctModel precinctModel);
        public bool Edit(PrecinctModel precinctModel);
    }
}
