using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;
using static BookingApplication.WPF.ViewModel.TourGuideViewModel.TourRequestRecommendationVM;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourRequestStatisticsVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private readonly LocationService _locationService;
        private readonly NavigationService _navigationService;
        private readonly SimpleTourRequestService _tourRequestService;
        public List<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<SimpleTourRequest> TourRequests { get; set; }
        public User SelectedUser { get; set; }
        public TourRequestFilterDTO SelectedTourRequestFilterDTO { get; set; }
        public List<string> Years { get; set; }
        public List<string> Months { get; set; }

        public ICommand ShowTourStatisticsOverviewCommand { get; set; }
        public ICommand ResetAllCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand ShowRecommendedTourFormCommand { get; set; }
        public ICommand CountryChangedCommand { get; set; }

        private int _requestNumber;
        public int RequestNumber
        {
            get => _requestNumber;
            set
            {
                if (_requestNumber != value)
                {
                    _requestNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _acceptedRequestNumber;
        public int AcceptedRequestNumber
        {
            get => _acceptedRequestNumber;
            set
            {
                if (_acceptedRequestNumber != value)
                {
                    _acceptedRequestNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _year;
        public string Year
        {
            get => _year;
            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _month;
        public string Month
        {
            get => _month;
            set
            {
                if (_month != value)
                {
                    _month = value;
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
        public TourRequestStatisticsVM(User user, NavigationService navigationService)
        {
            SelectedUser = user;
            _tourRequestService = new SimpleTourRequestService();
            _navigationService = navigationService;
            _locationService = new LocationService();


            Cities = new ObservableCollection<string>();
            Countries = _locationService.GetLocations().Keys.ToList();
            TourRequests = new ObservableCollection<SimpleTourRequest>(_tourRequestService.GetAll());
            SelectedTourRequestFilterDTO = new TourRequestFilterDTO();
            SelectedTourRequestFilterDTO.TourLocation = new Location();

            RequestNumber = 0;
            AcceptedRequestNumber = 0;

            Years = new List<string>();
            Years.Add("All time");
            Years.AddRange(_tourRequestService.GetYearsOfAllTourRequests().ConvertAll(y => y.ToString()));

            Months = new List<string>();
            Months.Add("None");
            Months.AddRange(new List<string>{"Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"});


            ShowTourStatisticsOverviewCommand = new RelayCommand(ShowTourStatisticsOverview);
            ResetAllCommand = new RelayCommand(ResetAll);
            FilterCommand = new RelayCommand(Filter);
            CountryChangedCommand = new RelayCommand(CountryChanged);
            ShowRecommendedTourFormCommand = new RelayCommand(ShowRecommendedTourForm);
        }

        public void Filter()
        {
            List<int> statistics = new List<int>();
            if (Equals(Year, "All time"))
            {
                statistics = _tourRequestService.GetTourRequestStatistics(SelectedTourRequestFilterDTO, 0, 0);
            }
            else
            {
                if (Equals(Month, "None"))
                {
                    statistics =
                        _tourRequestService.GetTourRequestStatistics(SelectedTourRequestFilterDTO,
                            Convert.ToInt32(Year), 0);
                }
                else
                {
                    statistics = _tourRequestService.GetTourRequestStatistics(SelectedTourRequestFilterDTO,
                        Convert.ToInt32(Year), DateTime.ParseExact(Month, "MMM", CultureInfo.InvariantCulture).Month);
                }
            }

            RequestNumber = statistics[0];
            AcceptedRequestNumber = statistics[1];

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
            RequestNumber = 0;
            AcceptedRequestNumber = 0;
//            Statistics.Clear();
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

        private void ShowTourStatisticsOverview()
        {
            _navigationService.NavigateTo(new GuideTourStatisticsOverview(SelectedUser, _navigationService));
        }

        private void ShowRecommendedTourForm()
        {
            TourRequestRecommendation tourRequestRecommendation = new TourRequestRecommendation(SelectedUser, _navigationService);
            tourRequestRecommendation.ShowDialog();
        }
    }
}
