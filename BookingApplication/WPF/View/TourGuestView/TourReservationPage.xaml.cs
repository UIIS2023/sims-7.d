using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuestViewModel;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservationPage : Page
    {
        public TourReservationPage(Tour selectedTour, User user, NavigationService navigationService)
        {
            InitializeComponent();
            var tourReservationVm = new TourReservationVm(selectedTour, user, navigationService);
            DataContext = tourReservationVm;
        }
    }
}
