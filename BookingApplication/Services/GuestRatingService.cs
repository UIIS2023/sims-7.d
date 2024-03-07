using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Services
{
    public class GuestRatingByOwner
    {
        public Accommodation Accommodation { get; set; }
        public GuestRating Rating { get; set; }
    }

    internal class GuestRatingService
    {
        private readonly IGuestRatingRepository _guestRatingRepository;

        private readonly IUserRepository userRepository = new UserRepository();
        private readonly IAccommodationRepository accommodationRepository = new AccommodationRepository();
        private readonly OwnerRateService ownerRateService = new OwnerRateService();
        public GuestRatingService()
        {
            _guestRatingRepository = Injector.CreateInstance<IGuestRatingRepository>();
        }

        public List<GuestRating> GetAll()
        {
            return _guestRatingRepository.GetAll();
        }

        public GuestRating Save(GuestRating guestRating)
        {
            return _guestRatingRepository.Save(guestRating);
        }

        public List<GuestRatingByOwner> GetRatingsForUser(User guest)
        {
            List<GuestRating> guestRatings = _guestRatingRepository.GetAll().Where(x => x.GuestId == guest.Id).ToList();
            List<GuestRatingByOwner> ratingsByOwner = new List<GuestRatingByOwner>();

            guestRatings.ForEach(x =>
            {
                Accommodation accommodation = accommodationRepository.GetAll().FirstOrDefault(a => a.Id == x.AccommodationId);
                if (accommodation != null)
                {
                    if (ownerRateService.AccomodationIsRated(guest.Id, accommodation.Id))
                    {
                        ratingsByOwner.Add(new GuestRatingByOwner()
                        {
                            Accommodation = accommodation,
                            Rating = x
                        });
                    }
                }
            });

            return ratingsByOwner;
        }

    }
}

