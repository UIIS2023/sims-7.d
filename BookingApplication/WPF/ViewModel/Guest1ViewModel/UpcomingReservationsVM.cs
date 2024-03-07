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
using System.Windows.Input;
using static BookingApplication.Services.AccommodationReservationService;

namespace BookingApplication.WPF.ViewModel.GuestViewModel
{
    public class UpcomingReservationsVM : INotifyPropertyChanged
    {
        private User LoggedUser { get; set; }
        private NavigationService NavigationService { get; set; }

        public Commands CommandsHandlers { get; set; } = new Commands();

        public ObservableCollection<AcccommodationReservationData> UpcomingReservations { get; set; } = new ObservableCollection<AcccommodationReservationData>();
        public AccommodationReservationService reservationService = new AccommodationReservationService();

        public UpcomingReservationsVM(User user, NavigationService navigationService)
        {
            LoggedUser = user;
            NavigationService = navigationService;
            UpcomingReservations.Clear();
            reservationService.GetUpcomingReservationsForGuest(user).ForEach(x => UpcomingReservations.Add(x));
            CommandsHandlers.OnBack = new RelayCommand(OnBack);
            CommandsHandlers.OnReservationSelected = new RelayCommand<object>(OnReservationSelected);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public class Commands
        {
            public ICommand OnBack { get; set; }
            public ICommand OnReservationSelected { get; set; }
        }

        private void OnBack()
        {
            NavigationService.NavigateTo(null);
        }

        private void OnReservationSelected(object selResObj)
        {
            if (selResObj is AcccommodationReservationData reservationData)
            {
                NavigationService.NavigateTo(new ShowOneUpcomingReservationPage(LoggedUser, NavigationService, reservationData));
            }
        }
    }
}
