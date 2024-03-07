using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;

namespace BookingApplication.Services
{
    public class TourNotificationService
    {
        private readonly ITourNotificationRepository _tourNotificationRepository;

        public TourNotificationService()
        {
            _tourNotificationRepository = Injector.CreateInstance<ITourNotificationRepository>();
        }

        public void DeleteTourNotification(TourNotification tourNotification)
        {
            _tourNotificationRepository.Delete(tourNotification);
        }

        public void SaveTourNotification(TourNotification tourNotification)
        {
            _tourNotificationRepository.Save(tourNotification);
        }

        public TourNotification UpdateTourNotification(TourNotification tourNotification)
        {
            return _tourNotificationRepository.Update(tourNotification);
        }

        public TourNotification GetById(int id)
        {
            return _tourNotificationRepository.GetById(id);
        }

        public List<TourNotification> GetAllTourNotifications()
        {
            return _tourNotificationRepository.GetAll();
        }

        public void SendNotifications(List<TourReservation> tourReservations)
        {
            foreach (var tourReservation in tourReservations)
            {
                TourNotification tourNotification = new TourNotification(tourReservation.Id);
                _tourNotificationRepository.Save(tourNotification);
            }
        }
        
    }
}
