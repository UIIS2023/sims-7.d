using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuestView;
using CommunityToolkit.Mvvm.Input;
using Location = BookingApplication.Domain.Model.Location;

namespace BookingApplication.WPF.ViewModel.TourGuestViewModel
{
    public class TrackTourVm : INotifyPropertyChanged
    {
        public User User { get; set; }
        public NavigationService NavigationService { get; set; }
        public Tour Tour { get; set; }

        private readonly ImageService _imageService;
        public ObservableCollection<TourPoint> Checkpoints { get; set; }
        public ICommand NextImageCommand { get; set; }
        public ICommand PreviousImageCommand { get; set; }
        public Location TourLocation { get; set; }
        public TourReservation TourReservation { get; set; }

        private string _currentUrl;
        public string CurrentUrl
        {
            get => _currentUrl;
            set
            {
                if (_currentUrl != value)
                {
                    _currentUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        public TrackTourVm(User user, NavigationService navigationService, Tour tour, TourReservation tourReservation)
        {
            Tour = tour;
            User = user;
            TourReservation = tourReservation;
            NavigationService = navigationService;

            var locationService = new LocationService();
            var tourPointService = new TourPointService();
            _imageService = new ImageService();

            CurrentUrl = _imageService.GetImageUrl(_imageService.GetAll()[0].Id);
            Checkpoints = new ObservableCollection<TourPoint>(tourPointService.GetTourPointsByTour(tour));
            TourLocation = locationService.GetById(tour.LocationId);

            NextImageCommand = new RelayCommand(ChangeImageForward);
            PreviousImageCommand = new RelayCommand(ChangeImageBackward);

        }

        public void RespondToRequest()
        {
            if (TourReservation.IsChecked)
            {
                var result = MessageBox.Show("Are you present on this tour?", "Alert", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("You have accepted the request!");
                    var tourReservationService = new TourReservationService();
                    TourReservation.AcceptedRequest = true;
                    TourReservation.IsChecked = false;
                    tourReservationService.UpdateTourReservation(TourReservation);
                   
                }
                else if (result == MessageBoxResult.No)
                {
                    var tourReservationService = new TourReservationService();
                    TourReservation.TourPointId = -1;
                    TourReservation.IsChecked = false;
                    TourReservation.AcceptedRequest = false;
                    tourReservationService.UpdateTourReservation(TourReservation);
                }
            }
        }

        private void ChangeImageForward()
        {
            CurrentUrl = _imageService.ChangeImageForward();
        }

        private void ChangeImageBackward()
        {
            CurrentUrl = _imageService.ChangeImageBackward();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
