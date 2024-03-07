using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.DTO;
using BookingApplication.Serializer;
using BookingApplication.Repository;

namespace BookingApplication.Services
{
    public class OwnerRateService
    {
        private readonly IOwnerRatingRepository _ownerRatingRepository;
        
        

        public OwnerRateService()
        {
            _ownerRatingRepository = Injector.CreateInstance<IOwnerRatingRepository>();
            
        }

        public List<OwnerRating> GetAll()
        {
            return _ownerRatingRepository.GetAll();
        }


        public OwnerRating Save(OwnerRating ownerRating)
        {
            return _ownerRatingRepository.Save(ownerRating);
        }
        public bool AccomodationIsRated(int guestId, int accommodationId)
        {
            return _ownerRatingRepository.GetAll().Any(x => x.GuestId == guestId && x.AccommodationId == accommodationId);
        }
    }
}
