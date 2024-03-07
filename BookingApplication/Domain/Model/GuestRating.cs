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

    public class GuestRating : ISerializable, INotifyPropertyChanged, IDataErrorInfo
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


        private int _cleanness { get; set; }
        public int Cleanness
        {
            get => _cleanness;
            set
            {
                if (value != _cleanness)
                {
                    _cleanness = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _rulesFollowing { get; set; }
        public int RulesFollowing
        {
            get => _rulesFollowing;
            set
            {
                if (value != _rulesFollowing)
                {
                    _rulesFollowing = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _comment { get; set; }
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AccommodationId { get; set; }
        public int GuestId { get; set; }



        public string Error => null;

        public string this[string columnName]
        {
            get
            {

                if (columnName == "Cleanness")
                {
                    string c = Cleanness.ToString();
                    if (string.IsNullOrEmpty(c) || c == "0")
                        return "Cleanness is required!";
                }
                else if (columnName == "RulesFollowing")
                {
                    string rf = RulesFollowing.ToString();
                    if (string.IsNullOrEmpty(rf) || rf == "0")
                        return "RulesFollowing is required!";
                }



                return null;
            }
        }

        private readonly string[] validatedProperties = { "Cleanness", "RulesFollowing" };

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


        public GuestRating() { }

        public GuestRating(int cleanness, int rulesFollowing, string comment, int accommodationId, int guestId)
        {
            Cleanness = cleanness;
            RulesFollowing = rulesFollowing;
            Comment = comment;
            AccommodationId = accommodationId;
            GuestId = guestId;

        }

        public string[] ToCSV()
        {
            string[] csvValues =
           {
                Id.ToString(),
                Cleanness.ToString(),
                RulesFollowing.ToString(),
                Comment,
                AccommodationId.ToString(),
                GuestId.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Cleanness = Convert.ToInt32(values[1]);
            RulesFollowing = Convert.ToInt32(values[2]);
            Comment = values[3];
            AccommodationId = Convert.ToInt32(values[4]);
            GuestId = Convert.ToInt32(values[5]);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}

