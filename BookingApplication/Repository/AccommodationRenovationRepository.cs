using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingApplication.Repository
{
    public class AccommodationRenovationRepository : IAccommodationRenovationRepository
    {

        private const string FilePath = "../../../Resources/Data/accommodationRenovations.csv";

        private readonly Serializer<AccommodationRenovation> _serializer;

        private List<AccommodationRenovation> _accommodationRenovation;

        public AccommodationRenovationRepository()
        {

            _serializer = new Serializer<AccommodationRenovation>();
            _accommodationRenovation = _serializer.FromCSV(FilePath);
         

        }

        public List<AccommodationRenovation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation)
        {
            accommodationRenovation.RenovationId = NextId();
            _accommodationRenovation = _serializer.FromCSV(FilePath);
            _accommodationRenovation.Add(accommodationRenovation);
            _serializer.ToCSV(FilePath, _accommodationRenovation);
            return accommodationRenovation;
        }

        public int NextId()
        {
            _accommodationRenovation = _serializer.FromCSV(FilePath);
            if (_accommodationRenovation.Count < 1)
            {
                return 1;
            }

            return _accommodationRenovation.Max(ac => ac.RenovationId) + 1;
        }

        public void Delete(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovation = _serializer.FromCSV(FilePath);
            AccommodationRenovation founded = _accommodationRenovation.Find(ac => ac.RenovationId == accommodationRenovation.RenovationId);
            _accommodationRenovation.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationRenovation);
        }

        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovation = _serializer.FromCSV(FilePath);
            AccommodationRenovation current = _accommodationRenovation.Find(ac => ac.RenovationId == accommodationRenovation.RenovationId);
            int index = _accommodationRenovation.IndexOf(current);
            _accommodationRenovation.Remove(current);
            _accommodationRenovation.Insert(index, accommodationRenovation); // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _accommodationRenovation);

            return accommodationRenovation;
        }


    }
}
