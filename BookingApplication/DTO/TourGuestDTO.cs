using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain;



namespace BookingApplication.DTO
{
    public class TourGuestDTO
    {
        private int _userId;
        private string _userName;
        private int _tourReservationId;
        private int _tourReservationGroupSize;

        public TourGuestDTO() { }
        public TourGuestDTO(int userId, string userName, int tourReservationId, int tourReservationGroupSize)
        {
            UserId = userId;
            UserName = userName;
            TourReservationId = tourReservationId;
            TourReservationGroupSize = tourReservationGroupSize;
        }

        public int UserId
        {
            get => _userId;
            set => _userId = value;
        }

        public string UserName
        {
            get => _userName;
            set => _userName = value;
        }

        public int TourReservationId
        {
            get => _tourReservationId;
            set => _tourReservationId = value;
        }

        public int TourReservationGroupSize
        {
            get => _tourReservationGroupSize;
            set => _tourReservationGroupSize = value;
        }

    }
}
