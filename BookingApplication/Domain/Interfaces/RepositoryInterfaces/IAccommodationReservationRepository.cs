using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface IAccommodationReservationRepository
    {
        public List<AccommodationReservation> GetAll();
        public AccommodationReservation Save(AccommodationReservation accommodationReservation);
        public int NextId();
        public void Delete(AccommodationReservation accommodationReservation);
        public AccommodationReservation Update(AccommodationReservation accommodationReservation);

       
    }
}
