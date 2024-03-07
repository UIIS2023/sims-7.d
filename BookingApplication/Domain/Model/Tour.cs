using System;
using System.Collections.Generic;
using System.Linq;
using BookingApplication.Serializer;
using BookingApplication.Domain.Regexes;

namespace BookingApplication.Domain.Model
{

    public class Tour : ValidationBase, ISerializable//, INotifyPropertyChanged
    {

        private int _id;
        private string _name;
        private int _locationId;
        private string _description;
        private string _language;
        private int _maxGuests;
        private List<int> _possibleDatesIds;
        private List<TourDate> _possibleDates;
        private double _duration;
        private List<TourPoint> _tourPoints;
        private bool _began;
        private List<int> _imageId;
        private string _thumbnailUrl;
        private int _guideId;
        private bool _isNewlyCreated;
        public Tour()
        {

        }

        public Tour(string name, int locationId, string description, string language, int maxGuests,
            List<int> possibleDatesIds, List<TourDate> possibleDates, double duration, bool began, List<int> imageId, string thumbnailUrl,
            int guideId, bool isNewlyCreated)
        {
            Name = name;
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            PossibleDatesIds = new List<int>(possibleDatesIds);
            PossibleDates = new List<TourDate>(possibleDates);
            Duration = duration;
            TourPoints = new List<TourPoint>();
            Began = began;
            ImageId = new List<int>(imageId);
            ThumbnailUrl = thumbnailUrl;
            GuideId = guideId;
            IsNewlyCreated = isNewlyCreated;
        }

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public int LocationId
        {
            get => _locationId;
            set
            {
                if (_locationId != value)
                {
                    _locationId = value;
                    OnPropertyChanged("LocationId");
                }
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        public int MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (_maxGuests != value)
                {
                    _maxGuests = value;
                    OnPropertyChanged("MaxGuests");
                }
            }
        }

        public List<int> PossibleDatesIds
        {
            get => _possibleDatesIds;
            set
            {
                if (_possibleDatesIds != value)
                {
                    _possibleDatesIds = value;
                    OnPropertyChanged("PossibleDatesIds");
                }
            }
        }

        public List<TourDate> PossibleDates
        {
            get => _possibleDates;
            set
            {
                if (_possibleDates != value)
                {
                    _possibleDates = value;
                    OnPropertyChanged("PossibleDates");
                }
            }
        }

        public double Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }

        public List<TourPoint> TourPoints
        {
            get => _tourPoints;
            set
            {
                if (_tourPoints != value)
                {
                    _tourPoints = value;
                    OnPropertyChanged("TourPoints");
                }
            }
        }

        public bool Began
        {
            get => _began;
            set
            {
                if (_began != value)
                {
                    _began = value;
                    OnPropertyChanged("Began");
                }
            }
        }

        public List<int> ImageId
        {
            get => _imageId;
            set
            {
                if (_imageId != value)
                {
                    _imageId = value;
                    OnPropertyChanged("ImageId");
                }
            }
        }

        public string ThumbnailUrl
        {
            get => _thumbnailUrl;
            set
            {
                if (value != _thumbnailUrl)
                {
                    _thumbnailUrl = value;
                    OnPropertyChanged("ThumbnailUrl");
                }
            }
        }

        public int GuideId
        {
            get => _guideId;
            set
            {
                if (value != _guideId)
                {
                    _guideId = value;
                    OnPropertyChanged("GuideId");
                }
            }
        }

        public bool IsNewlyCreated
        {
            get => _isNewlyCreated;
            set
            {
                if (value != _isNewlyCreated)
                {
                    _isNewlyCreated = value;
                    OnPropertyChanged("IsFinished");
                }
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), Name, LocationId.ToString(), Description, Language, MaxGuests.ToString(),
                string.Join(",", PossibleDatesIds),
                Duration.ToString(), Began.ToString(), 
                string.Join(",", ImageId.Select(n => n.ToString()).ToArray()),
                ThumbnailUrl,
                GuideId.ToString(), IsNewlyCreated.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LocationId = Convert.ToInt32(values[2]);
            Description = values[3];
            Language = values[4];
            MaxGuests = Convert.ToInt32(values[5]);
            PossibleDatesIds = values[6].Split(',').Select(int.Parse).ToList();
            Duration = Convert.ToDouble(values[7]);
            Began = Convert.ToBoolean(values[8]);
            ImageId = values[9].Split(',').Select(int.Parse).ToList();
            ThumbnailUrl = values[10];
            GuideId = Convert.ToInt32(values[11]);
            IsNewlyCreated = Convert.ToBoolean(values[12]);
        }

        protected override void ValidateSelf()
        {
            NameRegex nameRegex = new NameRegex();
            LanguageRegex languageRegex = new LanguageRegex();
            NumberRegex numberRegex = new NumberRegex();
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                this.ValidationErrors["Name"] = "Name is required.";
            }
            else if (!nameRegex.Regex.IsMatch(this.Name))
            {
                this.ValidationErrors["Name"] = "You can only use letters and numbers in a name.";
            }
            if (string.IsNullOrWhiteSpace(this.Description))
            {
                this.ValidationErrors["Description"] = "Description cannot be empty.";
            }
            if (string.IsNullOrWhiteSpace(this.Language))
            {
                this.ValidationErrors["Language"] = "Language cannot be empty.";
            }
            else if (!languageRegex.Regex.IsMatch(this.Language))
            {
                this.ValidationErrors["Language"] = "You can only use letters in a language.";
            }
            if (string.IsNullOrWhiteSpace(this.MaxGuests.ToString()) || this.MaxGuests == 0)
            {
                this.ValidationErrors["Maximum Guests"] = "Maximum Guests is required.";
            }
            else if (!numberRegex.Regex.IsMatch(this.MaxGuests.ToString()))
            {
                this.ValidationErrors["Maximum Guests"] = "You can only use numbers.";
            }
            if (string.IsNullOrWhiteSpace(this.Duration.ToString()) || string.Equals(this.Duration.ToString(), "0"))
            {
                this.ValidationErrors["Duration"] = "Duration is required.";
            }
            else if (!numberRegex.Regex.IsMatch(this.Duration.ToString()))
            {
                this.ValidationErrors["Duration"] = "You can only use numbers.";
            }
            if (string.IsNullOrWhiteSpace(this.ThumbnailUrl))
            {
                this.ValidationErrors["ThumbnailUrl"] = "Thumbnail Image is required.";
            }
        }
    }
}