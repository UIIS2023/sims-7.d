using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using BookingApplication.Serializer;


namespace BookingApplication.Domain.Model
{
    public class TourReview : ISerializable, INotifyPropertyChanged
    {
        private int _id;
        private int _tourRating;
        private int _guideKnowledgeRating;
        private int _guideLanguageRating;
        private int _interestRating;
        private string _comment;
        private List<int> _imageId;
        private int _tourId;
        private int _tourPointId;
        private bool _isValid;
        private int _userId;

        public TourReview(){}

        public TourReview(int tourRating, int guideKnowledgeRating, int guideLanguageRating, int interestRating,
            string comment, List<int> imageId, int tourId, int tourPointId, bool isValid, int userId)
        {
            TourRating = tourRating;
            GuideKnowledgeRating = guideKnowledgeRating;
            GuideLanguageRating = guideLanguageRating;
            InterestRating = interestRating;
            Comment = comment;
            ImageId = imageId;
            TourPointId = tourPointId;
            IsValid = isValid;
            UserId = userId;
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

        public int TourRating
        {
            get => _tourRating;
            set
            {
                if (_tourRating != value)
                {
                    _tourRating = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GuideKnowledgeRating
        {
            get => _guideKnowledgeRating;
            set
            {
                if (_guideKnowledgeRating != value)
                {
                    _guideKnowledgeRating = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GuideLanguageRating
        {
            get => _guideLanguageRating;
            set
            {
                if (_guideLanguageRating != value)
                {
                    _guideLanguageRating = value;
                    OnPropertyChanged();
                }
            }
        }

        public int InterestRating
        {
            get => _interestRating;
            set
            {
                if (_interestRating != value)
                {
                    _interestRating = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged();
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

        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
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

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), TourRating.ToString(), GuideKnowledgeRating.ToString(),GuideLanguageRating.ToString(),
                InterestRating.ToString(), Comment, ImagesToString(ImageId), TourId.ToString(),
                TourPointId.ToString(), IsValid.ToString(), UserId.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourRating = Convert.ToInt32(values[1]);
            GuideKnowledgeRating = Convert.ToInt32(values[2]);
            GuideLanguageRating = Convert.ToInt32(values[3]);
            InterestRating = Convert.ToInt32(values[4]);
            Comment = values[5];
            ImageId = GetImages(values[6]);
            TourId = Convert.ToInt32(values[7]);
            TourPointId = Convert.ToInt32(values[8]);
            IsValid = Convert.ToBoolean(values[9]);
            UserId = Convert.ToInt32(values[10]);
        }

        private string ImagesToString(List<int> images)
        {
            string imagesString = "";
            foreach (var image in images)
            {
                if (image != images.Last())
                {
                    imagesString += image + ",";
                    continue;
                }
                imagesString += image;
            }

            return imagesString;
        }

        private List<int> GetImages(string imagesString)
        {
            List<int> images = new List<int>();
            string[] ids = imagesString.Split(',');
            foreach (var iterator in ids)
            {
                int.TryParse(iterator, out var id);
                images.Add(id);
            }

            return images;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
