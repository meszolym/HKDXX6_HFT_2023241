using CommunityToolkit.Mvvm.DependencyInjection;
using HKDXX6_GUI_2023242.WpfClient.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HKDXX6_GUI_2023242.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddSingleton<ICaseEditor, CaseEditorViaWindow>()
                .AddSingleton<IOfficerEditor, OfficerEditorViaWindow>()
                .AddSingleton<IPrecinctEditor, PrecinctEditorViaWindow>()
                .BuildServiceProvider()
                );
        }
    }
}
