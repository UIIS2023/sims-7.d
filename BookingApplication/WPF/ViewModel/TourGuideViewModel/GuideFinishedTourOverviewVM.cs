using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApplication.WPF.View.TourGuideView;
using NavigationService = BookingApplication.Services.NavigationService;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class GuideFinishedTourOverviewVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }

        private readonly TourService _tourService;
        private readonly NavigationService _navigationService;

        public ICommand ViewTourFormCommand { get; set; }
        public ICommand ViewTourReviewsCommand { get; set; }
        public User SelectedUser { get; set; }

        public GuideFinishedTourOverviewVM(User user, NavigationService navigationService)
        {
            SelectedUser = user;
            _navigationService = navigationService;
            _tourService = new TourService();
            Tours = new ObservableCollection<Tour>(_tourService.GetFinishedToursByReservations());
            ViewTourFormCommand = new RelayCommand(ViewTourForm);
            ViewTourReviewsCommand = new RelayCommand(ViewTourReviews);
        }

        public void ViewTourForm()
        {
            if (SelectedTour != null)
            {
                TourForm tourForm = new TourForm(SelectedTour, TourForm.FormStatus.LIVE);

            }
        }

        public void ViewTourReviews()
        {
            _navigationService.NavigateTo(new ReviewsOverview(SelectedUser, _navigationService));
        }

    }
}
