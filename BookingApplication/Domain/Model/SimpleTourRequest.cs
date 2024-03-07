using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Serializer;

namespace BookingApplication.Domain.Model
{
    public enum Status{Waiting, Accepted, Denied}
    public class SimpleTourRequest : INotifyPropertyChanged, ISerializable
    {
        private int _id;
        private int _userId;
        private int? _locationId;
        private Location _location;
        private string _language;
        private int? _guestsNumber;
        private string _description;
        private DateTime? _acceptedDate;
        private DateTime? _startDate;
        private Status _status;
        private DateTime? _endDate;
        private bool _isSeen;

        public int Id
        {
            get => _id;
            set
            {
                if (value != _id)
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
                if (value != _userId)
                {
                    _userId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? LocationId
        {
            get => _locationId;
            set
            {
                if (value != _locationId)
                {
                    _locationId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? GuestsNumber
        {
            get => _guestsNumber;
            set
            {
                if (value != _guestsNumber)
                {
                    _guestsNumber = value;
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

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    OnPropertyChanged();
                }
            }
        }


        public Status Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? AcceptedDate
        {
            get => _acceptedDate;
            set
            {
                if (value != _acceptedDate)
                {
                    _acceptedDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public Location Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSeen
        {
            get => _isSeen;
            set
            {
                if (value != _isSeen)
                {
                    _isSeen = value;
                    OnPropertyChanged();
                }
            }
        }

        public string[] ToCSV()
        {
            var accepetedDate = "";
            if(AcceptedDate != null)
                accepetedDate = ((DateTime) AcceptedDate).ToString("dd.MM.yyyy.");
            string[] csvValues =
            {
               Id.ToString(), UserId.ToString(), LocationId.ToString(), Language, GuestsNumber.ToString(), Description, ((DateTime)StartDate).ToString("dd.MM.yyyy."), 
               ((DateTime)EndDate).ToString("dd.MM.yyyy."), Status.ToString(), accepetedDate, IsSeen.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            LocationId = int.Parse(values[2]);
            Language = values[3];
            GuestsNumber = int.Parse(values[4]);
            Description = values[5];
            StartDate = DateTime.Parse(values[6]);
            EndDate = DateTime.Parse(values[7]);
            Status = (Status) Enum.Parse(typeof(Status), values[8]);
            if(!string.IsNullOrEmpty(values[9]))
                AcceptedDate = DateTime.Parse(values[9]);
            IsSeen = bool.Parse(values[10]);
        }

        public bool IsRequestFilled()
        {
            return !string.IsNullOrEmpty(Language) && !string.IsNullOrEmpty(Description) && GuestsNumber != 0 && GuestsNumber != null
                   && StartDate != null && EndDate != null && LocationId != 0 && LocationId != null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
