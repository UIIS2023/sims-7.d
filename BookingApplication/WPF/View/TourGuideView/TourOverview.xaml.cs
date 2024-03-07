using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using BookingApplication.WPF.ViewModel;
using BookingApplication.WPF.ViewModel.TourGuideViewModel;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class TourOverview : Page
    {
        public TourOverview(User user, NavigationService navigationService)
        {
            InitializeComponent();
            GuideTourOverviewVM guideTourOverviewVM = new GuideTourOverviewVM(user, navigationService);
            DataContext = guideTourOverviewVM;
        }
        
    }
}
