using System.Collections.ObjectModel;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuideViewModel;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for GuideTourPointOverview.xaml
    /// </summary>
    public partial class GuideTourPointOverview : Window
    {
        
        public static TourPointsOverviewVM tourPointsOverviewVM { get; set; }
        public GuideTourPointOverview(User user, Tour tour, TourFormVMParent.FormStatus formStatus, NavigationService navigationService)
        {
            InitializeComponent();
            tourPointsOverviewVM = new TourPointsOverviewVM(user, tour, formStatus, navigationService);
            DataContext = tourPointsOverviewVM;
        }
    }
}





