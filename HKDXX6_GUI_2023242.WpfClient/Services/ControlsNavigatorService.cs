using HKDXX6_GUI_2023242.WpfClient.Controls;
using HKDXX6_GUI_2023242.WpfClient.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HKDXX6_GUI_2023242.WpfClient.Services
{
    public class ControlsNavigatorService : IControlsNavigator
    {

        CasesControl casesControl;
        OfficersControl officersControl;
        PrecinctsControl precinctsControl;
        WelcomeControl welcomeControl;

        public UserControl CasesControl
        {
            get
            {
                if (casesControl == null)
                {
                    casesControl = new CasesControl();
                }
                return casesControl;
            }
        }

        public UserControl PrecinctsControl
        {
            get
            {
                if (precinctsControl == null)
                {
                    precinctsControl = new PrecinctsControl();
                }
                return precinctsControl;
            }
        }
        public UserControl OfficersControl
        {
            get
            {
                if (officersControl == null)
                {
                    officersControl = new OfficersControl();
                }
                return officersControl;
            }
        }

        public UserControl WelcomeControl
        {
            get
            {
                if (welcomeControl == null)
                {
                    welcomeControl = new WelcomeControl();
                }
                return welcomeControl;
            }
        }
    }
}
