using AnalyseApi.Models;
using AnalyseInterfaces.ViewModels;
using AnalyseInterfaces.Views.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using Microsoft.Win32;
using System.Data;
using System.IO;
using CsvHelper;
using ClosedXML.Excel;
using System.ComponentModel;
using AnalyseApi;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AnalyseApi.Controllers;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using static ClosedXML.Excel.XLPredefinedFormat;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AnalyseInterfaces.Views.Pages
{
    /// <summary>
    /// Logique d'interaction pour StepOneContinued.xaml
    /// </summary>
    /// 
    public enum FileType
    {
        Csv,
        Excel
    }

    public partial class StepOneContinued : INavigableView<DashboardViewModel>
    {
        private string NinToPass;
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
        public StepOneContinued(string Nin)
        {

            InitializeComponent();
            DataContext = new StepOneContinuedViewModel(Nin);
            NinToPass = Nin;
            Calls = new List<Call>();
        }

        public StepOneContinued(DashboardViewModel viewModel)
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
        private void ClickTab3(object sender, RoutedEventArgs e)
        {
            MyTabControl.SelectedIndex = 3;

        }
        private void Clear(object sender, RoutedEventArgs e)
        {

            textBoxDuration.Text = TimeSpan.Zero.ToString();
            NumberBox.Text = "";
            ZoneBox.Text = "";
            MarkBox.Text = "";
            MatriculeBox.Text = "";
            DateBox.SelectedDate = System.DateTime.Now;
            DateBox2.SelectedDate = System.DateTime.Now;

        }

        private void OnGoToSecondWindowClicked(object sender, RoutedEventArgs e)
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
                    AnalyseApi.Models.Person client = _db.Persons.Include(User => User.Calls).Where(user => (user.NIN == NinToPass)).First();
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
                        Duration = System.DateTime.ParseExact(row.Cell(3).Value.ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).TimeOfDay,
                        Date = System.DateTime.ParseExact(row.Cell(4).Value.ToString(), format, CultureInfo.InvariantCulture)

                    };
                    calls.Add(call);
                }

                // Ajouter les enregistrements à la base de données
                using (var _db = new AnalyseDbContext())
                {
                    AnalyseApi.Models.Person client = _db.Persons.Include(User => User.Calls).Where(user => (user.NIN == NinToPass)).First();
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