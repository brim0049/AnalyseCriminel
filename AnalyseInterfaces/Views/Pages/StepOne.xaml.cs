using AnalyseInterfaces.ViewModels;
using AnalyseInterfaces.Views.Windows;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Common.Interfaces;

namespace AnalyseInterfaces.Views.Pages
{
    /// <summary>
    /// Logique d'interaction pour StepOne.xaml
    /// </summary>
    public partial class StepOne : INavigableView<DashboardViewModel>
    {

        public DashboardViewModel ViewModel
        {
            get;
        }

        public StepOne()
        {

            InitializeComponent();

            StepOneViewModel StepTwo = new StepOneViewModel();
            DataContext = StepTwo;
        }

        public StepOne(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
        private void OnGoToSecondWindowClicked(object sender, RoutedEventArgs e)
        {
          
            string NIN = NINTxt.Text;
            Regex regex = new Regex(@"^\d{3}\d{3}\d{4}$");
            string phoneNumber = PhoneTxt.Text;

            if (regex.IsMatch(phoneNumber))
            {
                // Le numéro de téléphone est valide, continuez le traitement du formulaire.
                NavigationService.Navigate(new StepOneContinued(NIN));
            }
          
        }


    }
}