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
    /// Interaction logic for ShowOneUpcomingReservationPage.xaml
    /// </summary>
    public partial class ShowOneUpcomingReservationPage : Page
    {
        public ShowOneUpcomingReservationPage(User loggedUser, NavigationService navigationService, AccommodationReservationService.AcccommodationReservationData reservationData)
        {
            InitializeComponent();
            var vm = new ShowOneUpcomingReservationVM(loggedUser, navigationService, reservationData);
            DataContext = vm;
        }
    }
}