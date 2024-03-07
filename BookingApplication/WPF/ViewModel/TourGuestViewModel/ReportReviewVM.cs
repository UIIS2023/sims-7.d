using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using CommunityToolkit.Mvvm.Input;

namespace BookingApplication.WPF.ViewModel.TourGuestViewModel
{
    public class ReportReviewVM
    {
        private readonly  TourReviewService _tourReviewService;
        public TourReview SelectedTourReview { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand ReportCommand { get; set; }
        public Action CloseAction { get; set; }
        public ReportReviewVM(TourReview tourReview)
        {
            SelectedTourReview = tourReview;
            _tourReviewService = new TourReviewService();
            CloseCommand = new RelayCommand(Close);
            ReportCommand = new RelayCommand(Report);
        }
        private void Close()
        {
            CloseAction();
        }

        private void Report()
        {
            _tourReviewService.MarkTourReviewInvalid(SelectedTourReview);
            CloseAction();
        }
    }
}
