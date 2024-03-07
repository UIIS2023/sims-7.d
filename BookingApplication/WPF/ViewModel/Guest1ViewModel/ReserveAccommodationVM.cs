using BookingApplication.Domain.Model;
using BookingApplication.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApplication.WPF.ViewModel.GuestViewModel
{
    public class ReserveAccommodationVM : INotifyPropertyChanged
    {
        public User LoggedUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public AccommodationReservationService reservationService { get; set; }
        public Accommodation SelectedAccomodation { get; set; }
        public ReserveCommands Commands { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string NofGuests { get; set; }

        public ObservableCollection<AccommodationReservationService.DateTimeRange> OtherAvailableDates { get; set; } = new ObservableCollection<AccommodationReservationService.DateTimeRange>();

        public Visibility OtherAvailableDatesVisibility { get; set; } = Visibility.Hidden;

        public ReserveAccommodationVM(User user, NavigationService navigationService, Accommodation selectedAccomodation)
        {
            LoggedUser = user;
            NavigationService = navigationService;
            reservationService = new AccommodationReservationService();
            SelectedAccomodation = selectedAccomodation;
            Commands = new ReserveCommands();
            Commands.OnCancel = new RelayCommand(OnCancel);
            Commands.OnCheckAvailability = new RelayCommand(OnCheckAvailability);
            Commands.OnReserve = new RelayCommand(OnReserve);
            Commands.OnOtherDateSelected = new RelayCommand<object>(OnOtherDateSelected);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void OnCancel()
        {
            NavigationService.NavigateTo(null);
        }

        private void OnCheckAvailability()
        {
            if (!CheckAllFieldsSelected())
            {
                return;
            }

            AccommodationReservationService.AcccommodationReservationData reservationData = new AccommodationReservationService.AcccommodationReservationData()
            {
                Accommodation = SelectedAccomodation,
                DateFrom = (DateTime)DateFrom,
                DateTo = (DateTime)DateTo,
                GuestsNumber = int.Parse(NofGuests)
            };

            if (reservationService.CheckAvailability(reservationData))
            {
                MessageBox.Show("Accommodation is available.");
                OtherAvailableDatesVisibility = Visibility.Hidden;
                OnPropertyChanged("OtherAvailableDatesVisibility");
            }
            else
            {
                MessageBox.Show("Accommodation is not available.");
                OtherAvailableDates.Clear();
                reservationService.FindOtherAvailableDates(reservationData).ForEach(x => OtherAvailableDates.Add(x));
                OtherAvailableDatesVisibility = Visibility.Visible;
                OnPropertyChanged("OtherAvailableDatesVisibility");
            }
        }

        public bool CheckAllFieldsSelected()
        {
            bool AllFieldsSelected =
                DateFrom.HasValue && DateTo.HasValue &&
                SelectedAccomodation != null && !string.IsNullOrEmpty(NofGuests) &&
                NofGuests.All(x => char.IsDigit(x)) && !string.IsNullOrWhiteSpace(NofGuests);

            if (!AllFieldsSelected)
            {
                MessageBox.Show("Please fill out required fields.");
            }

            return AllFieldsSelected;
        }

        private void OnReserve()
        {
            if (!CheckAllFieldsSelected())
            {
                return;
            }

            AccommodationReservationService.AcccommodationReservationData reservationData = new AccommodationReservationService.AcccommodationReservationData()
            {
                Accommodation = SelectedAccomodation,
                DateFrom = (DateTime)DateFrom,
                DateTo = (DateTime)DateTo,
                GuestsNumber = int.Parse(NofGuests)
            };

            if (!reservationService.CheckAvailability(reservationData))
            {
                MessageBox.Show("Accommodation is not available.");
                return;
            }

            AccommodationReservation reservation = new AccommodationReservation()
            {
                AccommodationId = SelectedAccomodation.Id,
                StartDate = (DateTime)DateFrom,
                EndDate = (DateTime)DateTo,
                GuestId = LoggedUser.Id,
                GuestLimit = SelectedAccomodation.MaxGuests
            };

            reservationService.Reserve(reservation, LoggedUser);
            MessageBox.Show("Succesfully reserved an accommodation.");

            NavigationService.NavigateTo(null);
        }

        private void OnOtherDateSelected(object otherDateRange)
        {
            if (otherDateRange is AccommodationReservationService.DateTimeRange selectedRange)
            {
                DateFrom = selectedRange.StartDate;
                OnPropertyChanged("DateFrom");
                DateTo = selectedRange.EndDate;
                OnPropertyChanged("DateTo");
            }
        }

        public class ReserveCommands
        {
            public ICommand OnReserve { get; set; }
            public ICommand OnCheckAvailability { get; set; }
            public ICommand OnCancel { get; set; }
            public ICommand OnOtherDateSelected { get; set; }
        }
    }
}
