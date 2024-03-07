using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;
using FormStatus = BookingApplication.WPF.ViewModel.TourGuideViewModel.TourFormVMParent.FormStatus;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourDateOverviewVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Tour SelectedTour { get; set; }
        public TourDate SelectedTourDate { get; set; }
        public User SelectedUser { get; set; }
        public static ObservableCollection<TourDate> TourDates { get; set; }
        public List<TourReservation> TourReservations { get; set; }
        public FormStatus SelectedStatus { get; set; }

        private readonly TourService _tourService;
        private readonly TourPointService _tourPointService;
        private readonly TourDateService _tourDateService;
        private readonly TourReservationService _tourReservationService;
        private readonly VoucherService _voucherService;
        private readonly NavigationService _navigationService;

        public ICommand StartTourDateCommand { get; set; }
        public ICommand CancelTourDateCommand { get; set; }
        public ICommand EndTourDateCommand { get; set; }
        public ICommand ReturnOverviewCommand { get; set; } 
        public ICommand ShowTourDateFormCommand { get; set; }
        public ICommand DisplayTourDateInfoCommand { get; set; }

        private string _tourDateInfo;
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
        public TourDateOverviewVM(User user, Tour tour, FormStatus status, NavigationService navigationService)
        {
            SelectedUser = user;
            SelectedTour = tour;
            SelectedStatus = status;

            _navigationService = navigationService;
            _tourService = new TourService();
            _tourPointService = new TourPointService();
            _tourDateService = new TourDateService();
            _tourReservationService = new TourReservationService();
            _voucherService = new VoucherService();

            TourDateInfo = " ";
            StatusConfiguration();

            StartTourDateCommand = new RelayCommand(StartTourDate);
            CancelTourDateCommand = new RelayCommand(CancelTourDate);
            EndTourDateCommand = new RelayCommand(EndTourDate);
            ReturnOverviewCommand = new RelayCommand(ReturnOverview);
            ShowTourDateFormCommand = new RelayCommand(ShowTourDateForm);
            DisplayTourDateInfoCommand = new RelayCommand(DisplayTourDateInfo);
        }

        private void StatusConfiguration()
        {
            if (SelectedStatus == FormStatus.ADD)
            {
                TourDates = new ObservableCollection<TourDate>(_tourDateService.GetTourDatesByTour(SelectedTour));
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
        private void StartTourDate()
        {
            _tourReservationService.StartTourReservations(SelectedTour);
        }
        private void ReturnOverview()
        {
            _navigationService.NavigateTo(new TourFormPage(SelectedUser, SelectedTour, SelectedStatus, _navigationService));
        }
        private void ShowCancelTourError()
        {
            var result = MessageBox.Show("You can't cancel a Tour 48 hours before it begins",
                "Note", MessageBoxButton.OK);
        }

        private string GetTourDateInfo(int reservationCount, int guestNumber)
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
        private void CancelTourDate()
        {
            if (_tourDateService.IsTourDateCancellationPossible(SelectedTourDate))
            {
                ShowCancelTourError();
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
        private void EndTourDate()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to end the tour?", "End Tour",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _tourPointService.ResetTourPoints(SelectedTour);
                SelectedTourDate = _tourService.GetActiveTourDateByTour(SelectedTour);
                _tourReservationService.EndTourReservations(SelectedTour);
                _tourDateService.DeleteTourDate(SelectedTourDate);
                TourDates.Remove(SelectedTourDate);
                TourDates.Clear();
                foreach (var tourDate in _tourDateService.GetTourDatesByTour(SelectedTour))
                {
                    TourDates.Add(tourDate);
                }
            }
        }
        private void ShowTourDateForm()
        {
            TourDateForm tourDateForm = new TourDateForm(SelectedTour);
            tourDateForm.ShowDialog();
        }

        private void DisplayTourDateInfo()
        {
            if (SelectedTourDate != null)
            {
                TourReservations = _tourReservationService.GetReservationsByTourDate(SelectedTourDate);

                TourDateInfo = GetTourDateInfo(TourReservations.Count,
                    _tourReservationService.GetGuestNumberByTourReservations(TourReservations));
            }
        }
    }
}
