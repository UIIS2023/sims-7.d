using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.View.AccommodationGuestView;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApplication.WPF.ViewModel.GuestViewModel
{
    public class GuestMainVM
    {
        public User LoggedUser { get; set; }
        public NavigationService NavigationService { get; set; }

        public AccommodationReservationService.AcccommodationReservationData ReservationData { get; set; }
        public ICommand MakeReservationCommand { get; set; }
        public ICommand PreviousReservationsCommand { get; set; }
        public ICommand UpcomingReservationsCommand { get; set; }
        public ICommand ShowGuestRatings { get; set; }
        public ICommand ShowManageTools { get; set; }

        public GuestMainVM(User user, NavigationService navigationService,
           AccommodationReservationService.AcccommodationReservationData reservationData)
        {
            LoggedUser = user;
            NavigationService = navigationService;
            ReservationData = reservationData;

            MakeReservationCommand = new RelayCommand(ShowMakeReservations);
            PreviousReservationsCommand = new RelayCommand(ShowPreviousReservations);
            UpcomingReservationsCommand = new RelayCommand(ShowUpcomingReservations);
            ShowGuestRatings = new RelayCommand(ShowGuestRatingsHandler);
            ShowManageTools = new RelayCommand(ShowManageToolsHandler);
        }

        private void ShowMakeReservations()
        {
            NavigationService.NavigateTo(new MakeReservationPage(LoggedUser, NavigationService));
        }

        private void ShowPreviousReservations()
        {
            NavigationService.NavigateTo(new PastReservations(LoggedUser, NavigationService));
        }

        private void ShowUpcomingReservations()
        {
            NavigationService.NavigateTo(new UpcomingReservationPage(LoggedUser, NavigationService));
        }

        private void ShowGuestRatingsHandler()
        {
            NavigationService.NavigateTo(new ReceivedRatingsPage(LoggedUser, NavigationService));
        }

        private void ShowManageToolsHandler()
        {
            NavigationService.NavigateTo(
                new ShowOneUpcomingReservationPage(LoggedUser, NavigationService, ReservationData));
        }
    }
}

      
