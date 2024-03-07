using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface IAccommodationRenovationRepository
    {
        public List<AccommodationRenovation> GetAll();
        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation);
        public int NextId();
        public void Delete(AccommodationRenovation accommodationRenovation);
        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation);
    }
}
