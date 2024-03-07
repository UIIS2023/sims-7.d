using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuestViewModel;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for SimpleTourRequestPage.xaml
    /// </summary>
    public partial class SimpleTourRequestPage : Page
    {
        public SimpleTourRequestPage(User user, NavigationService navigationService)
        {
            InitializeComponent();
            SimpleTourRequestVm simpleTourRequestVm = new SimpleTourRequestVm(user, navigationService);
            DataContext = simpleTourRequestVm;
        }
    }
}
