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

    public class LocationRepository : ILocationRepository
    {
        private const string FilePath = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Location> _serializer;

        private List<Location> locations;

        public LocationRepository()
        {

            _serializer = new Serializer<Location>();
            locations = _serializer.FromCSV(FilePath);

        }

        public List<Location> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }



        public Location Save(Location location)
        {
            location.Id = NextId();
            locations = _serializer.FromCSV(FilePath);
            locations.Add(location);
            _serializer.ToCSV(FilePath, locations);
            return location;
        }

        public int NextId()
        {
            locations = _serializer.FromCSV(FilePath);
            if (locations.Count < 1)
            {
                return 1;
            }
            return locations.Max(c => c.Id) + 1;
        }

        public void Delete(Location location)
        {
            locations = _serializer.FromCSV(FilePath);
            Location founded = locations.Find(c => c.Id == location.Id);
            locations.Remove(founded);
            _serializer.ToCSV(FilePath, locations);
        }

        public Location Update(Location location)
        {
            locations = _serializer.FromCSV(FilePath);
            Location current = locations.Find(c => c.Id == location.Id);
            int index = locations.IndexOf(current);
            locations.Remove(current);
            locations.Insert(index, location);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, locations);
            return location;
        }

        public bool IsAlreadyInserted(Location location)     //checks if there is already a location with same country and city
        {
            foreach (var iteration in locations)
            {
                if (location.City == iteration.City && location.Country == iteration.Country)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetIdByCityAndCountry(string city, string country)
        {
            foreach (Location iteration in locations)
            {
                if (city == iteration.City && country == iteration.Country)
                {
                    return iteration.Id;
                }
            }

            return -1;
        }


        public Location GetById(int id)
        {
            foreach (var location in locations)
            {
                if (location.Id == id) return location;
            }
            return null;
        }
    }
}