using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Model;

namespace BookingApplication.DTO
{
    public class TourRequestFilterDTO
    {
        private int? _maxGuestsNumber;
        private int? _minGuestsNumber;
        private string _language;
        private Location _tourLocation;
        private DateTime _startDate;
        private DateTime _endDate;

        public int? MaxGuestsNumber
        {
            get => _maxGuestsNumber;
            set => _maxGuestsNumber = value;
        }
        public int? MinGuestsNumber
        {
            get => _minGuestsNumber;
            set => _minGuestsNumber = value;
        }
        public string Language
        {
            get => _language;
            set => _language = value;
        }
        public Location TourLocation
        {
            get => _tourLocation;
            set => _tourLocation = value;
        }
        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = value;
        }
        public DateTime EndDate
        {
            get => _endDate;
            set => _endDate = value;
        }

        public TourRequestFilterDTO(){}

        public TourRequestFilterDTO(int maxGuestsNumber, int minGuestNumber,string language, Location location, DateTime startDate, DateTime endDate)
        {
            MaxGuestsNumber = maxGuestsNumber;
            MinGuestsNumber = minGuestNumber;
            Language = language;
            TourLocation = location;
            StartDate = startDate;
            EndDate = endDate;
        }

    }
}
