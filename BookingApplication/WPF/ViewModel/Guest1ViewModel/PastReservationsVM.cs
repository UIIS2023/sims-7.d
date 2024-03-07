using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.View.AccommodationGuestView;
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
using static BookingApplication.Services.AccommodationReservationService;

namespace BookingApplication.WPF.ViewModel.GuestViewModel
{
    public class PastReservationsVM : INotifyPropertyChanged
    {
        private User LoggedUser { get; set; }
        private NavigationService NavigationService { get; set; }

        public ObservableCollection<AcccommodationReservationData> PastReservations { get; set; } = new ObservableCollection<AcccommodationReservationData>();

        private readonly AccommodationReservationService reservationService = new AccommodationReservationService();
        private readonly OwnerRateService ownerRateService = new OwnerRateService();
        public ICommand OnPastReservationSelected { get; set; }
        public ICommand OnBack { get; set; }

        public PastReservationsVM(User loggedUser, NavigationService navigationService)
        {
            LoggedUser = loggedUser;
            NavigationService = navigationService;

            var pastReservations = reservationService.GetPastReservationsForGuest(loggedUser);
            PastReservations.Clear();
            pastReservations.ForEach(x => PastReservations.Add(x));
            OnPastReservationSelected = new RelayCommand<object>(OnReservationSelected);
            OnBack = new RelayCommand(OnBackHandler);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OnReservationSelected(object pastReservation)
        {
            if (pastReservation is AcccommodationReservationData reservationData)
            {
                bool accommodationIsRateble = (DateTime.Now - reservationData.DateTo).Days <= 5;
                accommodationIsRateble &= (DateTime.Compare(reservationData.DateTo, DateTime.Now) <= 0);
                accommodationIsRateble &= !(ownerRateService.AccomodationIsRated(LoggedUser.Id, reservationData.Accommodation.Id));

                if (accommodationIsRateble)
                {
                    NavigationService.NavigateTo(new RateAccommodationPage(LoggedUser, NavigationService, reservationData));
                }
                else
                {
                    MessageBox.Show("You can not rate this accommodation.");
                }
            }
        }

        private void OnBackHandler()
        {
            NavigationService.NavigateTo(null);
        }
    }
}
