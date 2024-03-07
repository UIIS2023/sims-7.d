using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuestViewModel;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for AcceptedRequestsView.xaml
    /// </summary>
    public partial class AcceptedRequestsPage : Page
    {
        public AcceptedRequestsPage(User user, NavigationService navigationService)
        {
            InitializeComponent();
            AcceptedRequestsVm acceptedRequestsVm = new AcceptedRequestsVm(user, navigationService);
            DataContext = acceptedRequestsVm;
        }
    }
}
