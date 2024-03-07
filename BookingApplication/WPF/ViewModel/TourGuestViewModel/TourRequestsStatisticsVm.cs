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
using BookingApplication.Services;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace BookingApplication.WPF.ViewModel.TourGuestViewModel
{
    public class TourRequestsStatisticsVm : INotifyPropertyChanged
    {
        private SeriesCollection _series;
        public SeriesCollection Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged(nameof(Series));
            }
        }
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection LSeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public List<string> LLabels { get; set; }
        public User User { get; set; }
        public ObservableCollection<SimpleTourRequest> Requests { get; set; }
        public ICommand GetStatisticsCommand { get; set; }
        public ICommand ResetFilterCommand { get; set; }
        private DateTime? _startDate;
        private int _averageGroupSize;

        private bool _isLabelVisible = false;

        public bool IsLabelVisible
        {
            get => _isLabelVisible;
            set
            {
                _isLabelVisible = value;
                OnPropertyChanged();
            }
        }
        public int AverageGroupSize
        {
            get => _averageGroupSize;
            set
            {
                _averageGroupSize = value;
                OnPropertyChanged();
            }
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public TourRequestsStatisticsVm(User user, NavigationService navigationService)
        {
            User = user;
            var requestService = new SimpleTourRequestService();
            Requests = new ObservableCollection<SimpleTourRequest>(requestService.GetAll());
            LoadStatistics();
            GetStatisticsCommand = new RelayCommand(FilterByDate);
            ResetFilterCommand = new RelayCommand(ResetFilter);
            AverageGroupSize = requestService.GetAverageGroupSize(Requests);
        }

        private void ResetFilter()
        {
            StartDate = null;
            FilterByDate();
        }

        private void LoadStatistics()
        {
           LoadRequestStatusStatistics();
           LoadLocationsStatistics();
           LoadLanguageStatistics();
        }

        private void LoadRequestStatusStatistics()
        {
            var statisticsService = new TourRequestStatisticsService();
            var values = statisticsService.GetCountByRequestStatus(Requests, User.Id);
            Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Accepted",
                    Values = new ChartValues<double> { values.Item1 }
                },
                new PieSeries
                {
                    Title = "Denied",
                    Values = new ChartValues<double> { values.Item2 }
                },
                new PieSeries
                {
                    Title = "Waiting",
                    Values = new ChartValues<double> { values.Item3 }
                }
            };
        }
        private void LoadLocationsStatistics()
        {
            var locationService = new LocationService();
            var locations = new HashSet<Location>();

            foreach (var request in Requests)
            {
                if(request.UserId == User.Id)
                    locations.Add(request.Location);
            }
            
            var requests = new List<int>();
            foreach (var location in locations)
            {
                requests.Add(locationService.GetRequestNumberForLocation(location, Requests, User.Id));
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Requests",
                    Values = new ChartValues<ObservableValue>(requests.Select(r => new ObservableValue(r)))
                }
            };
            Labels = new List<string>();
            foreach (var location in locations)
            {
                Labels.Add(location.City + ", " + location.Country);
            }
        }

        private void LoadLanguageStatistics()
        {
            var requestService = new SimpleTourRequestService();
            var languages = new HashSet<string> ();
            foreach (var request in Requests)
            {
                languages.Add(request.Language);
            }

            LLabels = new List<string>();
            foreach (var label in languages)
            {
                LLabels.Add(label);
            }

            var requests = new List<int>();
            foreach (var label in LLabels)
            {
                requests.Add(requestService.GetRequestNumberByLanguage(label, Requests, User.Id));
            }

            LSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Requests",
                    Values = new ChartValues<ObservableValue>(requests.Select(r => new ObservableValue(r)))
                }
            };

        }
        private void FilterByDate()
        {
            Requests.Clear();
            var requestService = new SimpleTourRequestService();
            foreach (var request in requestService.GetStatisticsByYear(StartDate))
            {
                Requests.Add(request);
            }

            IsLabelVisible = Requests.Count == 0;

            LoadRequestStatusStatistics();
            AverageGroupSize = requestService.GetAverageGroupSize(Requests);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
