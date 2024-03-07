using BookingApplication.Domain.Model;
using BookingApplication.WPF.View.TourGuestView;
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
using BookingApplication.Services;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for GuideTourOverview.xaml
    /// </summary>
    public partial class GuideTourOverview : Window
    {
        public User User { get; set; }
        public NavigationService SelectedNavigationService { get; set; }
        public GuideTourOverview(User user)
        {
            InitializeComponent();
            User = user;
            SelectedNavigationService = new NavigationService(GuideToursWindow);
            GuideToursWindow.Content = new TourOverview(user, SelectedNavigationService);
        }

        private void FinishedToursClick(object sender, RoutedEventArgs e)
        {
            SelectedNavigationService.NavigateTo(new GuideFinishedToursOverview(User, SelectedNavigationService));
        }

        private void TourStatisticsOverviewClick(object sender, RoutedEventArgs e)
        {
            SelectedNavigationService.NavigateTo(new GuideTourStatisticsOverview(User, SelectedNavigationService));
        }

        private void UpcomingToursClick(object sender, RoutedEventArgs e)
        {
            SelectedNavigationService.NavigateTo(new TourOverview(User, SelectedNavigationService));
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TourRequestsClick(object sender, RoutedEventArgs e)
        {
            SelectedNavigationService.NavigateTo(new GuideTourRequestOverview(User, SelectedNavigationService));
        }
    }
}
