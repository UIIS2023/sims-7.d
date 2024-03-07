using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;
using FormStatus = BookingApplication.WPF.ViewModel.TourGuideViewModel.TourFormVMParent.FormStatus;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourRequestDateFormVM
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly TourDateService _tourDateService;
        private readonly SimpleTourRequestService _tourRequestService;
        private readonly NavigationService _navigationService;
        public User SelectedUser { get; set; }
        public SimpleTourRequest SelectedTourRequest { get; set; }
        public Tour SelectedTour { get; set; }

        public ICommand CreateRecommendedTourCommand { get; set; }
        public ICommand CancelTourRequestDateFormCommand { get; set; }
        public Action CloseAction { get; set; }

        private string _midday;
        public string Midday
        {
            get
            {
                return _midday;
            }
            set
            {
                if (value != _midday)
                {
                    _midday = value;
                }
            }
        }
        private int _hours;
        public int Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                if (value != _hours)
                {
                    _hours = value;
                }
            }
        }

        private int _minutes;
        public int Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                if (value != _minutes)
                {
                    _minutes = value;
                }
            }
        }

        private bool _minutesSelected;
        public bool MinutesSelected
        {
            get => _minutesSelected;
            set
            {
                if (value != _minutesSelected)
                {
                    _minutesSelected = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _hoursSelected;
        public bool HoursSelected
        {
            get => _hoursSelected;
            set
            {
                if (value != _hoursSelected)
                {
                    _hoursSelected = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _middaySelected;
        public bool MiddaySelected
        {
            get => _middaySelected;
            set
            {
                if (value != _middaySelected)
                {
                    _middaySelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _inputDate;

        public DateTime InputDate
        {
            get
            {
                return _inputDate;
            }
            set
            {
                if (value != _inputDate)
                {
                    _inputDate = value;
                }
            }
        }


        public TourRequestDateFormVM(User user, NavigationService navigationService, SimpleTourRequest tourRequest)
        {
            SelectedUser = user;
            _navigationService = navigationService;
            _tourRequestService = new SimpleTourRequestService();
            _tourDateService = new TourDateService();

            SelectedTourRequest = tourRequest;
            InputDate = DateTime.Now;

            CreateRecommendedTourCommand = new RelayCommand(CreateRecommendedTour);
            CancelTourRequestDateFormCommand = new RelayCommand(CancelTourRequestDateForm);

        }

        private void CreateRecommendedTour()
        {
            if (AreComboBoxesSelected())
            {
                AcceptedDateParse();
                SelectedTourRequest.Status = Status.Accepted;
                _tourRequestService.UpdateTourRequest(SelectedTourRequest);
                InitializeRecommmendedTour();
                CloseAction();
            }
        }

        private void InitializeRecommmendedTour()
        {
            SelectedTour = new Tour();
            SelectedTour.Language = SelectedTourRequest.Language;
            SelectedTour.LocationId = (int)SelectedTourRequest.LocationId;
            TourDate tourDate = new TourDate(SelectedTour.Id, (DateTime)SelectedTourRequest.AcceptedDate);
            SelectedTour.PossibleDates = new List<TourDate>();
            SelectedTour.PossibleDatesIds = new List<int>();
            SelectedTour.PossibleDates.Add(tourDate);
            tourDate = _tourDateService.SaveTourDate(tourDate);
            tourDate.CurrentGuests = (int)SelectedTourRequest.GuestsNumber;
            tourDate.TourId = SelectedTour.Id;
            _tourDateService.UpdateTourDate(tourDate);
            SelectedTour.PossibleDatesIds.Add(tourDate.Id);
            _navigationService.NavigateTo(new TourFormPage(SelectedUser, SelectedTour, FormStatus.ADD, _navigationService));
        }

        private void CancelTourRequestDateForm()
        {
            CloseAction();
        }

        private bool AreComboBoxesSelected()
        {
            if (MiddaySelected && HoursSelected && MinutesSelected)
                return true;
            return true;
        }

        private bool DetermineMidday()
        {
            if (Midday == "PM")
            {
                return true;
            }
            return false;
        }

        private void AcceptedDateParse()
        {
            if (DetermineMidday())
            {
                Hours += 12;
            }

            //Hours = Hours + Convert.ToInt32(HourComboBox.SelectedValue.ToString());
            //Minutes = Minutes + Convert.ToInt32(MinuteComboBox.SelectedValue.ToString());

            //String hourString = Hours.ToString();
            //String minuteString = Midday.ToString();

            //Minutes = Convert.ToInt32(minuteString);
            //Hours += Convert.ToInt32(hourString);

            SelectedTourRequest.AcceptedDate =
                new DateTime(InputDate.Year, InputDate.Month, InputDate.Day, Hours, Minutes, 0);

        }

    }
}
