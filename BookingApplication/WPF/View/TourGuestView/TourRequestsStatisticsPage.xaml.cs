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
using BookingApplication.WPF.ViewModel.TourGuestViewModel;

namespace BookingApplication.WPF.View.TourGuestView
{
    /// <summary>
    /// Interaction logic for TourRequestsStatistics.xaml
    /// </summary>
    public partial class TourRequestsStatisticsPage : Page
    {
        public TourRequestsStatisticsPage(User user, NavigationService navigationService)
        {
            InitializeComponent();
            var tourRequestsStatisticsVm = new TourRequestsStatisticsVm(user, navigationService);
            DataContext = tourRequestsStatisticsVm;
        }
    }
}
