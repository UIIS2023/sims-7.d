using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.DTO
{
    public class TourReviewDTO
    {
        private int _tourReviewId;
        private int _userId;
        private string _tourName;
        private string _userName;
        private string _tourRating;
        private bool _isValid;

        TourReviewDTO(){}

        public TourReviewDTO(int tourReviewId,string tourName, string userName, string tourRating, bool isValid)
        {
            TourReviewId = tourReviewId;
            TourName = tourName;
            UserName = userName;
            TourRating = tourRating;
            IsValid = isValid;
        }

        public int TourReviewId
        {
            get => _tourReviewId;
            set => _tourReviewId = value;
        }
        public string TourName
        {
            get => _tourName;
            set => _tourName = value;
        }

        public string UserName
        {
            get => _userName;
            set => _userName = value;
        }

        public string TourRating
        {
            get => _tourRating;
            set => _tourRating = value;
        }

        public bool IsValid
        {
            get => _isValid;
            set => _isValid = value;
        }
    }
}
