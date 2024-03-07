using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface ITourRepository
    {
        public List<Tour> GetAll();
        public Tour Save(Tour tour);
        public int NextId();
        public void Delete(Tour tour);
        public Tour Update(Tour tour);
        public Tour GetById(int id);
        public void BindTourPoints();
    }
}
