using BookingApplication.Domain.Model;
using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Repository
{
    public class TourPointRepository : ITourPointRepository
    {
        private const string FilePath = "../../../Resources/Data/tour_points.csv";

        private readonly Serializer<TourPoint> _serializer;

        private List<TourPoint> _tourPoints;

        public TourPointRepository()
        {

            _serializer = new Serializer<TourPoint>();
            _tourPoints = _serializer.FromCSV(FilePath);

        }

        public List<TourPoint> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourPoint Save(TourPoint tourPoint)
        {
            tourPoint.Id = NextId();
            _tourPoints = _serializer.FromCSV(FilePath);
            _tourPoints.Add(tourPoint);
            _serializer.ToCSV(FilePath, _tourPoints);
            return tourPoint;
        }

        public int NextId()
        {
            _tourPoints = _serializer.FromCSV(FilePath);
            if (_tourPoints.Count < 1)
            {
                return 1;
            }
            return _tourPoints.Max(c => c.Id) + 1;
        }

        public void Delete(TourPoint tourPoint)
        {
            _tourPoints = _serializer.FromCSV(FilePath);
            TourPoint founded = _tourPoints.Find(c => c.Id == tourPoint.Id);
            _tourPoints.Remove(founded);
            _serializer.ToCSV(FilePath, _tourPoints);
        }

        public TourPoint Update(TourPoint tourPoint)
        {
            _tourPoints = _serializer.FromCSV(FilePath);
            TourPoint current = _tourPoints.Find(c => c.Id == tourPoint.Id);
            int index = _tourPoints.IndexOf(current);
            _tourPoints.Remove(current);
            _tourPoints.Insert(index, tourPoint);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _tourPoints);
            return tourPoint;
        }

        public void SaveAll(List<TourPoint> tourPoints)
        {
            _serializer.ToCSV(FilePath,tourPoints);
        }

        public TourPoint GetById(int id)
        {
            return _tourPoints.Find(c => c.Id == id);
        }


    }
    
}
