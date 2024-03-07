using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;
using Image = BookingApplication.Domain.Model.Image;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourFormVMParent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Tour _selectedTour = new Tour();

        public Tour SelectedTour
        {
            get { return _selectedTour;}
            set
            {
                _selectedTour = value;
                    OnPropertyChanged("Selected Tour");
            }
        }
        public Location SelectedLocation { get; set; }
        public Image SelectedThumbnailImage { get; set; }
        public TourDate SelectedTourDate { get; set; }
        public static ObservableCollection<TourDate> TourDates { get; set; }

        protected TourService _tourService;
        protected LocationService _locationService;
        protected ImageService _imageService;
        protected TourPointService _tourPointService;
        protected TourDateService _tourDateService;
        protected TourReservationService _tourReservationService;
        protected VoucherService _voucherService;
        protected NavigationService _navigationService;
        public List<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public enum FormStatus { UPDATE, ADD, LIVE }
        public FormStatus SelectedStatus { get; set; }
        public User SelectedUser { get; set; }
        public List<TourReservation> TourReservations { get; set; }
        public ICommand SaveTourFormCommand { get; set; }
        public ICommand CancelFormCommand { get; set; }
        public ICommand StartTourDateCommand { get; set; }
        public ICommand CancelTourDateCommand { get; set; }
        public ICommand ShowTourPointsOverviewCommand { get; set; }
        public ICommand ShowImagesGalleryCommand { get; set; }
        public ICommand ShowTourDateOverviewCommand { get; set; }
        public ICommand ShowTourDateFormCommand { get; set; }
        public ICommand DisplayTourDateInfoCommand { get; set; }
        public ICommand DecreaseMaxGuestsCommand { get; set; }
        public ICommand IncreaseMaxGuestsCommand { get; set; }
        public ICommand DecreaseDurationCommand { get; set; }
        public ICommand IncreaseDurationCommand { get; set; }



        protected string _tourDateInfo;
        public string TourDateInfo
        {
            get => _tourDateInfo;
            set
            {
                if (_tourDateInfo != value)
                {
                    _tourDateInfo = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _selectedCountryIndex = -1;
        public int SelectedCountryIndex
        {
            get => _selectedCountryIndex;
            set
            {
                if (_selectedCountryIndex != value)
                {
                    _selectedCountryIndex = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _citiesIsEnabled;
        public bool CitiesIsEnabled
        {
            get => _citiesIsEnabled;
            set
            {
                if (_citiesIsEnabled != value)
                {
                    _citiesIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        protected void StatusConfiguration()
        {
            if (SelectedStatus == FormStatus.ADD)
            {
                TourDates = new ObservableCollection<TourDate>();
            }
            else if (SelectedStatus == FormStatus.UPDATE)
            {
                TourDates = new ObservableCollection<TourDate>(_tourDateService.GetTourDatesByTour(SelectedTour));
            }
            else if (SelectedStatus == FormStatus.LIVE)
            {
                TourDates = new ObservableCollection<TourDate>(_tourDateService.GetTourDatesByTour(SelectedTour));
            }
        }

        protected void LocationConfiguration(){
            if (SelectedTour.LocationId > 0)
            {
                InitializeSelectedLocation();
                DisplayCountryAndCity();
                CountryChanged();
            }
            else
            {
                SelectedLocation = new Location();
            }
        }


        protected void InitializeSelectedLocation()
        {
            //Location initialLocation = _locationService.GetById(SelectedTour.LocationId);
            //SelectedLocation = new Location(initialLocation.City, initialLocation.Country);
            SelectedLocation = _locationService.GetById(SelectedTour.LocationId);
        }

        //is to be changed once TourPointOverviewPage is implemented
        protected void ShowTourPointsOverview()
        {
            if (SelectedStatus == FormStatus.ADD || SelectedStatus == FormStatus.UPDATE)
            {
                _navigationService.NavigateTo(new TourPointsOverviewPage(SelectedUser, SelectedTour, SelectedStatus, _navigationService));
            }
            else
            {
                _navigationService.NavigateTo(new TourPointsOverviewPage(SelectedUser, SelectedTour, SelectedStatus, _navigationService));
            }
        }
        protected void ShowImagesOverview()
        {
            _navigationService.NavigateTo(new ImageGalleryPage(SelectedUser, SelectedTour, _navigationService, SelectedStatus));
        }

        protected void ShowTourDateOverview()
        {
            _navigationService.NavigateTo(new TourDateOverview(SelectedUser, SelectedTour, SelectedStatus, _navigationService));
        }

        protected void CountryChanged()
        {
            if (SelectedCountryIndex != -1)
            {
                CitiesIsEnabled = true;
                Cities.Clear();
                foreach (var city in _locationService.GetLocations()[SelectedLocation.Country])
                {
                    Cities.Add(city);
                }
            }
        }


        protected void DisplayCountryAndCity()
        {
            SelectedCountryIndex = Countries.IndexOf(SelectedLocation.Country);
            CitiesIsEnabled = true;
        }
        public void ShowCancelTourError()
        {
            var result = MessageBox.Show("You can't cancel a Tour 48 hours before it begins",
                "Note", MessageBoxButton.OK);
        }

        public string GetTourDateInfo(int reservationCount, int guestNumber)
        {
            string tourDateInfo = "";
            if (reservationCount != 0)
                if (reservationCount > 1)
                    tourDateInfo = "There are " + reservationCount.ToString() + " reservations, and" + guestNumber.ToString() +
                                   "guests, you can start a Tour!";
                else
                    tourDateInfo = "There is 1 reservation and" + guestNumber.ToString() +
                                   "guests, you can start a Tour!";
            else
                tourDateInfo = "There aren't any reservations yet.";
            return tourDateInfo;
        }
        protected void CancelTourDate()
        {
            if (_tourDateService.IsTourDateCancellationPossible(SelectedTourDate))
            {
                ShowCancelTourError();
                return;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel the tour?", "Cancel Tour",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _voucherService.AssignVouchersToGuestsByTourDates(SelectedTourDate, SelectedTour);
                    _tourReservationService.DeleteTourReservations(SelectedTour);
                    _tourDateService.DeleteTourDate((SelectedTourDate));
                    TourDates.Remove(SelectedTourDate);
                }
            }
        }
    }
}
