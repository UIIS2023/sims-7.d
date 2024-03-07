using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.WPF.ViewModel.TourGuestViewModel;
using NavigationService = BookingApplication.Services.NavigationService;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for VouchersPage.xaml
    /// </summary>
    public partial class VouchersPage : Page
    {
        public VouchersPage(User user, NavigationService navigationService)
        {
            InitializeComponent();
            var vouchersVm = new VouchersVm(user, navigationService);
            DataContext = vouchersVm;
        }
    }
}
