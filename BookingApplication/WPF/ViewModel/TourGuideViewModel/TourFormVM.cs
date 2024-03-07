using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;
using Image = BookingApplication.Domain.Model.Image;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourFormVM : TourFormVMParent
    {
        public ICommand CountryChangedCommand { get; set; }
        public TourFormVM(User user, Tour tour, FormStatus status, NavigationService navigationService)
        {
            SelectedUser = user;
            SelectedTour = tour;
            SelectedStatus = status;
 

            _navigationService = navigationService;
            _tourService = new TourService();
            _locationService = new LocationService();
            _imageService = new ImageService();
            _tourPointService = new TourPointService();
            _tourDateService = new TourDateService();
            _tourReservationService = new TourReservationService();
            _voucherService = new VoucherService();


            Countries = _locationService.GetLocations().Keys.ToList();
            Cities = new ObservableCollection<string>();
            TourDateInfo = "Selected Tour is currently not live";
            StatusConfiguration();
            LocationConfiguration();

            SaveTourFormCommand = new RelayCommand(SaveTourForm);
            CancelFormCommand = new RelayCommand(CancelTourForm);
            ShowTourPointsOverviewCommand = new RelayCommand(ShowTourPointsOverview);
            ShowImagesGalleryCommand = new RelayCommand(ShowImagesOverview);
            ShowTourDateOverviewCommand = new RelayCommand(ShowTourDateOverview);

            DisplayTourDateInfoCommand = new RelayCommand(DisplayTourDateInfo);
            DecreaseMaxGuestsCommand = new RelayCommand(DecreaseMaxGuests);
            IncreaseMaxGuestsCommand = new RelayCommand(IncreaseMaxGuests);
            DecreaseDurationCommand = new RelayCommand(DecreaseDuration);
            IncreaseDurationCommand = new RelayCommand(IncreaseDuration);
            CountryChangedCommand = new RelayCommand(CountryChanged);

            ShowTourDateFormCommand = new RelayCommand(ShowTourDateForm);
            
        }
        private void SaveTourForm()
        {
            SelectedTour.Validate();
            SelectedLocation.Validate();
            if (SelectedTour.IsValid && SelectedLocation.IsValid)
            {
                SelectedTour.LocationId = _locationService.ConfigureLocation(SelectedLocation).Id;
                if (SelectedStatus == FormStatus.UPDATE)
                {
                    SelectedTour.PossibleDates = TourDates.ToList();
                    Tour updatedTour = _tourService.UpdateTour(SelectedTour);
                    _locationService.UpdateLocation(SelectedLocation);
                    //_imageService.Update(SelectedThumbnailImage);

                    if (updatedTour != null)
                    {
                        //int index = TourOverview.Tours.IndexOf(SelectedTour);
                        //TourOverview.Tours.Remove(SelectedTour);
                        //TourOverview.Tours.Insert(index, updatedTour);
                        _tourPointService.SaveTourPointsInTour(updatedTour.Id);
                        _navigationService.NavigateTo(new TourOverview(SelectedUser, _navigationService));
                    }
                }
                else if (SelectedStatus == FormStatus.ADD)
                { 
                    
                    SelectedTour.PossibleDates = TourDates.ToList();
                    Tour newTour = new Tour(SelectedTour.Name, SelectedTour.LocationId, SelectedTour.Description,
                        SelectedTour.Language, SelectedTour.MaxGuests, SelectedTour.PossibleDatesIds, SelectedTour.PossibleDates, SelectedTour.Duration,
                        false, SelectedTour.ImageId, SelectedTour.ThumbnailUrl, SelectedUser.Id, false);
                    
                    newTour = _tourService.SaveTour(newTour);
                    //TourOverview.Tours.Add(newTour);
                    _tourPointService.SaveTourPointsInTour(newTour.Id);
                    _navigationService.NavigateTo(new TourOverview(SelectedUser, _navigationService));
                
                }
            }
        }
        
        
        private void CancelTourForm()
        {
            if (SelectedStatus == FormStatus.ADD)
            {
                _tourPointService.DeleteUnassignedTourPoints();
            }
            _navigationService.NavigateTo(new TourOverview(SelectedUser, _navigationService));
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
        private void DecreaseMaxGuests()
        {
            SelectedTour.MaxGuests = VaryMaxGuests(false, SelectedTour.MaxGuests);
        }
        private void IncreaseMaxGuests()
        {
            SelectedTour.Duration = VaryDuration(true, SelectedTour.Duration);
        }
        private void DecreaseDuration()
        {
            SelectedTour.Duration = VaryDuration(true, SelectedTour.Duration);
        }
        private void IncreaseDuration()
        {
            SelectedTour.Duration = VaryDuration(false, SelectedTour.Duration);
        }
        private void ShowTourDateForm()
        {
            TourDateForm tourDateForm = new TourDateForm(SelectedTour);
            tourDateForm.ShowDialog();
        }
        public int VaryMaxGuests(bool variation, int maxGuests)
        {
            if (variation)
                return ++maxGuests;
            else
                return --maxGuests;
        }

        public double VaryDuration(bool variation, double duration)
        {
            if (variation)
                return ++duration;
            else
                return --duration;
        }

    }
}
