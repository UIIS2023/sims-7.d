using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Serializer;
using System.Windows.Markup;


namespace BookingApplication.Domain.Model
{

    public class AccommodationReservation : ISerializable, INotifyPropertyChanged
    {
        private int _reservationId;
        private int _guestId;
        private int _accommodationId;
        public Accommodation Accommodation { get; set; }
        private int _guestLimit;
        private DateTime _startDate;
        private DateTime _endDate;

        public AccommodationReservation() { }

        public AccommodationReservation(int _reservationId, int guestId, int accommodationId, int guestLimit, DateTime startDate, DateTime endDate)
        {
            ReservationId = _reservationId;
            GuestId = guestId;
            AccommodationId = accommodationId;
            GuestLimit = guestLimit;
            StartDate = startDate;
            EndDate = endDate;
        }

        public AccommodationReservation(int guestId, int accommodationId, int guestLimit, DateTime startDate, DateTime endDate)
        {

            GuestId = guestId;
            AccommodationId = accommodationId;
            GuestLimit = guestLimit;
            StartDate = startDate;
            EndDate = endDate;
        }
        public int ReservationId
        {
            get => _reservationId;
            set
            {
                if (_reservationId != value)
                {
                    _reservationId = value;
                    OnPropertyChanged();
                }
            }
        }
        public int GuestId
        {
            get => _guestId;
            set
            {
                if (_guestId != value)
                {
                    _guestId = value;
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

        public int GuestLimit
        {
            get => _guestLimit;
            set
            {
                if (_guestLimit != value)
                {
                    _guestLimit = value;
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
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                 ReservationId.ToString(), GuestId.ToString(), AccommodationId.ToString(), GuestLimit.ToString(), StartDate.ToString(), EndDate.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            ReservationId = Convert.ToInt32(values[0]);
            GuestId = int.Parse(values[1]);
            AccommodationId = int.Parse(values[2]);
            GuestLimit = int.Parse(values[3]);
            StartDate = DateTime.Parse(values[4]);
            EndDate = DateTime.Parse(values[5]);
        }
    }
}
