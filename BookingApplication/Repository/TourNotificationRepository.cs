using BookingApplication.Domain.Model;
using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Repository
{
    class TourNotificationRepository : ITourNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/tourNotifications.csv";

        private readonly Serializer<TourNotification> _serializer;

        private List<TourNotification> _tourNotifications;

        private readonly TourNotificationRepository _tourNotificationRepository;

        public TourNotificationRepository()
        {
            _serializer = new Serializer<TourNotification>();
            _tourNotifications = _serializer.FromCSV(FilePath);
        }

        public List<TourNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _tourNotifications = _serializer.FromCSV(FilePath);
            if (_tourNotifications.Count < 1)
            {
                return 1;
            }
            return _tourNotifications.Max(c => c.Id) + 1;
        }

        public TourNotification Save(TourNotification tourNotification)
        {
            tourNotification.Id = NextId();
            _tourNotifications = _serializer.FromCSV(FilePath);
            _tourNotifications.Add(tourNotification);
            _serializer.ToCSV(FilePath, _tourNotifications);
            return tourNotification;
        }

        public void Delete(TourNotification tourNotification)
        {
            _tourNotifications = _serializer.FromCSV(FilePath);
            TourNotification founded = _tourNotifications.Find(c => c.Id == tourNotification.Id);
            _tourNotifications.Remove(founded);
            _serializer.ToCSV(FilePath, _tourNotifications);
        }

        public TourNotification Update(TourNotification tourNotification)
        {
            _tourNotifications = _serializer.FromCSV(FilePath);
            TourNotification current = _tourNotifications.Find(c => c.Id == tourNotification.Id);
            int index = _tourNotifications.IndexOf(current);
            _tourNotifications.Remove(current);
            _tourNotifications.Insert(index, tourNotification);
            _serializer.ToCSV(FilePath, _tourNotifications);
            return tourNotification;
        }

        public TourNotification GetById(int id)
        {
            _tourNotifications = _serializer.FromCSV(FilePath);
            return _tourNotifications.Find(c => c.Id == id);
        }
    }

}
