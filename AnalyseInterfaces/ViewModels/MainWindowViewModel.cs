using AnalyseApi;
using AnalyseApi.Controllers;
using AnalyseApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace AnalyseInterfaces.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public Person Victim;
        public Person Event;
        public Person Address;
        public Event AddressEvent;
        public List<Call> Calls;
        public List<Car> Cars;
        public List<Relationship> Relations;
        
        public MainWindowViewModel()
        {
        }
      
        public MainWindowViewModel(string Nin) {

            Victim = PersonController.GetClient(Nin);
            Event = PersonController.GetClientEvent( Nin);
            Address = PersonController.GetClientAddress(Nin);
            AddressEvent = PersonController.GetClientEventAddress(Nin);
            Calls = PersonController.GetClientCalls(Nin);
            Cars = PersonController.GetClientCars(Nin);
            Relations = PersonController.GetClientRelation(Nin);
        }    
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
