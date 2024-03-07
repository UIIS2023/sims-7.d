using System;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using BookingApplication.WPF.View.TourGuestView;
using NavigationService = BookingApplication.Services.NavigationService;

namespace BookingApplication.WPF.ViewModel.TourGuestViewModel
{
    public class TourOverviewVm : INotifyPropertyChanged
    {
        private User User { get; set; }
        public Tour SelectedTour { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        public List<string> Countries { get; set; }
        public TourFilterDto TourFilterDto { get; set; }

        private readonly NavigationService _navigationService;
        
        private readonly LocationService _locationService;

        private readonly TourService _tourService;
        public ICommand ResetAllCommand { get; set; }
        public ICommand BookTourCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand CountryChangedCommand { get; set; }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
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


        private int _selectedCityIndex = -1;

        public int SelectedCityIndex
        {
            get => _selectedCityIndex;
            set
            {
                if (_selectedCityIndex != value)
                {
                    _selectedCityIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        public TourOverviewVm(User user, NavigationService navigationService, bool fromRequestPage)
        {
            User = user;
            if (fromRequestPage)
            {
                var requestService = new SimpleTourRequestService();
                Tours = requestService.GetToursWithMatchingLocationOrLanguage(User.Id);
                var tourService = new TourService();
                tourService.SetNewTours(Tours);
            }
            else
            {
                _tourService = new TourService();
                Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            }
                
            _navigationService = navigationService;
            IsEnabled = false;
            SelectedTour = new Tour();
            Cities = new ObservableCollection<string>();
            _locationService = new LocationService();
            Countries = _locationService.GetLocations().Keys.ToList();

            var languageService = new LanguageService();
            Languages = languageService.GetLanguages();

            TourFilterDto = new TourFilterDto
            {
                Location = new Location()
            };

            ResetAllCommand = new RelayCommand(ResetAll);
            FilterCommand = new RelayCommand(FilterTours);
            BookTourCommand = new RelayCommand<Tour>(selectedTour =>
            {
                SelectedTour = selectedTour;
                BookTour();
            });
            CountryChangedCommand = new RelayCommand(CountryChanged);
        }

        private void ResetAll()
        {
            TourFilterDto.Duration = null;
            TourFilterDto.GroupSize = null;
            TourFilterDto.Language = "";
            TourFilterDto.Location.Country = "";
            TourFilterDto.Location.City = "";
            SelectedCountryIndex = -1;
            SelectedCityIndex = -1;
            IsEnabled = false;
            Tours.Clear();
            foreach (var tour in _tourService.GetAll())
            {
                Tours.Add(tour);
            }

        }
        private void CountryChanged()
        {

            if (SelectedCountryIndex != -1)
            {
                IsEnabled = true;
                Cities.Clear();
                foreach (var city in _locationService.GetLocations()[TourFilterDto.Location.Country])
                {
                    Cities.Add(city);
                }

            }
        }


        private void FilterTours()
        {
            Tours.Clear();
            foreach (var tour in _tourService.FilterTours(TourFilterDto))
            {
                Tours.Add(tour);
            }
        }

        private void BookTour()
        {
            _navigationService.NavigateTo(new TourReservationPage(SelectedTour, User, _navigationService));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
