using AnalyseApi.Models;
using AnalyseInterfaces.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using AnalyseApi;
using AnalyseApi.Controllers;
using System.Reflection.Metadata;

namespace AnalyseInterfaces.ViewModels
{
    public class ControlsViewModel : ObservableObject, INavigationAware
    {
        public  ObservableCollection<Person> persons;

        private readonly INavigationService _navigationService;

        private readonly ITestWindowService _testWindowService;

        private ICommand _navigateCommand;

        private ICommand _openWindowCommand;

        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand<string>(OnNavigate);

        public ICommand OpenWindowCommand => _openWindowCommand ??= new RelayCommand<string>(OnOpenWindow);

        public ControlsViewModel(INavigationService navigationService, ITestWindowService testWindowService)
        {
            _navigationService = navigationService;
            _testWindowService = testWindowService;
            persons= new ObservableCollection<Person>();
            LoadData();
        }
        public ControlsViewModel()
        {
            persons = new ObservableCollection<Person>();
            LoadData();
        }

        public ObservableCollection<Person> Persons
        {
            get { return persons; }
            set
            {
                persons = value;
                OnPropertyChanged("Persons");
            }
        }

        public ICommand EditCommand;
        private void LoadData()
        {
           Persons = new ObservableCollection<Person>(PersonController.GetClientsWithAddress());

        }
        public void OnNavigatedTo() 
        {
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(ControlsViewModel)} navigated", "VCrim");
        }

        public void OnNavigatedFrom()
        {
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(ControlsViewModel)} navigated", "VCrim");
        }

        private void OnNavigate(string parameter)
        {
            switch (parameter)
            {

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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

