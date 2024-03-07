using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Repository;
using BookingApplication.Domain.Model;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Repository
{
    public class TourDateRepository : ITourDateRepository
    { 
        private const string FilePath = "../../../Resources/Data/toursDates.csv";

        private readonly Serializer<TourDate> _serializer;

        private List<TourDate> _toursDates;

        private readonly TourRepository _tourRepository;

        public TourDateRepository()
        {

            _serializer = new Serializer<TourDate>();
            _toursDates = _serializer.FromCSV(FilePath);

        }

        public List<TourDate> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _toursDates = _serializer.FromCSV(FilePath);
            if (_toursDates.Count < 1)
            {
                return 1;
            }
            return _toursDates.Max(c => c.Id) + 1;
        }

        public TourDate Save(TourDate tourDate)
        {
            tourDate.Id = NextId();
            _toursDates = _serializer.FromCSV(FilePath);
            _toursDates.Add(tourDate);
            _serializer.ToCSV(FilePath, _toursDates);
            return tourDate;
        }

        public void Delete(TourDate tourDate)
        {
            _toursDates = _serializer.FromCSV(FilePath);
            TourDate founded = _toursDates.Find(c => c.Id == tourDate.Id );
            _toursDates.Remove(founded);
            _serializer.ToCSV(FilePath, _toursDates);
        }

        public TourDate Update(TourDate tourDate)
        {
            _toursDates = _serializer.FromCSV(FilePath);
            TourDate current = _toursDates.Find(c => c.Id == tourDate.Id);
            int index = _toursDates.IndexOf(current);
            _toursDates.Remove(current);
            _toursDates.Insert(index, tourDate);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _toursDates);
            return tourDate;
        }

        public TourDate GetById(int id)
        {
            _toursDates = _serializer.FromCSV(FilePath);
            return _toursDates.Find(c => c.Id == id);
        }



    }
}
