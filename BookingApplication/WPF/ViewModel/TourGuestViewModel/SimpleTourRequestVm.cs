using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuestView;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;
using FluentScheduler;

namespace BookingApplication.WPF.ViewModel.TourGuestViewModel
{
    public class SimpleTourRequestVm : INotifyPropertyChanged
    {
        public User User { get; set; }
        public SimpleTourRequest SimpleTourRequest { get; set; }
        public NavigationService NavigationService { get; set; }

        public ObservableCollection<SimpleTourRequest> Requests { get; set; }
        public ICommand SaveCommand { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public List<string> Countries { get; set; }
        public ICommand CountryChangedCommand { get; set; }
        public ICommand CityChangedCommand { get; set; }
        public ICommand ShowStatisticsCommand { get; set; }
        public ICommand ShowNewToursCommand { get; set; }
        public ICommand HideNotificationCommand { get; set; }
        public ICommand ShowAcceptedRequestsCommand { get; set; }
        public ICommand HideAcceptedRequestsCommand { get; set; }

        private bool _canSave = false;
        public Location Location { get; set; }

        public bool CanSave
        {
            get => _canSave;
            set
            {
                _canSave = value;
                OnPropertyChanged();
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

        private int _selectedLanguageIndex = -1;

        public int SelectedLanguageIndex
        {
            get => _selectedLanguageIndex;
            set
            {
                if (_selectedLanguageIndex != value)
                {
                    _selectedLanguageIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isNotificationVisible;

        public bool IsNotificationVisible
        {
            get => _isNotificationVisible;
            set
            {
                if (_isNotificationVisible != value)
                {
                    _isNotificationVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isAcceptedNotificationVisible;

        public bool IsAcceptedNotificationVisible
        {
            get => _isAcceptedNotificationVisible;
            set
            {
                if (_isAcceptedNotificationVisible != value)
                {
                    _isAcceptedNotificationVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public SimpleTourRequestVm(User user, NavigationService navigationService)
        {
            User = user;
            NavigationService = navigationService;
            SimpleTourRequest = new SimpleTourRequest();
            Location = new Location();
            SimpleTourRequest.UserId = User.Id;
            var requestService = new SimpleTourRequestService();
            Requests = new ObservableCollection<SimpleTourRequest>(requestService.GetAllRequestsByUser(user.Id));
            var languageService = new LanguageService();
            Languages = languageService.GetLanguages();
            Cities = new ObservableCollection<string>();
            var locationService = new LocationService();
            Countries = locationService.GetLocations().Keys.ToList();

            SaveCommand = new RelayCommand(Save);
            CountryChangedCommand = new RelayCommand(CountryChanged);
            CityChangedCommand = new RelayCommand(CityChanged);
            ShowStatisticsCommand = new RelayCommand(ShowStatistics);
            ShowNewToursCommand = new RelayCommand(ShowNewTours);
            HideNotificationCommand = new RelayCommand(HideNotification);
            ShowAcceptedRequestsCommand = new RelayCommand(ShowAcceptedRequests);
            HideAcceptedRequestsCommand = new RelayCommand(HideAcceptedRequestsNotification);

            JobManager.AddJob(CheckForNewTours, s => s.ToRunEvery(1).Seconds());
            JobManager.AddJob(CheckForAcceptedRequests, s => s.ToRunEvery(1).Seconds());
            JobManager.AddJob(CheckCanSave, s => s.ToRunEvery(1).Milliseconds());
            JobManager.AddJob(UpdateRequestsStatus, s=> s.ToRunEvery(1).Days());
        }
        private void CheckForNewTours()
        {
            var requestService = new SimpleTourRequestService();
            IsNotificationVisible = requestService.GetToursWithMatchingLocationOrLanguage(User.Id).Count > 0;
        }
        private void CheckForAcceptedRequests()
        {
            var requestService = new SimpleTourRequestService();
            IsAcceptedNotificationVisible = requestService.GetUnnotifiedAcceptedRequests(User.Id).Count > 0;
        }
        private void ShowAcceptedRequests()
        {
            var requestService = new SimpleTourRequestService();
            requestService.SetRequestsAsNotified(User.Id);
            NavigationService.NavigateTo(new AcceptedRequestsPage(User, NavigationService));
        }
        private void HideAcceptedRequestsNotification()
        {
            IsAcceptedNotificationVisible = false;
            var requestService = new SimpleTourRequestService();
            requestService.SetRequestsAsNotified(User.Id);
        }
        private void HideNotification()
        {
            IsNotificationVisible = false;
            var tourService = new TourService();
            tourService.SetNewTours(tourService.GetNewTours());
        }
        private void ShowNewTours()
        {
            NavigationService.NavigateTo(new ToursOverviewPage(User, NavigationService, true));
        }
        private void CountryChanged()
        {
            if (SelectedCountryIndex == -1) return;
            IsEnabled = true;
            Cities.Clear();
            var locationService = new LocationService();
            foreach (var city in locationService.GetLocations()[Location.Country])
            {
                Cities.Add(city);
            }
        }
        private void ShowStatistics()
        {
            NavigationService.NavigateTo(new TourRequestsStatisticsPage(User, NavigationService));
        }
        private void UpdateRequestsStatus()
        {
            var simpleRequestService = new SimpleTourRequestService();
            simpleRequestService.InvalidateRequests(Requests);
        }
        private void CityChanged()
        {
            if (SelectedCityIndex == -1) return;
            var locationService = new LocationService();
            SimpleTourRequest.LocationId = locationService.GetIdByCityAndCountry(Location.City, Location.Country);

        }
        private void CheckCanSave()
        {
            CanSave = SimpleTourRequest.IsRequestFilled();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void ClearForm()
        {
            SelectedCityIndex = -1;
            SelectedCountryIndex = -1;
            SimpleTourRequest.StartDate = null;
            SimpleTourRequest.EndDate = null;
            SimpleTourRequest.Language = "";
            SimpleTourRequest.LocationId = 0;
            SimpleTourRequest.GuestsNumber = null;
            SimpleTourRequest.Description = "";
            SelectedLanguageIndex = -1;
        }
        private void Save()
        {
            var simpleTourRequestService = new SimpleTourRequestService();
            simpleTourRequestService.Save(SimpleTourRequest);
            Requests.Clear();
            foreach (var request in simpleTourRequestService.GetAll())
            {
                Requests.Add(request);
            }
            ClearForm();
        }
    }
}
