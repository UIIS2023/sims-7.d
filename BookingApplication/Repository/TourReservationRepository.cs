using BookingApplication.Domain.Model;
using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Repository
{
    public class TourReservationRepository : ITourReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/toursReservations.csv";

        private readonly Serializer<TourReservation> _serializer;

        private List<TourReservation> _toursReservations;

        public TourReservationRepository()
        {

            _serializer = new Serializer<TourReservation>();
            _toursReservations = _serializer.FromCSV(FilePath);

        }

        public List<TourReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _toursReservations = _serializer.FromCSV(FilePath);
            if (_toursReservations.Count < 1)
            {
                return 1;
            }
            return _toursReservations.Max(c => c.Id) + 1;
        }

        public TourReservation Save(TourReservation tourReservation)
        {
            tourReservation.Id = NextId();
            _toursReservations = _serializer.FromCSV(FilePath);
            _toursReservations.Add(tourReservation);
            _serializer.ToCSV(FilePath, _toursReservations);
            return tourReservation;
        }

        public void Delete(TourReservation tourReservation)
        {
            _toursReservations = _serializer.FromCSV(FilePath);
            TourReservation founded = _toursReservations.Find(c => c.UserId == tourReservation.UserId && c.TourId == tourReservation.TourId);
            _toursReservations.Remove(founded);
            _serializer.ToCSV(FilePath, _toursReservations);
        }

        public TourReservation Update(TourReservation tourReservation)
        {
            _toursReservations = _serializer.FromCSV(FilePath);
            TourReservation current = _toursReservations.Find(c => c.UserId == tourReservation.UserId && c.TourId == tourReservation.TourId);
            int index = _toursReservations.IndexOf(current);
            _toursReservations.Remove(current);
            _toursReservations.Insert(index, tourReservation);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _toursReservations);
            return tourReservation;
        }

    }

}
