using BookingApplication.Domain.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookingApplication.WPF.ViewModel.GuestViewModel;
using NavigationService = BookingApplication.Services.NavigationService;

namespace BookingApplication.WPF.View.AccommodationGuestView
{
    /// <summary>
    /// Interaction logic for ReceivedRatingsPage.xaml
    /// </summary>
    public partial class ReceivedRatingsPage : Page
    {
        public ReceivedRatingsPage(User loggerUser, NavigationService navigationService)
        {
            InitializeComponent(); 
            var vm = new ReceivedRatingsVM(loggerUser, navigationService);
            DataContext = vm;

        }
    }
}
