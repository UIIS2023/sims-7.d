using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.GuestViewModel;
using System.Windows;

namespace BookingApplication.WPF.View.AccommodationGuestView
{
    /// <summary>
    /// Interaction logic for AccommodationsOverallView.xaml
    /// </summary>
    public partial class AccommodationsOverallView : Window
    {
        public AccommodationsOverallView(User signedUser)
        {
            InitializeComponent();
            NavigationService navigationService = new NavigationService(GuestAcommodationsWindow);
              AccommodationReservationService.AcccommodationReservationData reservationData =
                new AccommodationReservationService.AcccommodationReservationData();
       
        GuestMainVM guestMainVM = new GuestMainVM(signedUser, navigationService, reservationData);
            DataContext = guestMainVM;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}