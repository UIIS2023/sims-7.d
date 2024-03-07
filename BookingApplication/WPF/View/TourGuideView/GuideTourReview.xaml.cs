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
    /// Interaction logic for GuideTourReview.xaml
    /// </summary>
    public partial class GuideTourReview : Window
    {
        public GuideTourReviewVM TourReviewVM { get; set; }

        public GuideTourReview(User user, TourReview tourReview)
        {
            InitializeComponent();
            TourReviewVM = new GuideTourReviewVM(user, tourReview);
            DataContext = TourReviewVM;
            if (TourReviewVM.CloseAction == null)
                TourReviewVM.CloseAction = new Action(() => this.Close());
        }
    }
}
