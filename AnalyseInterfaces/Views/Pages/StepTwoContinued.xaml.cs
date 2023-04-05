using AnalyseApi;
using AnalyseApi.Models;
using AnalyseInterfaces.ViewModels;
using AnalyseInterfaces.Views.Windows;
using ClosedXML.Excel;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
    /// Logique d'interaction pour StepTwoContinued.xaml
    /// </summary>
    public partial class StepTwoContinued : INavigableView<DashboardViewModel>
    {
        private string NinToPass;
        private string NinCourant;
        private string _filePath;
        private FileType _fileType;
        private List<Call> _calls;

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public FileType FileType
        {
            get => _fileType;
            set
            {
                _fileType = value;
                OnPropertyChanged(nameof(FileType));
            }
        }

        public List<Call> Calls
        {
            get => _calls;
            set
            {
                _calls = value;
                OnPropertyChanged(nameof(Calls));
            }
        }
        public TimeSpan Duration { get; set; }
        public DashboardViewModel ViewModel
        {
            get;
        }
        public StepTwoContinued(string Nin, string CourantNIN)
        {

            InitializeComponent();
            DataContext = new StepTwoContinuedViewModel(Nin, CourantNIN);
            NinToPass = Nin;
            NinCourant= CourantNIN;
            Calls = new List<Call>();

        }

        public StepTwoContinued(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
        private void ClickTab(object sender, RoutedEventArgs e)
        {
            MyTabControl.SelectedIndex = 1;

        }
        private void ClickTab2(object sender, RoutedEventArgs e)
        {
            Regex regex1 = new Regex(@"^\d{3}\d{3}\d{4}$");
            string input = Duration.ToString();
            Regex regex = new Regex(@"^([0-9]{2}):([0-5][0-9]):([0-5][0-9])$");
            Match match = regex.Match(input);

            if (regex1.IsMatch(NumberBox.Text))
            {
                if (match.Success)
                {
                    MyTabControl.SelectedIndex = 2;
                    return;
                }
            }
        }
      
        private void Clear(object sender, RoutedEventArgs e)
        {
            NumberBox.Text = "";
            ZoneBox.Text = "";
            MarkBox.Text = "";
            MatriculeBox.Text = "";
            DateBox.SelectedDate = DateTime.Now;
        }
        private void OnGoToSecondWindowClicked(object sender, RoutedEventArgs e)
        {
            MarkBox.Text = "";
            MatriculeBox.Text = "";
            MainWindow secondWindow = new MainWindow(NinToPass);
            secondWindow.Show();
        }
        private void AjouterPersonClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StepTwo(NinToPass));
        }
        private void OnBrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx|CSV files (*.csv)|*.csv";
            if (dialog.ShowDialog() == true)
            {
                FilePath = dialog.FileName;
                FileType = dialog.FileName.ToLower().EndsWith(".csv") ? FileType.Csv : FileType.Excel;
                ImportDataToDatabase(FilePath, FileType);
            }
        }

        private void ImportDataToDatabase(string filePath, FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Csv:
                    ImportCsv(filePath);
                    break;
                case FileType.Excel:
                    ImportExcel(filePath);
                    break;
                default:
                    throw new ArgumentException($"Unsupported file type: {fileType}");
            }

            using (var context = new AnalyseDbContext())
            {
                Calls = context.Calls.ToList();
            }
        }

        private void ImportCsv(string csvFilePath)
        {
            // Lire le fichier CSV en utilisant CsvHelper
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader((IParser)reader))
            {
                var records = csv.GetRecords<Call>().ToList();

                // Ajouter les enregistrements à la base de données
                using (var _db = new AnalyseDbContext())
                {
                    AnalyseApi.Models.Person client = _db.Persons.Include(User => User.Calls).Where(user => (user.NIN == NinCourant)).First();
                    if (client != null)
                    {
                        foreach (var call in records)
                        {
                            client.Calls.Add(call);
                        }
                        _db.SaveChanges();
                    }
                }
            }
            MyTabControl.SelectedIndex = 2;
        }
        private void ImportExcel(string excelFilePath)
        {
            // Lire le fichier Excel en utilisant ClosedXML
            using (var workbook = new XLWorkbook(excelFilePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1); // sauter la première ligne d'en-tête

                // Convertir les lignes en objets Call
                var calls = new List<Call>();
                // Définition du format de la chaîne de date
                string format = "dd/MM/yyyy HH:mm:ss";
                string formatSpan = "HH:mm:ss";
                foreach (var row in rows)
                {
                    var call = new Call()
                    {
                        Number = row.Cell(1).Value.ToString(),
                        Zone = row.Cell(2).Value.ToString(),
                        Duration = DateTime.ParseExact(row.Cell(3).Value.ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).TimeOfDay,
                        Date = DateTime.ParseExact(row.Cell(4).Value.ToString(), format, CultureInfo.InvariantCulture)

                    };
                    calls.Add(call);
                }
           // Ajouter les enregistrements à la base de données
                using (var _db = new AnalyseDbContext())
                {
                    AnalyseApi.Models.Person client = _db.Persons.Include(User => User.Calls).Where(user => (user.NIN == NinCourant)).First();
                    if (client != null)
                    {
                        foreach (var call in calls)
                        {
                            client.Calls.Add(call);
                        }
                        _db.SaveChanges();
                    }
                }
                MyTabControl.SelectedIndex = 2;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}