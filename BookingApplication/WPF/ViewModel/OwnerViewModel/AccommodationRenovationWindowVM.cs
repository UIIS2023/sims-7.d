using BookingApplication.Domain.Model;
using BookingApplication.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingApplication.WPF.View.OwnerView;
using BookingApplication.WPF.ViewModel.OwnerViewModel;
using BookingApplication.WPF.View.OwnerView;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApplication.WPF.ViewModel.OwnerViewModel
{
    public class AccommodationRenovationWindowVM:INotifyPropertyChanged

    {
        private readonly AccommodationRenovationService _accommodationRenovationService;
        private readonly AccommodationReservationService _accommodationReservationService;
        public AccommodationRenovation NewAccommodationRenovation { get; set; }

        public Accommodation SelectedAccommodation { get; set; }

        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public ObservableCollection<Tuple<DateTime, DateTime>> AvailableDatesCombo { get; set; }
        public Tuple<DateTime, DateTime> SelectedDates { get; set; }

        public ICommand CheckAvailabilityCommand { get; set; }
        public ICommand RenovateCommand { get; set; }
       
        public ICommand CloseCommand { get; set; }
        public string DaysForRenovation { get; set; }
        public string AvailabilityTextShow { get; set; }
                    
        public string Description { get; set; }
        public DateTime StartDatePick { get; set; }
        public DateTime EndDatePick { get; set; }




        public AccommodationRenovationWindowVM(Accommodation selectedAccommodation)
        {
            //User = user;
            // NavigationService = navigationService;

            _accommodationRenovationService = new AccommodationRenovationService();
            _accommodationReservationService = new AccommodationReservationService();
            NewAccommodationRenovation = new AccommodationRenovation();
            Reservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationService.GetAll());
            SelectedAccommodation = selectedAccommodation;
            

            AvailableDatesCombo = new ObservableCollection<Tuple<DateTime, DateTime>>();

            StartDatePick = DateTime.Today;
            EndDatePick = DateTime.Today;

            CheckAvailabilityCommand = new RelayCommand(CheckAvailability);
            RenovateCommand = new RelayCommand(Renovate);
            CloseCommand = new RelayCommand(Close);
        }

        private void CheckAvailability()
        {
            DateTime startDate = StartDatePick;
            DateTime endDate = EndDatePick;
            int daysForRenovation = int.Parse(DaysForRenovation);
            List<Tuple<DateTime, DateTime>> AvailableDateRange = new List<Tuple<DateTime, DateTime>>();


            if (startDate > endDate)
            {
                if (AvailableDatesCombo != null)
                {
                    AvailableDatesCombo.Clear();
                }
                MessageBox.Show("Start date must be lower than end!");
                return;
            }


            AvailableDatesCombo.Clear();
            //AvailabilityTextShow ="";
            AvailableDateRange = _accommodationRenovationService.GetAvailableDates(SelectedAccommodation, daysForRenovation, startDate, endDate);

            foreach (var date in AvailableDateRange)
            {
                AvailableDatesCombo.Add(date);
            }

            if (AvailableDateRange.Count == 0)
            {
               // AvailabilityTextShow = "Change range, accommodation is reserved in this range!";
                MessageBox.Show("Change range, accommodation is reserved in this range!!");
                AvailableDatesCombo.Clear();

            }
        }


        private void Renovate()
        {
            if (SelectedDates != null)
            {
                NewAccommodationRenovation.NumberOfDays = int.Parse(DaysForRenovation);
                NewAccommodationRenovation.StartDate = SelectedDates.Item1;
                NewAccommodationRenovation.EndDate = SelectedDates.Item2;
                NewAccommodationRenovation.Description = Description;
                AccommodationRenovation renovation = new AccommodationRenovation(SelectedAccommodation.Id, NewAccommodationRenovation.NumberOfDays, NewAccommodationRenovation.StartDate, NewAccommodationRenovation.EndDate, NewAccommodationRenovation.Description);
                _accommodationRenovationService.SaveAccommodationRenovation(renovation);
                MessageBox.Show("Renovation saved.");

                //Close();
            }
            else
            {
                MessageBox.Show("Select dates for your renovation.");
            }
        }


        private void Close()
        {
           // Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
