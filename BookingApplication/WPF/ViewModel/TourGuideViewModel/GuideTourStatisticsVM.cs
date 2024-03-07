using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using CommunityToolkit.Mvvm.Input;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{


    public class GuideTourStatisticsVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourService _tourService { get; set; }
        public Tour SelectedTour { get; set; }
        public User SelectedUser { get; set; }

        public List<double> TourStatistics { get; set; }
        public List<string> TourStatisticsDisplay { get; set; }

        public Action CloseAction { get; set; }
        public ICommand CloseCommand { get; set; }
        public GuideTourStatisticsVM(User selectedUser, Tour selectedTour)
        {
            SelectedUser = selectedUser;
            SelectedTour = selectedTour;
            _tourService = new TourService();
            TourStatistics = _tourService.GetTourStatistics(SelectedTour);
            TourStatisticsDisplay = new List<string>();
            BuildTourStatisticsDisplay();
            CloseCommand = new RelayCommand(Close);
        }


        public void BuildTourStatisticsDisplay()
        {
            TourStatisticsDisplay.Add(SelectedTour.Name);
            TourStatisticsDisplay.Add(SelectedTour.Language);
            TourStatisticsDisplay.Add(SelectedTour.Duration.ToString());
            TourStatisticsDisplay.Add(TourStatistics[0].ToString());
            TourStatisticsDisplay.Add(TourStatistics[1].ToString());
            TourStatisticsDisplay.Add(TourStatistics[2] + "%");
            TourStatisticsDisplay.Add(TourStatistics[3] + "(" + TourStatistics[4] + "% of All");
            TourStatisticsDisplay.Add(TourStatistics[5] + "(" + TourStatistics[6] + "% of All");
            TourStatisticsDisplay.Add(TourStatistics[7] + "(" + TourStatistics[8] + "% of All");
            TourStatisticsDisplay.Add(TourStatistics[9] + "%");
            TourStatisticsDisplay.Add(TourStatistics[10] + "%");
        }

        public void Close()
        {
            CloseAction();
        }

    }
}
