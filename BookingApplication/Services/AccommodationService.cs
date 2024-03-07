using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Services
{
    internal class AccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository;
       

        public AccommodationService()
        {
            _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
        }

        public Accommodation Save(Accommodation accommodation)
        {
            return _accommodationRepository.Save(accommodation);
        }

       

    }
}
