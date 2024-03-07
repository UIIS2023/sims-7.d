using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Model;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILocationRepository
    {
        public List<Location> GetAll();
        public Location Save(Location location);
        public int NextId();
        public void Delete(Location location);
        public Location Update(Location location);
        public bool IsAlreadyInserted(Location location);
        public int GetIdByCityAndCountry(string city, string country);
        public Location GetById(int id);
    }
}
