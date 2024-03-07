using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class GuideTourReviewVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TourReview SelectedTourReview { get; set; }

        public ImageService _imageService;
        public TourReviewService _tourReviewService;
        public User SelectedUser { get; set; }
        public Image SelectedImage { get; set; }
        public int CurrentImageIndex { get; set; }
        public List<Image> TourReviewImages { get; set; }
        public TourPoint SelectedTourPoint { get; set; }
        public ICommand ChangeImageForwardCommand { get; set; }
        public ICommand ChangeImageBackwardCommand { get; set; }
        public ICommand ReportCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ObservableCollection<TourPoint> TourPoints { get; set; }
        public Action CloseAction { get; set; }

        public GuideTourReviewVM(User user, TourReview tourReview)
        {
            SelectedUser = user;
            SelectedTourReview = tourReview;
            _imageService = new ImageService();
            _tourReviewService = new TourReviewService();
            TourReviewImages = _imageService.GetImagesByImageIds(SelectedTourReview.ImageId);
            CurrentImageIndex = 0;
            SelectedImage = TourReviewImages[CurrentImageIndex];
            ChangeImageForwardCommand = new RelayCommand(ChangeImageForward);
            ChangeImageBackwardCommand = new RelayCommand(ChangeImageBackward);
            CloseCommand = new RelayCommand(Close);
            ReportCommand = new RelayCommand(Report);
            TourPoints = new ObservableCollection<TourPoint>(_tourReviewService.GetTourPointsByReview(SelectedTourReview));
        }

        public string GetImageForwardUrl()
        {
            if (TourReviewImages.Count > CurrentImageIndex + 1)
                return _imageService.GetImageUrl(TourReviewImages[++CurrentImageIndex].Id);
            CurrentImageIndex = 0;
            return _imageService.GetImageUrl(TourReviewImages[CurrentImageIndex].Id);
        }

        public string GetImageBackwardUrl()
        {
            if (CurrentImageIndex - 1 > -1)
                return _imageService.GetImageUrl(TourReviewImages[--CurrentImageIndex].Id);
            CurrentImageIndex = TourReviewImages.Count - 1;
            return _imageService.GetImageUrl(TourReviewImages[CurrentImageIndex].Id);
        }

        public void ChangeImageForward()
        {
            SelectedImage.Url = GetImageForwardUrl();
        }
        public void ChangeImageBackward()
        {
            SelectedImage.Url = GetImageBackwardUrl();
        }
        public void Close()
        {
            CloseAction();
        }

        public void Report()
        {
            ReportReview reportReview = new ReportReview(SelectedTourReview);
            reportReview.ShowDialog();
        }
    }
}
