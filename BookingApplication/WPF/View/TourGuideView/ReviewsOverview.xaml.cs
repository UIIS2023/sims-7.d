using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuideViewModel;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for ReviewsOverview.xaml
    /// </summary>

    public partial class ReviewsOverview : Page
    {
        public ReviewsOverviewVM SelectedReviewsOverviewVm { get; set; }
        public ReviewsOverview(User user, NavigationService navigationService)
        {
            InitializeComponent();
            SelectedReviewsOverviewVm = new ReviewsOverviewVM(user, navigationService);   
            DataContext = SelectedReviewsOverviewVm;
        }


    }
}
