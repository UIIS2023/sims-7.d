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
using BookingApplication.WPF.ViewModel.TourGuideViewModel;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for GuideTourStatisticsOverview.xaml
    /// </summary>
    public partial class GuideTourStatisticsOverview : Page
    {
        public GuideTourStatisticsOverview(User user, NavigationService navigationService)
        {
            InitializeComponent();
            GuideTourStatisticsOverviewVM guideTourStatisticsOverviewVm = new GuideTourStatisticsOverviewVM(user, navigationService);
            DataContext = guideTourStatisticsOverviewVm;
        }
        
    }
}
