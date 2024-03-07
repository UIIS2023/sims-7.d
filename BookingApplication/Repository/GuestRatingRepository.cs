using BookingApplication.Domain.Model;
using BookingApplication.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Repository
{
    public class GuestRatingRepository : IGuestRatingRepository
    {

        private const string FilePath = "../../../Resources/Data/guestRatings.csv";

        private readonly Serializer<GuestRating> _serializer;

        private List<GuestRating> _guestRatings;

        public GuestRatingRepository()
        {
            _serializer = new Serializer<GuestRating>();
            _guestRatings = _serializer.FromCSV(FilePath);
        }

        public List<GuestRating> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public GuestRating Save(GuestRating guestRating)
        {
            guestRating.Id = NextId();
            _guestRatings = _serializer.FromCSV(FilePath);
            _guestRatings.Add(guestRating);
            _serializer.ToCSV(FilePath, _guestRatings);
            return guestRating;
        }

        public int NextId()
        {
            _guestRatings = _serializer.FromCSV(FilePath);
            if (_guestRatings.Count < 1)
            {
                return 1;
            }
            return _guestRatings.Max(c => c.Id) + 1;
        }

        public void Delete(GuestRating guestRating)
        {
            _guestRatings = _serializer.FromCSV(FilePath);
            GuestRating founded = _guestRatings.Find(c => c.Id == guestRating.Id);
            _guestRatings.Remove(founded);
            _serializer.ToCSV(FilePath, _guestRatings);
        }

        public GuestRating Update(GuestRating guestRating)
        {
            _guestRatings = _serializer.FromCSV(FilePath);
            GuestRating current = _guestRatings.Find(c => c.Id == guestRating.Id);
            int index = _guestRatings.IndexOf(current);
            _guestRatings.Remove(current);
            _guestRatings.Insert(index, guestRating);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _guestRatings);
            return guestRating;
        }


      
    }
}
