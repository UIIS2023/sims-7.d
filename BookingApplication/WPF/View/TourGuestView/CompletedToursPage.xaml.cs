using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.WPF.ViewModel.TourGuestViewModel;
using NavigationService = BookingApplication.Services.NavigationService;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for CompletedToursPage.xaml
    /// </summary>
    public partial class CompletedToursPage : Page
    {
        public CompletedToursPage(User user, NavigationService navigationService)
        {
            InitializeComponent();
            var completedToursVm = new CompletedToursVm(user, navigationService);
            DataContext = completedToursVm;
        }
    }
}
