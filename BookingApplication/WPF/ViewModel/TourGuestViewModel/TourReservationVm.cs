using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuestView;
using CommunityToolkit.Mvvm.Input;
using Image = BookingApplication.Domain.Model.Image;

namespace BookingApplication.WPF.ViewModel.TourGuestViewModel
{
    public class TourReservationVm : INotifyPropertyChanged
    {
        public Tour AlternativeTour { get; set; }
        public Tour SelectedTour { get; set; }

        public readonly LocationRepository LocationRepository;
        public User User { get; set; }
        public List<TourDate> TourDates { get; set; }
        public ObservableCollection<TourReservation> TourReservations { get; set; }
        public List<DateTime> SelectedDates { get; set; }
        public ObservableCollection<string> Dates { get; set; }
        public Location TourLocation { get; set; }

        private readonly ImageService ImageService;
        public DateTime? SelectedDate { get; set; }

        private readonly NavigationService _navigationService;

        private readonly TourService _tourService;
        public ICommand BookTourCommand { get; set; }
        public ICommand AvailableCommand { get; set; }
        public ICommand NextImageCommand { get; set; }
        public ICommand PreviousImageCommand { get; set; }
        public ICommand DateChangedCommand { get; set; }
        public ICommand ShowAlternativeTourCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ObservableCollection<Voucher> Vouchers { get; set; }
        public VoucherService VoucherService { get; set; }
        public Voucher SelectedVoucher { get; set; }

        private int _groupSize;

        public int GroupSize
        {
            get => _groupSize;
            set
            {
                if (_groupSize != value)
                {
                    _groupSize = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isAlternativeTourVisible;

        public bool IsAlternativeTourVisible
        {
            get => _isAlternativeTourVisible;
            set
            {
                if (_isAlternativeTourVisible != value)
                {
                    _isAlternativeTourVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _currentUrl;
        public string CurrentUrl
        {
            get => _currentUrl;
            set
            {
                if (_currentUrl != value)
                {
                    _currentUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _averageAge = 1;
        public double AverageAge
        {
            get => _averageAge;
            set
            {
                if (_averageAge != value)
                {
                    _averageAge = value;
                    OnPropertyChanged();
                }
            }
        }



        private string _feedback;

        public string Feedback
        {
            get => _feedback;
            set
            {
                if (_feedback != value)
                {
                    _feedback = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedDateIndex = -1;

        public int SelectedDateIndex
        {
            get => _selectedDateIndex;
            set
            {
                if (_selectedDateIndex != value)
                {
                    _selectedDateIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedVoucherIndex = -1;

        public int SelectedVoucherIndex
        {
            get => _selectedVoucherIndex;
            set
            {
                if (_selectedVoucherIndex != value)
                {
                    _selectedVoucherIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _buttonIsEnabled = false;

        public bool ButtonIsEnabled
        {
            get => _buttonIsEnabled;
            set
            {
                if (_buttonIsEnabled != value)
                {
                    _buttonIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isConfirmationVisible;

        public bool IsConfirmationVisible
        {
            get => _isConfirmationVisible;
            set
            {
                if (_isConfirmationVisible != value)
                {
                    _isConfirmationVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Tour> AlternativeTours { get; set; }
        public TourReservationVm(Tour selectedTour, User user, NavigationService navigationService)
        {
            IsAlternativeTourVisible = false;
            SelectedTour = selectedTour;
            User = user;
            IsConfirmationVisible = false;
            SelectedVoucher = new Voucher();
            VoucherService = new VoucherService();
            VoucherService.RemoveExpiredVouchers();
            Vouchers = new ObservableCollection<Voucher>(VoucherService.GetVouchersByGuestId(user.Id));
            AlternativeTour = new Tour();
            AlternativeTours = new ObservableCollection<Tour>();
            ButtonIsEnabled = false;
            _tourService = new TourService();
            var tourDateService = new TourDateService();
            _navigationService = navigationService;
            ImageService = new ImageService();

            CurrentUrl = ImageService.GetImageUrl(ImageService.GetAll()[0].Id);
            LocationRepository = new LocationRepository();
            TourLocation = LocationRepository.GetById(SelectedTour.LocationId);
            TourDates = SelectedTour.PossibleDates;
            TourReservations = new ObservableCollection<TourReservation>();
            SelectedDates = new List<DateTime>();
            SelectedTour.PossibleDates = tourDateService.GetTourDatesByIds(SelectedTour.PossibleDatesIds);
            SelectedDates = SelectedTour.PossibleDates.Select(date => date.StartTime).ToList();
            Dates = new ObservableCollection<string>();
            foreach (var date in tourDateService.PossibleDatesToString(SelectedDates))
            {
                Dates.Add(date);
            }

            AssignCommands();
            

        }
        
        private void AssignCommands()
        {
            BookTourCommand = new RelayCommand(BookTour);
            AvailableCommand = new RelayCommand(CheckTourAvailability);
            DateChangedCommand = new RelayCommand(DateChanged, IsDateSelected);
            ShowAlternativeTourCommand =  new RelayCommand<Tour>(selectedTour =>
            {
                AlternativeTour = selectedTour;
                PickAlternative();
            });
            NextImageCommand = new RelayCommand(ChangeImageForward);
            PreviousImageCommand = new RelayCommand(ChangeImageBackward);
            CancelCommand = new RelayCommand(CancelBooking);
        }

        private void CancelBooking()
        {
            Feedback = "";
            GroupSize = 0;
            AverageAge = 0;
            IsConfirmationVisible = false;
            IsAlternativeTourVisible = false;
            SelectedDateIndex = -1;
            SelectedVoucherIndex = -1;
            ButtonIsEnabled = false;
            AlternativeTours.Clear();
        }

        private int CalculateAvailableGroupSize(int groupSize)
        {
            return SelectedTour.MaxGuests - SelectedTour.PossibleDates.Find(d => d.StartTime == SelectedDate).CurrentGuests;
        }

        private bool IsSizeAvailable(int? groupSize, Tour tour)
        {
            return groupSize <= tour.MaxGuests - tour.PossibleDates.Find(d => d.StartTime == SelectedDate).CurrentGuests;
        }

        private bool IsTourFilled(Tour tour)
        {
            return tour.MaxGuests - tour.PossibleDates.Find(d => d.StartTime == SelectedDate).CurrentGuests == 0;
        }
        private void CheckTourAvailability()
        {
            if (IsTourFilled(SelectedTour))
            {
                Feedback = "The selected tour is full. Select alternative from below.";
                IsConfirmationVisible = false;
                IsAlternativeTourVisible = true;
                AlternativeTours.Clear();
                foreach (var tour in _tourService.GetAlternativeToursOnLocation(TourLocation.Id, SelectedTour.Id))
                {
                    AlternativeTours.Add(tour);
                }
                return;
            }
            if (IsSizeAvailable(GroupSize, SelectedTour))
            {
                IsAlternativeTourVisible = false;
                IsConfirmationVisible = true;
                Feedback = "";
            }
            else
            {
                //BookTourButton.IsEnabled = false;
                Feedback = " Maximum available size at the moment is: " + CalculateAvailableGroupSize(GroupSize);
            }
        }

        private void ChangeImageForward()
        {
            CurrentUrl = ImageService.ChangeImageForward();
        }

        private void ChangeImageBackward()
        {
            CurrentUrl = ImageService.ChangeImageBackward();
        }

        public void BookTour(int userId, int tourId, DateTime date, int groupSize, double averageAge)
        {
            var tourReservation = new TourReservation(userId, tourId, groupSize, date, -1, false, averageAge, SelectedVoucher != null, false, false, false);
            SelectedTour.PossibleDates[0].CurrentGuests += groupSize;

            var tourDateService = new TourDateService();
            tourDateService.UpdateTourDate(SelectedTour.PossibleDates.Find(d => d.StartTime == SelectedDate));

            var tourReservationService = new TourReservationService();
            TourReservations.Add(tourReservation);
            tourReservationService.SaveTourReservation(tourReservation);
        }

        private bool IsDateSelected()
        {
            return SelectedDateIndex != -1;
        }

        private void DateChanged()
        {
            
                SelectedDate = SelectedDates[SelectedDateIndex];
                ButtonIsEnabled = true;
            

        }

        private void BookTour()
        {
            BookTour(User.Id, SelectedTour.Id, SelectedTour.PossibleDates[0].StartTime, GroupSize, AverageAge);
            if (SelectedVoucher != null)
            {
                VoucherService.DeleteVoucher(SelectedVoucher);
            }
            _navigationService.NavigateTo(new ToursOverviewPage(User, _navigationService, false));
        }

        private void PickAlternative()
        {
            _navigationService.NavigateTo(new TourReservationPage(AlternativeTour, User, _navigationService));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
