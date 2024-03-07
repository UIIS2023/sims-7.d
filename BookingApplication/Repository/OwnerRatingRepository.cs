using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingApplication.Repository
{
    public class OwnerRatingRepository : IOwnerRatingRepository
    {

        private const string FilePath = "../../../Resources/Data/ownerRatings.csv";

        private readonly Serializer<OwnerRating> _serializer;

        private List<OwnerRating> _ownerRatings;


        public OwnerRatingRepository()
        {
            _serializer = new Serializer<OwnerRating>();
            _ownerRatings = _serializer.FromCSV(FilePath);
        }

        public List<OwnerRating> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public OwnerRating Save(OwnerRating ownerRating)
        {
            ownerRating.Id = NextId();
            _ownerRatings = _serializer.FromCSV(FilePath);
            _ownerRatings.Add(ownerRating);
            _serializer.ToCSV(FilePath, _ownerRatings);
            return ownerRating;
        }

        public int NextId()
        {
            _ownerRatings = _serializer.FromCSV(FilePath);
            if (_ownerRatings.Count < 1)
            {
                return 1;
            }
            return _ownerRatings.Max(c => c.Id) + 1;
        }

        public void Delete(OwnerRating ownerRating)
        {
            _ownerRatings = _serializer.FromCSV(FilePath);
            OwnerRating founded = _ownerRatings.Find(c => c.Id == ownerRating.Id);
            _ownerRatings.Remove(founded);
            _serializer.ToCSV(FilePath, _ownerRatings);
        }

        public OwnerRating Update(OwnerRating ownerRating)
        {
            _ownerRatings = _serializer.FromCSV(FilePath);
            OwnerRating current = _ownerRatings.Find(c => c.Id == ownerRating.Id);
            int index = _ownerRatings.IndexOf(current);
            _ownerRatings.Remove(current);
            _ownerRatings.Insert(index, ownerRating);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _ownerRatings);
            return ownerRating;
        }




    }
}
