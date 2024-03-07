using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuideViewModel;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourPointMarkGuests.xaml
    /// </summary>
    public partial class TourPointMarkGuests : Window
    {

        public TourPointMarkGuests(TourPoint tourPoint)
        {
            InitializeComponent();
            Title = "Mark Guests";
            TourPointMarkGuestsOverviewVM tourPointMarkGuestsOverviewVM = new TourPointMarkGuestsOverviewVM(tourPoint);
            DataContext = tourPointMarkGuestsOverviewVM;
            if (tourPointMarkGuestsOverviewVM.CloseAction == null)
            {
                tourPointMarkGuestsOverviewVM.CloseAction = new Action(() => this.Close());
            }
        }
        
    }
}
