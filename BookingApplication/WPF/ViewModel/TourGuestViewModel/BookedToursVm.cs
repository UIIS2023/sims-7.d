using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuestView;
using CommunityToolkit.Mvvm.Input;

namespace BookingApplication.WPF.ViewModel.TourGuestViewModel
{
    public class BookedToursVm
    {
        public User User { get; set; }
        public NavigationService NavigationService { get; set; }

        private readonly TourReservationService _tourReservationService;
        public Tour SelectedTour { get; set; }
        public ObservableCollection<Tour> BookedTours { get; set; }
        public ObservableCollection<TourReservation> TourReservations { get; set; }
        public ICommand TrackCommand { get; set; }
        public BookedToursVm(User user, NavigationService navigationService)
        {
            User = user;
            NavigationService = navigationService;

            _tourReservationService = new TourReservationService();
            TourReservations = new ObservableCollection<TourReservation>(_tourReservationService.GetReservationsForUser(user.Id));
            BookedTours = new ObservableCollection<Tour>(_tourReservationService.GetBookedToursFromReservations(TourReservations));

            var tourService = new TourService();
            tourService.ActivateTours(TourReservations, BookedTours);

            TrackCommand = new RelayCommand<Tour>(selectedTour =>
            {
                SelectedTour = selectedTour;
                Track();
            });
        }

        private void Track()
        {
            var tourReservation = _tourReservationService.GetReservationForUserAndTour(SelectedTour, User.Id);
            NavigationService.NavigateTo(new TrackTourPage(User, NavigationService, SelectedTour, tourReservation));
        }
    }
}
