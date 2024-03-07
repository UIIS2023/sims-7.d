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
using BookingApplication.WPF.ViewModel.TourGuestViewModel;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for ReportReview.xaml
    /// </summary>
    public partial class ReportReview : Window
    {
        public ReportReview(TourReview tourReview)
        {
            InitializeComponent();
            ReportReviewVM reportReviewVM = new ReportReviewVM(tourReview);
            DataContext = reportReviewVM;
            if (reportReviewVM.CloseAction == null)
            {
                reportReviewVM.CloseAction = new Action(() => this.Close());
            }
        }
    }
}
