using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Services;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for GuestTourOverview.xaml
    /// </summary>
    public partial class GuestTourOverview : Window
    {
        public User User { get; set; }
        public NavigationService NavigationService { get; set; }
        public GuestTourOverview(User user)
        {
            InitializeComponent();
            User = user;
            NavigationService = new NavigationService(ToursWindow);
            ToursWindow.Content = new ToursOverviewPage(user, NavigationService, false);
            DataContext = this;
        }

        private void OverviewButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new ToursOverviewPage(User, NavigationService, false));
        }

        private void VouchersButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new VouchersPage(User, NavigationService));
        }

        private void CompletedButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new CompletedToursPage(User, NavigationService));
        }

        private void BookedButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new BookedToursPage(User, NavigationService));
        }

        
        private void LogoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            Window loginWindow = new SignInForm();
            loginWindow.Show();
            Close();
        }

        private void ComplexRequestButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void RequestButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new SimpleTourRequestPage(User, NavigationService));
        }
    }
}
