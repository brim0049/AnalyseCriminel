using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.CompilerServices;
using AnalyseApi;
using AnalyseApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using AnalyseApi.Controllers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using AnalyseInterfaces.Views.Pages;
using Wpf.Ui.Mvvm.Services;
using System.Text.RegularExpressions;
using System.Net;

namespace AnalyseInterfaces.ViewModels
{
    public class StepTwoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Person> Persons;

        public Person _person;
        public Relationship _relation;

        public IEnumerable<CriminalRecordType> CriminalRecord => Enum.GetValues(typeof(CriminalRecordType)).Cast<CriminalRecordType>();
        public IEnumerable<RelationType> Relation => Enum.GetValues(typeof(RelationType)).Cast<RelationType>();


        private string _VictimNIN;
        public string VictimNIN
        {
            get { return _VictimNIN; }
            set
            {
                _VictimNIN = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VictimNIN)));
            }
        }
        public StepTwoViewModel(string Nin)
        {
            _person = new Person();
            _relation = new Relationship();
            AddUserCommand = new RelayCommand(AddPerson);
            VictimNIN = Nin;

        }
        public string NIN
        {
            get { return _person.NIN; }
            set
            {
                _person.NIN = value;
                OnPropertyChanged("NIN");
                ValiderSaisie();
            }
        }
        public string FirstName
        {
            get { return _person.FirstName; }
            set
            {
                _person.FirstName = value;
                OnPropertyChanged("FirstName");
                ValiderSaisie();
            }
        }

        public string LastName
        {
            get { return _person.LastName; }
            set
            {
                _person.LastName = value;
                OnPropertyChanged("LastName");
                ValiderSaisie();
            }
        }
        public string Phone
        {
            get { return _person.Phone; }
            set
            {
                _person.Phone = value;
                OnPropertyChanged("Phone");
                ValiderSaisie();
            }
        }
        public CriminalRecordType CriminalRecordSelected
        {
            get { return _person.CriminalRecord; }
            set
            {
                _person.CriminalRecord = value;
                OnPropertyChanged("CriminalRecordSelected");
            }
        }
        public RelationType RelationSelected
        {
            get { return _relation.Relation; }
            set
            {
                _relation.Relation=value;
                OnPropertyChanged("RelationSelected");
            }
        }


        /* definition of the commands */
        public ICommand AddUserCommand { get; private set; }
        private void AddPerson()
        {
            Regex regex = new Regex(@"^\d{3}\d{3}\d{4}$");

            if (regex.IsMatch(Phone))
            {
                PersonController.AddPerson(new Person() { NIN = _person.NIN, FirstName = _person.FirstName, LastName = _person.LastName, Phone = _person.Phone, CriminalRecord = _person.CriminalRecord }); // To Database
                PersonController.UpdatePersonRelation(new Relationship() { Relation = _relation.Relation, SuspectId = _person.NIN }, VictimNIN);

            }
            else
            {
                MessageBox.Show("Le numéro de téléphone n'est pas valide. Veuillez entrer un numéro de téléphone valide sous la forme XXXYYYZZZZ.");
            }

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
        private bool _estValide;

        public bool EstValide
        {
            get { return _estValide; }
            set
            {
                _estValide = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EstValide)));
            }
        }
        private void ValiderSaisie()
        {
            EstValide = !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(Phone);
        }
    }
}
