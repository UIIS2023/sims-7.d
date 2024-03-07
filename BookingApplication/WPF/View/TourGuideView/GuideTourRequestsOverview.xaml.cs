using BookingApplication.WPF.ViewModel.TourGuideViewModel;
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
using BookingApplication.Domain.Model;
using BookingApplication.Services;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for GuideTourRequestOverview.xaml
    /// </summary>
    public partial class GuideTourRequestOverview : Page
    {
        public GuideTourRequestOverview(User user, NavigationService navigationService)
        {
            InitializeComponent();
            TourRequestsOverviewVM tourRequestsOverviewVM = new TourRequestsOverviewVM(user, navigationService);
            DataContext = tourRequestsOverviewVM;
        }
    }
}
