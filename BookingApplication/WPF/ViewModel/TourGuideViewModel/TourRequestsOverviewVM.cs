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
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuestView;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;
using FormStatus = BookingApplication.WPF.ViewModel.TourGuideViewModel.TourFormVMParent.FormStatus;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourRequestsOverviewVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly SimpleTourRequestService _tourRequestService;
        private readonly NavigationService _navigationService;
        private readonly LocationService _locationService;
        public List<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<SimpleTourRequest> TourRequests { get; set; }
        public User SelectedUser { get; set; }
        public SimpleTourRequest SelectedTourRequest { get; set; }
        public TourRequestFilterDTO SelectedTourRequestFilterDTO { get; set; }

        public ICommand FilterTourRequestsCommand { get; set; }
        public ICommand AcceptTourRequestCommand { get; set; }
        public ICommand ShowRecommendedTourFormCommand { get; set; }
        public ICommand ResetAllCommand { get; set; }
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

        public TourRequestsOverviewVM(User user, NavigationService navigationService)
        {
            SelectedUser = user;


            _navigationService = navigationService;
            _tourRequestService = new SimpleTourRequestService();
            _locationService = new LocationService();

            Cities = new ObservableCollection<string>();
            Countries = _locationService.GetLocations().Keys.ToList();
            TourRequests = new ObservableCollection<SimpleTourRequest>(_tourRequestService.GetWaitingTourRequests());
            foreach (var tourRequest in TourRequests)
            {
                tourRequest.Location = _locationService.GetById((int)tourRequest.LocationId);
            }
            SelectedTourRequest = new SimpleTourRequest();

            SelectedTourRequestFilterDTO = new TourRequestFilterDTO();
            SelectedTourRequestFilterDTO.TourLocation = new Location();
            SelectedTourRequestFilterDTO.StartDate = DateTime.Now;
            SelectedTourRequestFilterDTO.EndDate = DateTime.Now;

            FilterTourRequestsCommand = new RelayCommand(FilterTourRequests);
            ShowRecommendedTourFormCommand = new RelayCommand(ShowRecommendedTourForm);
            ResetAllCommand = new RelayCommand(ResetAll);
            CountryChangedCommand = new RelayCommand(CountryChanged);
                AcceptTourRequestCommand = new RelayCommand<SimpleTourRequest>(selectedTourRequest =>
               {
                   SelectedTourRequest = selectedTourRequest;
                   AcceptTourRequest();
               });  

        }

        private void ResetAll()
        {
            SelectedTourRequestFilterDTO.Language = "";
            SelectedTourRequestFilterDTO.TourLocation.Country = "";
            SelectedTourRequestFilterDTO.TourLocation.City = "";
            SelectedCountryIndex = -1;
            SelectedCityIndex = -1;
            IsEnabled = false;
            TourRequests.Clear();
            foreach (var tourRequest in _tourRequestService.GetAll())
            {
                TourRequests.Add(tourRequest);
            }

        }
        private void CountryChanged()
        {

            if (SelectedCountryIndex != -1)
            {
                IsEnabled = true;
                Cities.Clear();
                foreach (var city in _locationService.GetLocations()[SelectedTourRequestFilterDTO.TourLocation.Country])
                {
                    Cities.Add(city);
                }

            }
        }

        private void FilterTourRequests()
        {
            SelectedTourRequestFilterDTO.StartDate = new DateTime(SelectedTourRequestFilterDTO.StartDate.Year,
                SelectedTourRequestFilterDTO.StartDate.Day, SelectedTourRequestFilterDTO.StartDate.Month,
                0, 0, 0);

            SelectedTourRequestFilterDTO.EndDate = new DateTime(SelectedTourRequestFilterDTO.EndDate.Year,
                SelectedTourRequestFilterDTO.EndDate.Day, SelectedTourRequestFilterDTO.EndDate.Month,
                0, 0, 0);


            TourRequests.Clear();
            foreach (var tourRequest in _tourRequestService.FilterTourRequestsByDTO(SelectedTourRequestFilterDTO))
            {
                TourRequests.Add(tourRequest);
            }
        }


        private void AcceptTourRequest()
        {
            TourRequestDateForm tourRequestDateForm = new TourRequestDateForm(SelectedUser, _navigationService, SelectedTourRequest);
            tourRequestDateForm.ShowDialog();

//            FilterTourRequests(); 

        }
        private void ShowRecommendedTourForm()
        {
            TourRequestRecommendation tourRequestRecommendation = new TourRequestRecommendation(SelectedUser, _navigationService);
            tourRequestRecommendation.ShowDialog();
        }

    }
}
