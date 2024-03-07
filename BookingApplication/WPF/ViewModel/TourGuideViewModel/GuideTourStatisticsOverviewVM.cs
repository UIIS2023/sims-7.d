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
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class GuideTourStatisticsOverviewVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private TourService _tourService { get; set; }
        private TourDateService _tourDateService { get; set; }
        private NavigationService _navigationService { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<TourStatisticsDTO> TourStatisticsDTOs { get; set; }
        public Tour SelectedTour { get; set; }
        public TourStatisticsDTO SelectedTourStatisticsDTO { get; set; }
        public List<string> Years { get; set; }
        public User SelectedUser { get; set; }  
        public ICommand ViewStatisticsCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand ShowTourRequestStatisticsCommand { get; set; }
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

        public GuideTourStatisticsOverviewVM(User user, NavigationService navigationService)
        {
            SelectedUser = user;
            _tourService = new TourService();
            _tourDateService = new TourDateService();
            _navigationService = navigationService;
            Tours = new ObservableCollection<Tour>(_tourService.GetFinishedToursByReservations());
            TourStatisticsDTOs = new ObservableCollection<TourStatisticsDTO>(_tourService.AssignStatisticsToDTO(Tours));
            Years = _tourDateService.GetActiveToursYears(Tours).ConvertAll(y => y.ToString());
            Years.Add("All time");
            ViewStatisticsCommand = new RelayCommand(ViewStatistics);
            ShowTourRequestStatisticsCommand = new RelayCommand(ShowTourRequestStatistics);
            FilterCommand = new RelayCommand(Filter);
        }

        public void ViewStatistics()
        {
            if (SelectedTourStatisticsDTO != null)
            {
                SelectedTour = _tourService.GetTourByStatisticsDTO(SelectedTourStatisticsDTO);
                GuideTourStatistics guideTourStatistics = new GuideTourStatistics(SelectedUser, SelectedTour);
                guideTourStatistics.ShowDialog();

            }
        }

        public void Filter()
        {
            if (Year != null)
            {
                Tours.Clear();
                TourStatisticsDTOs.Clear();
                if (Year.Equals("All time"))
                {
                    foreach (Tour tour in _tourService.GetFinishedToursByReservations())
                    {
                        Tours.Add(tour);
                        ObservableCollection<TourStatisticsDTO> tourStatisticsDTOs = new ObservableCollection<TourStatisticsDTO>(_tourService.AssignStatisticsToDTO(Tours));
                        foreach (var ts in tourStatisticsDTOs)
                        {
                            TourStatisticsDTOs.Add(ts);
                        }
                    }
                }
                else
                {
                    foreach (Tour tour in _tourService.GetFinishedToursByYear(Convert.ToInt32(Year)))
                    {
                        Tours.Add(tour);
                        ObservableCollection<TourStatisticsDTO> tourStatisticsDTOs = new ObservableCollection<TourStatisticsDTO>(_tourService.AssignStatisticsToDTO(Tours));
                        foreach (var ts in tourStatisticsDTOs)
                        {
                            TourStatisticsDTOs.Add(ts);
                        }
                    }
                }

            }
        }

        private void ShowTourRequestStatistics()
        {
            _navigationService.NavigateTo(new TourRequestStatistics(SelectedUser, _navigationService));
        }
    }
}

