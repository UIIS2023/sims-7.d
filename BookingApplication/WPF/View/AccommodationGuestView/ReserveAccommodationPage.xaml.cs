using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.GuestViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DateTimeFormat = BookingApplication.Domain.Model.DateTimeFormat;

namespace BookingApplication.WPF.View.AccommodationGuestView
{
    /// <summary>
    /// Interaction logic for ReserveAccommodationPage.xaml
    /// </summary>
    public partial class ReserveAccommodationPage : Page
    {
        public ReserveAccommodationPage(User loggedUser, NavigationService navigationService, Accommodation selectedAccomodation)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = DateTimeFormat.Format.Split(' ')[0];
            Thread.CurrentThread.CurrentCulture = ci;

            InitializeComponent();
            ReserveAccommodationVM reserveAccommodationVM = new ReserveAccommodationVM(loggedUser, navigationService, selectedAccomodation);
            DataContext = reserveAccommodationVM;
        }
    }
}