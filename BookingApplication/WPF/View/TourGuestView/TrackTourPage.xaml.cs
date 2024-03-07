using System.Windows;
using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuestViewModel;
namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for TrackTourPage.xaml
    /// </summary>
    public partial class TrackTourPage : Page
    {
        public TrackTourVm TrackTourVm { get; set; }
        public TrackTourPage(User user, NavigationService navigationService, Tour tour, TourReservation tourReservation)
        {
            InitializeComponent();
            TrackTourVm = new TrackTourVm(user, navigationService, tour, tourReservation);
            DataContext = TrackTourVm;
            Loaded += TrackTourPage_Loaded;
        }

        private void TrackTourPage_Loaded(object sender, RoutedEventArgs e)
        {
            TrackTourVm.RespondToRequest();
        }

    }
}
