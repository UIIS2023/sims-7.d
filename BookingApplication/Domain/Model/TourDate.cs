using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Serializer;

namespace BookingApplication.Domain.Model
{
    public class TourDate : ISerializable, INotifyPropertyChanged
    {
        private int _id;
        private int _tourId;
        private DateTime _startTime;
        private int _currentGuests;

        public TourDate(int tourId, DateTime startTime)
        {
            TourId = tourId;
            StartTime = startTime;
            CurrentGuests = 0;
        }

        public TourDate() { }

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TourId
        {
            get => _tourId;
            set
            {
                if (_tourId != value)
                {
                    _tourId = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CurrentGuests
        {
            get => _currentGuests;
            set
            {
                if (_currentGuests != value)
                {
                    _currentGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), TourId.ToString(), StartTime.ToString(), CurrentGuests.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourId = int.Parse(values[1]);
            StartTime = DateTime.Parse(values[2]);
            CurrentGuests = int.Parse(values[3]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
