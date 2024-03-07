using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.GuestViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApplication.WPF.View.AccommodationGuestView
{
    /// <summary>
    /// Interaction logic for RateAccommodationPage.xaml
    /// </summary>
    public partial class RateAccommodationPage : Page
    {
        public RateAccommodationPage(User user, NavigationService navigationService, AccommodationReservationService.AcccommodationReservationData reservationData)
        {
            InitializeComponent();
            var vm = new RateAccommodationVM(user, navigationService, reservationData);
            DataContext = vm;
        }
    }
}