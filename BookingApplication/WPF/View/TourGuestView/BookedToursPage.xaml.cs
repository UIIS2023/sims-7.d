using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.WPF.ViewModel.TourGuestViewModel;
using NavigationService = BookingApplication.Services.NavigationService;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for BookedToursPage.xaml
    /// </summary>
    public partial class BookedToursPage : Page
    {
        public BookedToursPage(User user, NavigationService navigationService)
        {
            InitializeComponent();
            var bookedToursVm = new BookedToursVm(user, navigationService);
            DataContext = bookedToursVm;
        }
    }
}
