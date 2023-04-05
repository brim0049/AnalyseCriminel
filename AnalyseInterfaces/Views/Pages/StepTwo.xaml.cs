using AnalyseInterfaces.ViewModels;
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
    /// Logique d'interaction pour StepTwo.xaml
    /// </summary>
    public partial class StepTwo : INavigableView<DashboardViewModel>
    {
        private string NinToPass;

        public DashboardViewModel ViewModel
        {
            get;
        }
        public StepTwo(string Nin)
        {

            InitializeComponent();
            DataContext = new StepTwoViewModel(Nin);
            NinToPass= Nin; 
        }

        public StepTwo(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
        private void OnGoToSecondWindowClicked(object sender, RoutedEventArgs e)
        {

            Regex regex = new Regex(@"^\d{3}\d{3}\d{4}$");
            string phoneNumber = PhoneTxt.Text;

            if (regex.IsMatch(phoneNumber))
            {
                // Le numéro de téléphone est valide, continuez le traitement du formulaire.
                string CourantNIN = NIN2Txt.Text;
                NavigationService.Navigate(new StepTwoContinued(NinToPass, CourantNIN));
            }

        }
    }
}
