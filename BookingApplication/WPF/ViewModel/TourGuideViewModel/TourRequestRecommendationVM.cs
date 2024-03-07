using System;
using System.Collections.Generic;
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
    public class TourRequestRecommendationVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private readonly NavigationService _navigationService;
        private readonly SimpleTourRequestService _tourRequestService;
        private readonly LocationService _locationService;
        public User SelectedUser { get; set; }
        public Tour SelectedTour { get; set; }
        public SimpleTourRequest SelectedTourRequest { get; set; }
        public TourRequestFilterDTO SelectedTourRequestFilterDTO { get; set; }
        public ICommand RecommendTourCommand { get; set; }
        public ICommand CreateRecommendedTourCommand { get; set; }
        public ICommand CancelTourRecommendationCommand { get; set; }
        public Action CloseAction { get; set; }

        private string _recommendationInfo;
        public string RecommendationInfo
        {
            get => _recommendationInfo;
            set
            {
                if (_recommendationInfo != value)
                {
                    _recommendationInfo = value;
                    OnPropertyChanged();
                }
            }
        }
        public enum RecommendationStatus { NOTHING, LANGUAGE, LOCATION }
        private RecommendationStatus _selectedStatus;
        public RecommendationStatus SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _selectedRecommendationFactorIndex;
        public int SelectedRecommendationFactorIndex
        {
            get => _selectedRecommendationFactorIndex;
            set
            {
                if (_selectedRecommendationFactorIndex != value)
                {
                    _selectedRecommendationFactorIndex = value;
                    OnPropertyChanged();
                }
            }
        }
        public TourRequestRecommendationVM(User user, NavigationService navigationService)
        {
            SelectedUser = user;
            SelectedTour = new Tour();
            SelectedTourRequest = new SimpleTourRequest();
            SelectedTourRequestFilterDTO = new TourRequestFilterDTO();

            SelectedRecommendationFactorIndex = -1;
            RecommendationInfo = "";

            SelectedStatus = RecommendationStatus.NOTHING;
            _navigationService = navigationService;
            _tourRequestService = new SimpleTourRequestService();
            _locationService = new LocationService();

            RecommendTourCommand = new RelayCommand(RecommendTour);
            CancelTourRecommendationCommand = new RelayCommand(CancelTourRecommendation);
            CreateRecommendedTourCommand = new RelayCommand(CreateRecommendedTour);
        }

        public void RecommendTour()
        {
            if (SelectedRecommendationFactorIndex == 0)
            {
                SelectedStatus = RecommendationStatus.LANGUAGE;
                RecommendationInfo = "Most Requested Language for Tours in a previous year was: "
                    + _tourRequestService.GetMostRequestedLanguage().ToString();
            }
            else if (SelectedRecommendationFactorIndex == 1)
            {
                SelectedStatus = RecommendationStatus.LOCATION;
                int locationId = _tourRequestService.GetMostRequestedLocation();
                RecommendationInfo = "Most Requested Location for Tours in a previous year was:" 
                + _locationService.GetById(locationId).Country + ", " + 
                _locationService.GetById(locationId).City;
            }
        }

        public void CreateRecommendedTour()
        {
            if (SelectedStatus == RecommendationStatus.LANGUAGE)
            {
                SelectedTour = new Tour();
                SelectedTour.Language = _tourRequestService.GetMostRequestedLanguage();
            }
            else if (SelectedStatus == RecommendationStatus.LOCATION)
            {
                SelectedTour = new Tour();
                SelectedTour.LocationId = _tourRequestService.GetMostRequestedLocation();
            }
            else
            {
                return;
            }

            _navigationService.NavigateTo(new TourFormPage(SelectedUser, SelectedTour, FormStatus.ADD, _navigationService));
            CloseAction();
        }

        public void CancelTourRecommendation()
        {
            CloseAction();
        }



    }
}







