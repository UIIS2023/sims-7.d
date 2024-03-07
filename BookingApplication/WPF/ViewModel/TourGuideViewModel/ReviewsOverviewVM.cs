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
using BookingApplication.DTO;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class ReviewsOverviewVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<TourReview> TourReviews { get; set; }
        public ObservableCollection<TourReviewDTO> TourReviewsDTO { get; set; }
        public TourReview SelectedTourReview { get; set; }
        public User SelectedUser { get; set; }
        public TourReviewDTO SelectedTourReviewDTO { get; set; }
        public TourReviewService _tourReviewService { get; set; }
        public TourService _tourService { get; set; }
        public UserRepository _userRepository { get; set; }

        private NavigationService _navigationService { get; set; }
        public ICommand ViewReviewCommand { get; set; }
        public ICommand ReturnCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public ReviewsOverviewVM(User selectedUser, NavigationService navigationService)
        {
            SelectedUser = selectedUser;
            _tourReviewService = new TourReviewService();
            _userRepository = new UserRepository();
            _navigationService = navigationService;
            TourReviews = new ObservableCollection<TourReview>(_tourReviewService.GetReviewsByGuide(SelectedUser));
            TourReviewsDTO = new ObservableCollection<TourReviewDTO>(_tourReviewService.AssignReviewsToDTO(TourReviews));
            ViewReviewCommand = new RelayCommand(ViewReview);
            ReturnCommand = new RelayCommand(Return);
            RefreshCommand = new RelayCommand(Refresh);
        }

        public void ViewReview()
        {
            if (SelectedTourReviewDTO != null)
            {
                SelectedTourReview = _tourReviewService.GetTourReviewByDTO(SelectedTourReviewDTO);
                GuideTourReview guideTourReview = new GuideTourReview(SelectedUser, SelectedTourReview);
                guideTourReview.Show();
            }
        }
        public void Refresh()
        {
            _navigationService.NavigateTo(new ReviewsOverview(SelectedUser, _navigationService));
        }

        public void Return()
        {
            _navigationService.NavigateTo(new GuideFinishedToursOverview(SelectedUser, _navigationService));
        }
    }
}
