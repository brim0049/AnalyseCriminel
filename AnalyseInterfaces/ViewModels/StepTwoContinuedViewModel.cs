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
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AnalyseInterfaces.ViewModels
{
    public class StepTwoContinuedViewModel : INotifyPropertyChanged
    {
        //pass data
        private string _NIN;
        public string NIN
        {
            get { return _NIN; }
            set
            {
                _NIN = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NIN)));
            }
        }
        public StepTwoContinuedViewModel(string Nin, string CourantNIN)
        {
            _address = new Address();
            _call = new Call();
            _car = new Car();
            AddAddressCommand = new RelayCommand(AddAddress);
            AddCallCommand = new RelayCommand(AddCall);
            AddCarCommand = new RelayCommand(AddCar);
            NIN = CourantNIN;
            Date = DateTime.Now;

        }
        //Partie01
        private Address _address;

        public int? NoStreet
        {
            get { return _address.NoStreet; }
            set
            {
                if (_address.NoStreet != value)
                {
                    _address.NoStreet = value;
                    OnPropertyChanged("NoStreet");
                    ValiderSaisie();

                }
            }
        }

        public string NameStreet
        {
            get { return _address.NameStreet; }
            set
            {
                _address.NameStreet = value;
                OnPropertyChanged("NameStreet");
                ValiderSaisie();

            }
        }
        public string Ville
        {
            get { return _address.Ville; }
            set
            {
                _address.Ville = value;
                OnPropertyChanged("Ville");
                ValiderSaisie();

            }
        }
        public string Zip
        {
            get { return _address.Zip; }
            set
            {
                _address.Zip = value;
                OnPropertyChanged("Zip");
                ValiderSaisie();

            }
        }
        //Partie 02
        private Call _call;
        public string Number
        {
            get { return _call.Number; }
            set
            {
                _call.Number = value;
                OnPropertyChanged("Number");
            }
        }
        public DateTime Date
        {
            get { return _call.Date; }
            set
            {
                _call.Date = value;
                OnPropertyChanged("Date");
            }
        }
        public string Zone
        {
            get { return _call.Zone; }
            set
            {
                _call.Zone = value;
                OnPropertyChanged("Zone");
            }
        }
        public TimeSpan Duration
        {
            get { return _call.Duration; }
            set
            {
                _call.Duration = value;
                OnPropertyChanged("Duration");
            }
        }
        // Partie04
        private Car _car;
        public string Mark
        {
            get { return _car.Mark; }
            set
            {
                _car.Mark = value;
                OnPropertyChanged("Mark");
            }
        }
        public string Matricule
        {
            get { return _car.Matricule; }
            set
            {
                _car.Matricule = value;
                OnPropertyChanged("Matricule");
            }
        }

        /* definition of the commands */
        public ICommand AddAddressCommand { get; private set; }
        public ICommand AddCallCommand { get; private set; }
        public ICommand AddCarCommand { get; private set; }
        private void AddAddress()
        {
            PersonController.UpdatePersonAddress(new Address { NoStreet = _address.NoStreet, NameStreet = _address.NameStreet, Ville = _address.Ville, Zip=_address.Zip }, _NIN);
        }
        //Partie02
        private void AddCall()
        {
            Regex regex1 = new Regex(@"^\d{3}\d{3}\d{4}$");
            string input = Duration.ToString();
            Regex regex = new Regex(@"^([0-9]{2}):([0-5][0-9]):([0-5][0-9])$");
            Match match = regex.Match(input);

            if (!match.Success)
            {
                MessageBox.Show("La durée de temps doit être au format HH:mm:ss.", "Erreur lexicale");
                return;
            }

            try
            {
                if (regex1.IsMatch(Number))
                {
                    PersonController.UpdatePersonCall(new Call { Number = _call.Number, Date = _call.Date, Zone = _call.Zone, Duration = _call.Duration }, _NIN);

                }
                else
                {
                    MessageBox.Show("Le numéro de téléphone n'est pas valide. Veuillez entrer un numéro de téléphone valide sous la forme XXXYYYZZZZ." , "Erreur lexicale");
                }
                Duration = TimeSpan.ParseExact(input, @"hh\:mm\:ss", CultureInfo.InvariantCulture);

            }
            catch (Exception ex)
            {
                MessageBox.Show("La durée de temps doit être au format HH:mm:ss.", "Erreur lexicale");
                return;
            }
        }

        //Partie04

        private void AddCar()
        {
            PersonController.UpdatePersonCar(new Car { Mark = _car.Mark, Matricule = _car.Matricule }, _NIN);
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
            EstValide = !string.IsNullOrEmpty(NameStreet) && !string.IsNullOrEmpty(NoStreet.ToString()) && !string.IsNullOrEmpty(Ville) && !string.IsNullOrEmpty(Zip);
        }
    }
}

