using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApplication.Serializer;

namespace BookingApplication.Domain.Model
{
    public class TourNotification : ISerializable, INotifyPropertyChanged
    {
        private int _id;
        private int _tourReservationId;

        public TourNotification() { }

        public TourNotification(int tourReservationId)
        {
            TourReservationId = tourReservationId;
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

        public int TourReservationId
        {
            get => _tourReservationId;
            set
            {
                if (_tourReservationId != value)
                {
                    _tourReservationId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                TourReservationId.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourReservationId = Convert.ToInt32(values[1]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
