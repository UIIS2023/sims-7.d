using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Serializer;

namespace BookingApplication.Domain.Model
{
    public class TourReservation : ISerializable, INotifyPropertyChanged
    {
        private int _id;
        private int _userId;
        private int _tourId;
        private int _groupSize;
        private DateTime _date;
        private int _tourPointId;
        private bool _isChecked;
        private double _averageAge;
        private bool _hasVoucher;
        private bool _isCompleted;
        private bool _hasStarted;
        private bool _acceptedRequest;

        public TourReservation() { }
        public TourReservation(int userId, int tourId, int groupSize, DateTime date, int tourPointId,
            bool isChecked, double averageAge, bool hasVoucher, bool isCompleted, bool hasStarted, bool acceptedRequest)
            {
            UserId = userId;
            TourId = tourId;
            GroupSize = groupSize;
            Date = date;
            TourPointId = tourPointId;
            IsChecked = isChecked;
            AverageAge = averageAge;
            HasVoucher = hasVoucher;
            IsCompleted = isCompleted;
            HasStarted = hasStarted;
            AcceptedRequest = acceptedRequest;
        }

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

        public int UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
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

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public int TourPointId
        {
            get => _tourPointId;
            set
            {
                if (_tourPointId != value)
                {
                    _tourPointId = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged();
                }
            }
        }
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
        public bool HasVoucher
        {
            get => _hasVoucher;
            set
            {
                if (_hasVoucher != value)
                {
                    _hasVoucher = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool HasStarted
        {
            get => _hasStarted;
            set
            {
                if (_hasStarted != value)
                {
                    _hasStarted = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool AcceptedRequest
        {
            get => _acceptedRequest;
            set
            {
                if (_acceptedRequest != value)
                {
                    _acceptedRequest = value;
                    OnPropertyChanged();
                }
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), UserId.ToString(), TourId.ToString(), GroupSize.ToString(), Date.ToString(), TourPointId.ToString(),
                IsChecked.ToString(), AverageAge.ToString(), HasVoucher.ToString(), IsCompleted.ToString(), HasStarted.ToString(), AcceptedRequest.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = int.Parse(values[1]);
            TourId = int.Parse(values[2]);
            GroupSize = int.Parse(values[3]);
            Date = DateTime.Parse(values[4]);
            TourPointId = Convert.ToInt32(values[5]);
            IsChecked = Convert.ToBoolean(values[6]);
            AverageAge = Convert.ToDouble(values[7]);
            HasVoucher = Convert.ToBoolean(values[8]);
            IsCompleted = Convert.ToBoolean(values[9]);
            HasStarted = Convert.ToBoolean(values[10]);
            AcceptedRequest = Convert.ToBoolean(values[11]);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
