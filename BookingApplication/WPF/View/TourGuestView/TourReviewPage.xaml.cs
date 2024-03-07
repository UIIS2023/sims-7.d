using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.WPF.ViewModel.TourGuestViewModel;
using NavigationService = BookingApplication.Services.NavigationService;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for TourReviewPage.xaml
    /// </summary>
    public partial class TourReviewPage : Page
    {
        public TourReviewPage(User user, NavigationService navigationService, Tour tour)
        {
            InitializeComponent();
            var tourReviewVm = new TourReviewVm(user, navigationService, tour);
            DataContext = tourReviewVm;
        }
    }
}
