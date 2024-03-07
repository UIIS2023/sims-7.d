using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuestView;
using CommunityToolkit.Mvvm.Input;

namespace BookingApplication.WPF.ViewModel.TourGuestViewModel
{
    public class TourReviewVm : INotifyPropertyChanged
    {

        private int _guidesKnowledge = 1;

        public int GuidesKnowledge
        {
            get => _guidesKnowledge;
            set
            {
                if (_guidesKnowledge != value)
                {
                    _guidesKnowledge = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _guidesLanguage = 1;

        public int GuidesLanguage
        {
            get => _guidesLanguage;
            set
            {
                if (_guidesLanguage != value)
                {
                    _guidesLanguage = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _tourQuality = 1;

        public int TourQuality
        {
            get => _tourQuality;
            set
            {
                if (_tourQuality != value)
                {
                    _tourQuality = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _imageUrl;

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Comment { get; set; }

        public ICommand UploadCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public User User { get; set; }
        public List<int> ImageIds { get; set; }
        public Tour Tour { get; set; }
        public NavigationService NavigationService { get; set; }
        private ImageService _imageService;

        public TourReviewVm(User user, NavigationService navigationService, Tour tour)
        {
            _imageService = new ImageService();
            ImageIds = new List<int>();

            User = user;
            Tour = tour;
            NavigationService = navigationService;

            UploadCommand = new RelayCommand(Upload);
            CancelCommand = new RelayCommand(Cancel);
            SubmitCommand = new RelayCommand(Submit);
        }

        private void Upload()
        {

            var image = new Image(ImageUrl.Trim());
            ImageIds.Add(_imageService.Save(image).Id);
            ImageUrl = "";
        }

        private void Cancel()
        {
            NavigationService.NavigateTo(new CompletedToursPage(User, NavigationService));
        }

        private void Submit()
        {
            var overallRating = (GuidesKnowledge + GuidesLanguage + TourQuality) / 3;
            var review = new TourReview(overallRating, GuidesKnowledge, GuidesLanguage, TourQuality, Comment, ImageIds, Tour.Id, -1, true, User.Id);

            var tourReviewService = new TourReviewService();
            tourReviewService.SaveTourReview(review);

            NavigationService.NavigateTo(new CompletedToursPage(User, NavigationService));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
