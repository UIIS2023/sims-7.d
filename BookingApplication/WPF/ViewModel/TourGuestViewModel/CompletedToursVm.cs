using System.Collections.ObjectModel;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuestView;
using CommunityToolkit.Mvvm.Input;

namespace BookingApplication.WPF.ViewModel.TourGuestViewModel
{
    public class CompletedToursVm
    {
        public ObservableCollection<Tour> CompletedTours { get; set; }

        private readonly NavigationService _navigationService;
        public Tour SelectedTour { get; set; }
        private User User { get; set; }
        public ICommand ReviewCommand { get; set; }
        public CompletedToursVm(User user, NavigationService navigationService)
        {
            User = user;
            _navigationService = navigationService;

            var tourReservationService = new TourReservationService();
            CompletedTours = new ObservableCollection<Tour>(tourReservationService.GetCompletedToursForUser(user.Id));

            ReviewCommand = new RelayCommand<Tour>(selectedTour =>
            {
                SelectedTour = selectedTour;
                GoToReviewPage();
            });
        }

        private void GoToReviewPage()
        {
            _navigationService.NavigateTo(new TourReviewPage(User, _navigationService, SelectedTour));
        }
    }

}
