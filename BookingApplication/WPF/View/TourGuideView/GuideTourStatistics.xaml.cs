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
using BookingApplication.WPF.ViewModel.TourGuideViewModel;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for GuideTourStatistics.xaml
    /// </summary>
    public partial class GuideTourStatistics : Window
    {
        public GuideTourStatistics(User user, Tour tour)
        {
            InitializeComponent();
            GuideTourStatisticsVM guideTourStatisticsVm = new GuideTourStatisticsVM(user, tour);
            DataContext = guideTourStatisticsVm;
            if (guideTourStatisticsVm.CloseAction == null)
                guideTourStatisticsVm.CloseAction = new Action(() => this.Close());
        }


    }
}
