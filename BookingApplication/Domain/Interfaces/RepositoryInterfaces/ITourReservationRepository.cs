using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface ITourReservationRepository
    {
        public List<TourReservation> GetAll();
        public TourReservation Save(TourReservation tourReservation);
        public void Delete(TourReservation tourReservation);
        public TourReservation Update(TourReservation tourReservation);

    }
}
