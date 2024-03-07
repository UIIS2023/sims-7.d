using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface ITourPointRepository
    {
        public List<TourPoint> GetAll();
        public TourPoint Save(TourPoint tourPoint);
        public int NextId();
        public void Delete(TourPoint tourPoint);
        public TourPoint Update(TourPoint tourPoint);
        public void SaveAll(List<TourPoint> tourPoints);
        public TourPoint GetById(int id);
    }
}
