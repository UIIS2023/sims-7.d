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
using FormStatus = BookingApplication.WPF.ViewModel.TourGuideViewModel.TourFormVMParent.FormStatus;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourPointsOverviewPage.xaml
    /// </summary>
    
    public partial class TourPointsOverviewPage : Page
    {
        public static TourPointsOverviewVM tourPointsOverviewVM { get; set; }

        public TourPointsOverviewPage(User user, Tour tour, FormStatus formStatus, NavigationService navigationService)
        {
            InitializeComponent();
            tourPointsOverviewVM = new TourPointsOverviewVM(user, tour, formStatus, navigationService);
            DataContext = tourPointsOverviewVM;

        }
    }
}
