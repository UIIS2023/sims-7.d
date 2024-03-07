using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Repository;

namespace BookingApplication
{

    public class Injector
        {
            private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
            {
                { typeof(IUserRepository), new UserRepository() },
                { typeof(ICommentRepositoy), new CommentRepository() },
                { typeof(IAccommodationRepository), new AccommodationRepository() },
                { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
                { typeof(IAccommodationRenovationRepository), new AccommodationRenovationRepository() },
                { typeof(IGuestRepository), new GuestRepository() },
                { typeof(IGuestRatingRepository), new GuestRatingRepository() },
                { typeof(IImageRepository), new ImageRepository() },
                { typeof(IOwnerRepository), new OwnerRepository() },
                { typeof(IOwnerRatingRepository), new OwnerRatingRepository() },
                { typeof(ILocationRepository), new LocationRepository() },
                { typeof(ITourDateRepository), new TourDateRepository() },
                { typeof(ITourPointRepository), new TourPointRepository() },
                { typeof(ITourRepository), new TourRepository() },
                { typeof(IVoucherRepository), new VoucherRepository() },
                { typeof(ITourNotificationRepository), new TourNotificationRepository() },
                { typeof(ITourReviewRepository), new TourReviewRepository() },
                { typeof(ITourReservationRepository), new TourReservationRepository() },
                { typeof(ISimpleTourRequestRepository), new SimpleTourRequestRepository()}


                 
                // Add more implementations here
            };

            public static T CreateInstance<T>()
            {
                Type type = typeof(T);

                if (_implementations.ContainsKey(type))
                {
                    return (T)_implementations[type];
                }

                throw new ArgumentException($"No implementation found for type {type}");
            }
        }
    
}
