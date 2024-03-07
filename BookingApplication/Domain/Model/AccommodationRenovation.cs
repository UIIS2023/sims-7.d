using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApplication.Domain.Model
{
    public class AccommodationRenovation : ISerializable, INotifyPropertyChanged
    {
        private int _renovationId;
        private int _accommodationId;
        public Accommodation Accommodation { get; set; }
        private int _numberOfDays;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _description;

        public AccommodationRenovation() { }

        public AccommodationRenovation(int renovationId, int accommodationId, int numberOfDays, DateTime startDate, DateTime endDate)
        {
            RenovationId = _renovationId;
            AccommodationId = accommodationId;
            NumberOfDays = numberOfDays;
            StartDate = startDate;
            EndDate = endDate;
        }

        public AccommodationRenovation(int accommodationId, int numberOfDays, DateTime startDate, DateTime endDate, string description)
        {

            AccommodationId = accommodationId;
            NumberOfDays = numberOfDays;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
        }
        public int RenovationId
        {
            get => _renovationId;
            set
            {
                if (_renovationId != value)
                {
                    _renovationId = value;
                    OnPropertyChanged();
                }
            }
        }
       

        public int AccommodationId
        {
            get => _accommodationId;
            set
            {
                if (_accommodationId != value)
                {
                    _accommodationId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int  NumberOfDays
        {
            get => _numberOfDays;
            set
            {
                if (_numberOfDays != value)
                {
                    _numberOfDays = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                 RenovationId.ToString(), AccommodationId.ToString(), NumberOfDays.ToString(), StartDate.ToString(), EndDate.ToString(), Description.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            RenovationId = Convert.ToInt32(values[0]);
            AccommodationId = int.Parse(values[1]);
            NumberOfDays = int.Parse(values[2]);
            StartDate = DateTime.Parse(values[3]);
            EndDate = DateTime.Parse(values[4]);
            Description = values[5];
        }
    }
}
