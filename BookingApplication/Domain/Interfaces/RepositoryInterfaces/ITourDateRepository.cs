using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface ITourDateRepository
    {
        public List<TourDate> GetAll();
        public int NextId();
        public TourDate Save(TourDate tourDate);
        public void Delete(TourDate tourDate);
        public TourDate Update(TourDate tourDate);
        public TourDate GetById(int id);
    }
}
