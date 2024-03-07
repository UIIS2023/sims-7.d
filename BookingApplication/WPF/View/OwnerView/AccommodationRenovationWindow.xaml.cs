using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.OwnerViewModel;
using BookingApplication.WPF.ViewModel.TourGuestViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace BookingApplication.WPF.View.OwnerView
{
    /// <summary>
    /// Interaction logic for AccommodationRenovationWindow.xaml
    /// </summary>
    public partial class AccommodationRenovationWindow : Window
    {
        public AccommodationRenovationWindow(Accommodation selectedAccommodation)
        {
            InitializeComponent();
            var accommodationRenovationsWindowVm = new AccommodationRenovationWindowVM(selectedAccommodation);
            DataContext = accommodationRenovationsWindowVm;
        }

}
}
