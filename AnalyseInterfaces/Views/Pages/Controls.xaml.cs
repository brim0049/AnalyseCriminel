using AnalyseInterfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Interfaces;
using AnalyseApi.Models;
using AnalyseApi.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.ObjectModel;

namespace AnalyseInterfaces.Views.Pages
{
    /// <summary>
    /// Logique d'interaction pour Controls.xaml
    /// </summary>
    public partial class Controls : INavigableView<ControlsViewModel>
    {
        string Nin;
        public ControlsViewModel ViewModel
        {
            get;
        }
        public Controls()
        {

            InitializeComponent();
            ControlsViewModel Controls = new ControlsViewModel();
            DataContext = Controls;
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            Load();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Person> persons;
            myDataGrid.ItemsSource = new ObservableCollection<Person>(PersonController.GetClientsWithAddress());
            // Mettre à jour l'affichage du DataGrid
            myDataGrid.Items.Refresh();
        }

        public Controls(ControlsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
        public void Load()
        {
          
                ObservableCollection<Person> persons;
                myDataGrid.ItemsSource = new ObservableCollection<Person>(PersonController.GetClientsWithAddress());
                // Mettre à jour l'affichage du DataGrid
                myDataGrid.Items.Refresh();
        }
        private void Delete_Button(object sender, RoutedEventArgs e)
        {
            var selectedItem = myDataGrid.SelectedItem as Person;
            if (selectedItem != null)
            {
                PersonController.DeletePerson(selectedItem.NIN);
                ObservableCollection<Person> persons;
                myDataGrid.ItemsSource = new ObservableCollection<Person>(PersonController.GetClientsWithAddress());
                // Mettre à jour l'affichage du DataGrid
                myDataGrid.Items.Refresh();
            }
        }
        private void Edit_Button(object sender, RoutedEventArgs e)
        {
            // Obtenir la ligne sélectionnée
            var row = (DataGridRow)myDataGrid.ItemContainerGenerator.ContainerFromItem(((FrameworkElement)sender).DataContext);

            // Vérifier si la ligne a été modifiée
            if (row.IsEditing)
            {
                // Terminer l'édition de la ligne
                row.BindingGroup.CommitEdit();
            }
            var obj = (Person)row.DataContext;
            PersonController.UpdatePerson(obj,obj.NIN);
        }
        private void appel_Button(object sender, RoutedEventArgs e)
        {
            var selectedItem = myDataGrid.SelectedItem as Person;
            if (selectedItem != null)
            {
                ObservableCollection<Call> calls;
                AppelDataGrid.ItemsSource = new ObservableCollection<Call>(PersonController.GetClientCalls(selectedItem.NIN));
                // Mettre à jour l'affichage du DataGrid
                AppelDataGrid.Items.Refresh();
            }
            AppelPopup.IsOpen = true;
            Nin=selectedItem.NIN;
           
        }
        private void pro_Button(object sender, RoutedEventArgs e)
        {
            var selectedItem = myDataGrid.SelectedItem as Person;
            if (selectedItem != null)
            {
                ObservableCollection<Car> Cars;
                ProDataGrid.ItemsSource = new ObservableCollection<Car>(PersonController.GetClientCars(selectedItem.NIN));
                // Mettre à jour l'affichage du DataGrid
                ProDataGrid.Items.Refresh();
            }
            ProPopup.IsOpen = true;
            Nin = selectedItem.NIN;
        }
        private void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (AppelPopup.IsOpen && !AppelPopup.IsMouseOver)
            {
                AppelPopup.IsOpen = false;
            }
            if (ProPopup.IsOpen && !ProPopup.IsMouseOver)
            {
                ProPopup.IsOpen = false;
            }
        }
        private void Delete_Appel_Button(object sender, RoutedEventArgs e)
        {
            var selectedItem = AppelDataGrid.SelectedItem as Call;
            if (selectedItem != null)
            {
                

                PersonController.DeleteCall(selectedItem.CallId);
                ObservableCollection<Call> calls;
                AppelDataGrid.ItemsSource = new ObservableCollection<Call>(PersonController.GetClientCalls(Nin));
                // Mettre à jour l'affichage du DataGrid
                AppelDataGrid.Items.Refresh();
            }
        }
        private void Delete_Pro_Button(object sender, RoutedEventArgs e)
        {
            var selectedItem = ProDataGrid.SelectedItem as Car;
            if (selectedItem != null)
            {


                PersonController.DeleteCar(selectedItem.CarId);
                ObservableCollection<Car> cars;
                ProDataGrid.ItemsSource = new ObservableCollection<Car>(PersonController.GetClientCars(Nin));
                // Mettre à jour l'affichage du DataGrid
                ProDataGrid.Items.Refresh();
            }
        }
        private void Close_Button(object sender, RoutedEventArgs e)
        {
            AppelPopup.IsOpen = false;
            ProPopup.IsOpen = false;
        }
    }
}