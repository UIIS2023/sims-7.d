using BookingApplication.Domain.Model;
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
using BookingApplication.Services;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourFormLivePage.xaml
    /// </summary>
    public partial class TourFormLivePage : Page
    {
        public TourFormLivePage(User user, Tour tour, TourFormLiveVM.FormStatus formStatus, NavigationService navigationService)
        {
            InitializeComponent();
            var tourFormPageLiveVM = new TourFormLiveVM(user, tour, formStatus, navigationService);
            DataContext = tourFormPageLiveVM;   
        }

    }
}
