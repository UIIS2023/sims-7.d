using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using BookingApplication.Serializer;


namespace BookingApplication.Domain.Model
{
    public enum AccommodationType { Apartment, House, Shack }
    public class Accommodation : ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {

        private int _id { get; set; }
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
        private string _name { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _locationId { get; set; }
        public int LocationId
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
        private AccommodationType _accommodationType { get; set; }
        public AccommodationType AccommodationType
        {
            get => _accommodationType;
            set
            {
                if (value != _accommodationType)
                {
                    _accommodationType = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _maxGuests { get; set; }
        public int MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (value != _maxGuests)
                {
                    _maxGuests = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _minReservationDays { get; set; }
        public int MinReservationDays
        {
            get => _minReservationDays;
            set
            {
                if (value != _minReservationDays)
                {
                    _minReservationDays = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _daysBeforeCancelling { get; set; }
        public int DaysBeforeCancelling
        {
            get => _daysBeforeCancelling;
            set
            {
                if (value != _daysBeforeCancelling)
                {
                    _daysBeforeCancelling = value;
                    OnPropertyChanged();
                }
            }
        }



        public List<int> ImagesIds { get; set; }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        return "You must fill this!";
                }


                else if (columnName == "AccommodationType")
                {
                    if (string.IsNullOrEmpty(AccommodationType.ToString()))
                        return "AccommodationType is required";
                }
                else if (columnName == "MaxGuests")
                {
                    string mg = MaxGuests.ToString();
                    if (string.IsNullOrEmpty(mg) || mg == "0")
                        return "You must fill this!";
                    else if (mg.Contains("-"))
                        return "Number must be positive!";
                }
                else if (columnName == "MinReservationDays")
                {
                    string mrd = MinReservationDays.ToString();
                    if (string.IsNullOrEmpty(mrd) || mrd == "0")
                        return "You must fill this!";
                    else if (mrd.Contains("-"))
                        return "Number must be positive!";
                    
                }
                else if (columnName == "DaysBeforeCancelling")
                {
                    string dbc = DaysBeforeCancelling.ToString();
                    if (string.IsNullOrEmpty(dbc) || dbc == "0")
                        return "You must fill this!";
                    else if (dbc.Contains("-"))
                        return "Number must be positive!";
                }


                return null;
            }
        }

        private readonly string[] validatedProperties = { "Name", "AccommodationType", "MaxGuests", "MinReservationDays", "DaysBeforeCancelling" };

        public bool IsValid
        {
            get
            {
                foreach (var property in validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        public int OwnerId { get; internal set; }

        public Accommodation()
        {
            ImagesIds = new List<int>();
        }
        public Accommodation(int id, string name, int locationId, AccommodationType type, int maxGuests, int minReservationDays, int daysBeforeCancelling, List<int> imagesIds)
        {
            Id = id;
            Name = name;
            LocationId = locationId;
            AccommodationType = type;
            MaxGuests = maxGuests;
            MinReservationDays = minReservationDays;
            DaysBeforeCancelling = daysBeforeCancelling;
            ImagesIds = imagesIds;
        }

        public Accommodation(string name, int locactionId, AccommodationType type, int maxGuests, int minReservationDays, int daysBeforeCancelling, List<int> imagesIds)
        {

            Name = name;
            LocationId = locactionId;
            AccommodationType = type;
            MaxGuests = maxGuests;
            MinReservationDays = minReservationDays;
            DaysBeforeCancelling = daysBeforeCancelling;
            ImagesIds = imagesIds;
        }


        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                LocationId.ToString(),
                AccommodationType.ToString(),
                MaxGuests.ToString(),
                MinReservationDays.ToString(),
                DaysBeforeCancelling.ToString(),
                string.Join(";",ImagesIds)
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LocationId = Convert.ToInt32(values[2]);
            AccommodationType = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[3]);
            MaxGuests = Convert.ToInt32(values[4]);
            MinReservationDays = Convert.ToInt32(values[5]);
            DaysBeforeCancelling = Convert.ToInt32(values[6]);
            ImagesIds = values[7].Split(';').Select(int.Parse).ToList();

        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
