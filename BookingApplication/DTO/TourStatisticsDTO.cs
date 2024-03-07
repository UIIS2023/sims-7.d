using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.DTO
{
    public class TourStatisticsDTO
    {
        private int _tourId;
        private string _name;
        private string _language;
        private double _duration;
        private int _guestNumber;
        private string _thumbnailUrl;

        public TourStatisticsDTO(){}

        public TourStatisticsDTO(int tourId, string name, string language, double duration, int guestNumber,
            string thumbnailUrl)
        {
            TourId = tourId;
            Name = name;
            Language = language;
            Duration = duration;
            GuestNumber = guestNumber;
            ThumbnailUrl = thumbnailUrl;
        }
        
        public int TourId
        {
            get => _tourId;
            set => _tourId = value;
        }
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Language
        {
            get => _language;
            set => _language = value;
        }
        public double Duration
        {
            get => _duration;
            set => _duration = value;
        }
        public int GuestNumber
        {
            get => _guestNumber;
            set => _guestNumber = value;
        }
        public string ThumbnailUrl
        {
            get => _thumbnailUrl;
            set => _thumbnailUrl = value;
        }
    }
}
