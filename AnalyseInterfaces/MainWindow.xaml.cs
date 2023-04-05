using GraphSharp.Controls;
using QuickGraph;
using GraphX;
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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Globalization;
using AnalyseInterfaces.ViewModels;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AnalyseApi.Models;
using AnalyseInterfaces.Views.Pages;
using AnalyseApi.Controllers;
using System.Drawing;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using AnalyseApi;
using DocumentFormat.OpenXml.InkML;
using System.Diagnostics;

namespace AnalyseInterfaces
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel StepOne = new MainWindowViewModel();
        private List<Call> _filteredCalls;
        public static DateTime? debutDate = null;
        public static DateTime? finDate=null;
        public MainWindow(string Nin)
        {
            InitializeComponent();
            DataContext = this;
            StepOne = new MainWindowViewModel(Nin);
            CreateGraph();
        }

        private void Afficher_Click(object sender, RoutedEventArgs e)
        {
            if (DateDebut.SelectedDate != null && DateFin.SelectedDate != null)
            {
                DateTime debut = DateDebut.SelectedDate.Value; ;
                DateTime fin = DateFin.SelectedDate.Value;

                debutDate = debut;
                finDate = fin;

            }
            this.Close();
            MainWindow secondWindow = new MainWindow(StepOne.Victim.NIN);
            secondWindow.Show();
            debutDate = null;
            finDate = null;

        }

        private readonly BidirectionalGraph<object, IEdge<object>> _graph = new BidirectionalGraph<object, IEdge<object>>();
        public IBidirectionalGraph<object, IEdge<object>> Graph
        {
            get { return _graph; }

        }
        private string _layoutAlgorithm = "EfficientSugiyama";
        public string LayoutAlgorithm
        {
            get { return _layoutAlgorithm; }
            set
            {
                if (value != _layoutAlgorithm)
                {
                    _layoutAlgorithm = value;
                }
            }
        }
        private void CreateGraph()
        {
            _graph.Clear();
            SampleVertex victim = new SampleVertex($"{StepOne.Victim.FirstName} {StepOne.Victim.LastName}", "pack://application:,,,/Views/Resources/victim.png");
            _graph.AddVertex(victim);
            SampleVertex eventt = new SampleVertex($" Évenement: {StepOne.Event.Event.Type}\n Date d'evenement: {StepOne.Event.Event.Date.ToString("yyyy/MM/dd")}", "pack://application:,,,/Views/Resources/event.png");
            _graph.AddVertex(eventt);
            SampleVertex address = new SampleVertex($"{StepOne.Address.Address.NoStreet}, {StepOne.Address.Address.NameStreet}, {StepOne.Address.Address.Ville}, {StepOne.Address.Address.Zip}", "pack://application:,,,/Views/Resources/address.png");
            _graph.AddVertex(address);
            SampleVertex addressEvent = new SampleVertex($"{StepOne.AddressEvent.Address.NoStreet}, {StepOne.AddressEvent.Address.NameStreet}, {StepOne.AddressEvent.Address.Ville}, {StepOne.AddressEvent.Address.Zip}", "pack://application:,,,/Views/Resources/address.png");
            _graph.AddVertex(addressEvent);

            foreach (var call in (debutDate is null && finDate is null) ? StepOne.Calls : StepOne.Calls.Where(call => call.Date >= debutDate && call.Date <= finDate))
            {
                SampleVertex calls = new SampleVertex($"Numero: {call.Number} \nDate d'appel: {call.Date.ToString("yyyy/MM/dd")}\nHeure d'appel: {call.Date.ToString("HH:mm:ss")}\nDurée d'appel: {call.Duration.ToString()} ,", "pack://application:,,,/Views/Resources/call.png");
                _graph.AddVertex(calls);
                _graph.AddEdge(new Edge<object>(victim, calls));
                foreach (var pers in PersonController.GetClients())
                {
                    if (call.Number == pers.Phone)
                    {
                        SampleVertex calper = new SampleVertex($"{pers.FirstName} {pers.LastName} ,", (pers.CriminalRecord == CriminalRecordType.Non) ? "pack://application:,,,/Views/Resources/personNocCrim.png" : "pack://application:,,,/Views/Resources/suspect.png");
                        _graph.AddVertex(calper);
                        _graph.AddEdge(new Edge<object>(calls, calper));
                    }
                }
            }
            foreach (var voiture in StepOne.Cars)
            {
                SampleVertex cars = new SampleVertex($"Marque: {voiture.Mark} \nMatricule:{voiture.Matricule} ,", "pack://application:,,,/Views/Resources/car.png");
                _graph.AddVertex(cars);
                _graph.AddEdge(new Edge<object>(victim, cars));
            }

            _graph.AddEdge(new Edge<object>(victim, eventt));
            _graph.AddEdge(new Edge<object>(victim, address));
            _graph.AddEdge(new Edge<object>(eventt, addressEvent));
            foreach (var relation in StepOne.Relations)
            {
                SampleVertex relationSuspect = new SampleVertex($"{relation.Relation}", (relation.Relation == RelationType.Aucun) ? "pack://application:,,,/Views/Resources/noRelation.png" : "pack://application:,,,/Views/Resources/relation.png");
                _graph.AddVertex(relationSuspect);
                _graph.AddEdge(new Edge<object>(victim, relationSuspect));
                Person person = PersonController.GetClient(relation.SuspectId);
                Person addressSusp = PersonController.GetClientAddress(person.NIN);
                SampleVertex suspect = new SampleVertex($"{person.FirstName} {person.LastName}", (person.CriminalRecord == CriminalRecordType.Non) ? "pack://application:,,,/Views/Resources/personNocCrim.png" : "pack://application:,,,/Views/Resources/suspect.png");
                _graph.AddVertex(suspect);
                _graph.AddEdge(new Edge<object>(relationSuspect, suspect));
                SampleVertex addressSuspect = new SampleVertex($"{addressSusp.Address.NoStreet}, {addressSusp.Address.NameStreet}, {addressSusp.Address.Ville}, {addressSusp.Address.Zip}", "pack://application:,,,/Views/Resources/address.png");
                _graph.AddVertex(addressSuspect);
                _graph.AddEdge(new Edge<object>(suspect, addressSuspect));

                foreach (var call in (debutDate is null && finDate is null)?PersonController.GetClientCalls(relation.SuspectId): PersonController.GetClientCalls(relation.SuspectId).Where(call => call.Date >= debutDate && call.Date <= finDate))
                {
                    SampleVertex calls = new SampleVertex($"Numero: {call.Number} \nDate d'appel: {call.Date.ToString("yyyy/MM/dd")}\nHeure d'appel: {call.Date.ToString("HH:mm:ss")}\nDurée d'appel: {call.Duration.ToString("")}  ,", "pack://application:,,,/Views/Resources/call.png");
                    _graph.AddVertex(calls);
                    _graph.AddEdge(new Edge<object>(suspect, calls));
                    foreach (var pers in PersonController.GetClients())
                    {
                        if (call.Number == pers.Phone)
                        {
                            SampleVertex calper = new SampleVertex($"{pers.FirstName} {pers.LastName} ,", (pers.CriminalRecord == CriminalRecordType.Non) ? "pack://application:,,,/Views/Resources/personNocCrim.png" : "pack://application:,,,/Views/Resources/suspect.png");
                            _graph.AddVertex(calper);
                            _graph.AddEdge(new Edge<object>(calls, calper));
                        }
                    }
                }
                foreach (var voiture in PersonController.GetClientCars(relation.SuspectId))
                {
                    SampleVertex cars = new SampleVertex($"Marque: {voiture.Mark} \nMatricule:{voiture.Matricule} ,", "pack://application:,,,/Views/Resources/car.png");
                    _graph.AddVertex(cars);
                    _graph.AddEdge(new Edge<object>(suspect, cars));
                }

            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            var vertex = menuItem.Tag as SampleVertex;
            vertex.Change();
        }
    }
    public class SampleVertex : INotifyPropertyChanged
    {
        private bool _active;
        private string _text; 
        private string _imagePath;
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                NotifyChanged("Active");
            }
        }
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                NotifyChanged("Text");
            }
        }
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                NotifyChanged("ImagePath");
            }
        }
        public SampleVertex(string text, string imagePath)
        {
            Text = text;

            ImagePath = imagePath;
        }
        public void Change()
        {
            Active = !Active;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    [ValueConversion(typeof(bool), typeof(System.Windows.Media.Brush))]
    public class ActiveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (bool)value;
            if (state)
            {
                return new SolidColorBrush(Colors.WhiteSmoke);
            }
            else
            {
                return new SolidColorBrush(Colors.LightSalmon);
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }



}
























//var graph = new Graph();

//Graph.AddEdge("Node 1", "Node 2");

//GraphLayout.Graph = graph;


//    var graph = new AdjacencyGraph<string, Edge<string>>();
//    graph.AddVertex("Node 1");
//    graph.AddVertex("Node 2");
//    graph.AddVertex("Node 3");
//    graph.AddEdge(new Edge<string>("Node 1", "Node 2"));
//    graph.AddEdge(new Edge<string>("Node 2", "Node 3"));

//    var graphviz = new GraphvizAlgorithm<string, Edge<string>>(graph);
//    graphviz.FormatVertex += GraphvizFormatVertex;
//    var graphvizCode = graphviz.Generate();
//    GraphDisplay.Text = graphvizCode;
//}

//private void GraphvizFormatVertex(object sender, FormatVertexEventArgs<string> e)
//{
//    e.VertexFormatter.Label = e.Vertex;
//}

//var graph = new Graph();

//graph.AddEdge("Node 1", "Node 2");

//graphLayout.Graph = graph;



















































//public partial class MainWindow : Window
//{
//    public MainWindow()
//    {
//        InitializeComponent();

//        foreach (TreeViewItem item in treeView.Items)
//        {
//            VisualizeNode(item);
//        }
//    }

//    private void VisualizeNode(TreeViewItem item)
//    {
//        // Create a rectangle to represent the node
//        Rectangle rect = new Rectangle();
//        rect.Width = 100;
//        rect.Height = 50;
//        rect.Fill = System.Windows.Media.Brushes.LightBlue;

//        // Create a label to display the node's header text
//        Label label = new Label();
//        label.Content = item.Header;
//        label.HorizontalAlignment = HorizontalAlignment.Center;
//        label.VerticalAlignment = VerticalAlignment.Center;

//        // Add the label to the rectangle
//        StackPanel panel = new StackPanel();
//        panel.Children.Add(rect);
//        panel.Children.Add(label);

//        // Add the panel to the TreeViewItem
//        item.Header = panel;

//        // Recursively visualize child nodes
//        foreach (TreeViewItem childItem in item.Items)
//        {
//            VisualizeNode(childItem);
//        }
//    }
//}


