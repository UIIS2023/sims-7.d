using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.CompilerServices;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourDateFormVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TourDate SelectedTourDate { get; set; }

        private readonly TourDateService _tourDateService;

        public ICommand SaveTourDateFormCommand { get; set; }
        public ICommand CancelTourDateFormCommand { get; set; }
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
        public TourDateFormVM(Tour selectedTour)
        {
            _tourDateService = new TourDateService();
            SelectedTourDate = new TourDate(selectedTour.Id, DateTime.Now);
            InputDate = DateTime.Now;
            SaveTourDateFormCommand = new RelayCommand(SaveTourDateForm);
            CancelTourDateFormCommand = new RelayCommand(CancelTourDateForm);
        }
        private void SaveTourDateForm()
        {
            if (AreComboBoxesSelected())
            {
                TourDateParse();
                _tourDateService.SaveTourDate(SelectedTourDate);
                TourDateOverviewVM.TourDates.Add(SelectedTourDate);
                CloseAction();
            }

        }

        private void CancelTourDateForm()
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

        private void TourDateParse()
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


            SelectedTourDate.StartTime =
                new DateTime(InputDate.Year, InputDate.Month, InputDate.Day, Hours, Minutes, 0);

        }



    }
}
