using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface ITourNotificationRepository
    {
        public List<TourNotification> GetAll();
        public int NextId();
        public TourNotification Save(TourNotification tourNotification);
        public void Delete(TourNotification tourNotification);
        public TourNotification Update(TourNotification tourNotification);
        public TourNotification GetById(int id);
    }
}
