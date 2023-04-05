using AnalyseInterfaces.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace AnalyseInterfaces.ViewModels
{
    public class DashboardViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly ITestWindowService _testWindowService;

        private ICommand _navigateCommand;

        private ICommand _openWindowCommand;

        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand<string>(OnNavigate);

        public ICommand OpenWindowCommand => _openWindowCommand ??= new RelayCommand<string>(OnOpenWindow);

        public DashboardViewModel(INavigationService navigationService, ITestWindowService testWindowService)
        {
            _navigationService = navigationService;
            _testWindowService = testWindowService;
        }

        public void OnNavigatedTo()
        {
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(DashboardViewModel)} navigated", "VCrim");
        }

        public void OnNavigatedFrom()
        {
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(DashboardViewModel)} navigated", "VCrim");
        }

        private void OnNavigate(string parameter)
        {
            switch (parameter)
            {
                case "navigate_to_input":
                    _navigationService.Navigate(typeof(Views.Pages.Controls));
                    return;
            }
        }

        private void OnOpenWindow(string parameter)
        {
            switch (parameter)
            {
                case "open_window_visualize":
                    _testWindowService.Show<MainWindow>();
                    return;
            }
        }
    }

}