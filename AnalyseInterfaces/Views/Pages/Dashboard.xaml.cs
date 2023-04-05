using AnalyseInterfaces.Services;
using AnalyseInterfaces.ViewModels;
using AnalyseInterfaces.Views.Windows;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace AnalyseInterfaces.Views.Pages
{
    /// <summary>
    /// Logique d'interaction pour Dashboard.xaml
    /// </summary>
    public partial class Dashboard :INavigableView<DashboardViewModel>
    {
        
        public DashboardViewModel ViewModel
        {
            get;
        }
        public Dashboard( )
        {

            InitializeComponent();
            DataContext = this;
        }

        public Dashboard(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
        private void OnGoToSecondWindowClicked(object sender, RoutedEventArgs e)
        {
            GlobalWindow secondWindow = new GlobalWindow();
            secondWindow.Show();
           
        }
    }
}
