using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HKDXX6_GUI_2023242.WpfClient.Services.Interfaces
{
    public interface IControlsNavigator
    {

        public UserControl CasesControl { get; }

        public UserControl PrecinctsControl { get; }

        public UserControl OfficersControl { get; }

        public UserControl WelcomeControl { get; }
    }
}
