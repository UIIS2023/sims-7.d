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
    public class OwnerRating : ISerializable, INotifyPropertyChanged, IDataErrorInfo
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
        private int _ownerCorrectness { get; set; }
        public int OwnerCorrectness
        {
            get => _ownerCorrectness;
            set
            {
                if (value != _ownerCorrectness)
                {
                    _ownerCorrectness = value;
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

        private int? renovationLevel { get; set; }
        public int? RenovationLevel
        {
            get => renovationLevel;
            set
            {
                renovationLevel = value;
                OnPropertyChanged();
            }
        }

        private string renovationComment { get; set; }
        public string RenovationComment
        {
            get => renovationComment;
            set
            {
                renovationComment = value;
                OnPropertyChanged();
            }
        }

        public List<int> ImagesIds { get; set; }

        public int GuestId { get; set; }
        public int AccommodationId { get; set; }
        public int OwnerId { get; set; }



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
                    string oc = OwnerCorrectness.ToString();
                    if (string.IsNullOrEmpty(oc) || oc == "0")
                        return "OwnerCorrectness is required!";
                }



                return null;
            }
        }

        private readonly string[] validatedProperties = { "Cleanness", "OwnerCorrectness" };

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


        public OwnerRating()
        {
            ImagesIds = new List<int>();
        }

        public OwnerRating(int cleanness, int ownerCorrectness, string comment, List<int> imagesIds, int guestId, int accommodationId, int ownerId)
        {
            Cleanness = cleanness;
            OwnerCorrectness = ownerCorrectness;
            Comment = comment;
            ImagesIds = imagesIds;
            GuestId = guestId;
            AccommodationId = accommodationId;
            OwnerId = ownerId;

        }

        public string[] ToCSV()
        {
            string[] csvValues =
           {
                Id.ToString(),
                Cleanness.ToString(),
                OwnerCorrectness.ToString(),
                Comment,
                string.Join(";",ImagesIds),
                GuestId.ToString(),
                AccommodationId.ToString(),
                OwnerId.ToString(),
                RenovationLevel.HasValue ? RenovationLevel.ToString() : "null",
                RenovationComment
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Cleanness = Convert.ToInt32(values[1]);
            OwnerCorrectness = Convert.ToInt32(values[2]);
            Comment = values[3];
            if (!string.IsNullOrEmpty(values[4]))
            {
                ImagesIds = values[4].Split(';').Select(int.Parse).ToList();
            }
            GuestId = Convert.ToInt32(values[5]);
            AccommodationId = Convert.ToInt32(values[6]);
            OwnerId = Convert.ToInt32(values[7]);
            RenovationLevel = values[8].Equals("null") ? null : Convert.ToInt32(values[6]);
            RenovationComment = values[9];
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
