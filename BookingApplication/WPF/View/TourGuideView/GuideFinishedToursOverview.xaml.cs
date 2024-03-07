using BookingApplication.Domain.Model;
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
using BookingApplication.WPF.ViewModel.TourGuideViewModel;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for GuideFinishedToursOverview.xaml
    /// </summary>
    public partial class GuideFinishedToursOverview : Page
    {
        public GuideFinishedTourOverviewVM SelectedGuideFinishedTourOverviewVm { get; set; }
        public GuideFinishedToursOverview(User user, NavigationService navigationService)
        {
            InitializeComponent();
            SelectedGuideFinishedTourOverviewVm = new GuideFinishedTourOverviewVM(user, navigationService);
            DataContext = SelectedGuideFinishedTourOverviewVm;
        }


    }
}
