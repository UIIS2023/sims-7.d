using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;
using Image = BookingApplication.Domain.Model.Image;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourFormLiveVM : TourFormVMParent
    {
        public ICommand ReturnFormCommand { get; set; }
        public ICommand EndTourDateCommand { get; set; }

        public TourFormLiveVM(User user, Tour tour, FormStatus status, NavigationService navigationService)
        {
            SelectedUser = user;
            SelectedTour = tour;
            SelectedStatus = status;

            _navigationService = navigationService;
            _tourService = new TourService();
            _locationService = new LocationService();
            _tourPointService = new TourPointService();
            _tourDateService = new TourDateService();
            _tourReservationService = new TourReservationService();
            _voucherService = new VoucherService();
            Countries = _locationService.GetLocations().Keys.ToList();
            TourDateInfo = "Selected Tour is currently live";

            TourDates = new ObservableCollection<TourDate>(_tourDateService.GetTourDatesByTour(SelectedTour));
            LocationConfiguration();

            ReturnFormCommand = new RelayCommand(ReturnForm);
            CancelTourDateCommand = new RelayCommand(CancelTourDate);
            EndTourDateCommand = new RelayCommand(EndTourDate);
            StartTourDateCommand = new RelayCommand(StartTourDate);
            ShowTourPointsOverviewCommand = new RelayCommand(ShowTourPointsOverview);
            ShowImagesGalleryCommand = new RelayCommand(ShowImagesOverview);
            ShowTourDateOverviewCommand = new RelayCommand(ShowTourDateOverview);
            ShowTourDateFormCommand = new RelayCommand(ShowTourDateForm);
            DisplayTourDateInfoCommand = new RelayCommand(DisplayTourDateInfo);
        }
        
        private void ReturnForm()
        {
            _navigationService.NavigateTo(new TourOverview(SelectedUser, _navigationService));
        }
        private void StartTourDate()
        {
            _tourReservationService.StartTourReservations(SelectedTour);
        }


        private void EndTourDate()
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
