using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuestViewModel;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for ToursOverview.xaml
    /// </summary>
    public partial class ToursOverviewPage : Page
    {
        public ToursOverviewPage(User user, NavigationService navigationService, bool fromRequestsPage)
        {
            InitializeComponent();
            var tourOverviewVm = new TourOverviewVm(user, navigationService, fromRequestsPage);
            DataContext = tourOverviewVm;
        }
    }
}
