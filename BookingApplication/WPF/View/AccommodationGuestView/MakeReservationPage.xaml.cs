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
    /// Interaction logic for MakeReservationPage.xaml
    /// </summary>
    public partial class MakeReservationPage : Page
    {
        public MakeReservationPage(User loggedUser, NavigationService navigationService)
        {
            InitializeComponent();
            MakeReservationVM makeReservationVM = new MakeReservationVM(loggedUser, navigationService);
            DataContext = makeReservationVM;
        }

        private void CancelClick_Button(object sender, RoutedEventArgs e)
        {

        }
    }
}