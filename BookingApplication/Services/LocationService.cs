using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Serializer;

namespace BookingApplication.Services
{
    public class LocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService()
        {
            _locationRepository = Injector.CreateInstance<ILocationRepository>();
        }

        public List<Location> GetAll()
        {
            return _locationRepository.GetAll();
        }

        public Location SaveLocation(Location location)
        {
            return _locationRepository.Save(location);
        }

        public Location GetById(int id)
        {
            return _locationRepository.GetById(id);
        }
        public Location UpdateLocation(Location location)
        {
            return _locationRepository.Update(location);
        }

        public Dictionary<string, List<string>> GetLocations()
        {
            var locations = _locationRepository.GetAll();
            Dictionary<string, List<string>> countryCities = locations
                .GroupBy(l => l.Country)
                .ToDictionary(g => g.Key, g => g.Select(l => l.City).ToList());
            return countryCities;
        }

        public bool IsTourOnLocation(Location location, Tour tour)
        {
            var tourLocation = _locationRepository.GetById(tour.LocationId);
            return (tourLocation.Country == location.Country || string.IsNullOrEmpty(location.Country)) &&
                   (tourLocation.City == location.City || string.IsNullOrEmpty(location.City));

        }

        public bool IsTourRequestOnLocation(Location location, SimpleTourRequest tourRequest)
        {
            var tourRequestLocation = _locationRepository.GetById(tourRequest.LocationId.Value);
            if (location == null)
            {
                return true;
            }
            return (tourRequestLocation.Country == location.Country || string.IsNullOrWhiteSpace(location.Country)) &&
                   (tourRequestLocation.City == location.City || string.IsNullOrWhiteSpace(location.City));
        }
        public bool IsAlreadyInserted(Location location)
        {
            foreach (var iteration in GetAll())
            {
                if (location.City == iteration.City /*&& location.Country == iteration.Country*/)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetIdByCityAndCountry(string city, string country)
        {
            foreach (Location iteration in GetAll())
            {
                if (city == iteration.City && country == iteration.Country)
                {
                    return iteration.Id;
                }
            }

            return -1;
        }

        public Location ConfigureLocation(Location location)
        {
            //checking if we already have this location stored in data
            if (IsAlreadyInserted(location))
            {
                location.Id = GetIdByCityAndCountry(location.City,
                    location.Country);
            }
            else
            {
                location = SaveLocation(location);
            }

            return location;
        }

        public List<SimpleTourRequest> GetLocationsForRequests(List<SimpleTourRequest> requests)
        {
            foreach (var request in requests)
            {
                request.Location = GetById((int)request.LocationId);
            }
            return requests;
        }

        public int GetRequestNumberForLocation(Location location, ObservableCollection<SimpleTourRequest> requests, int userId)
        {
            return requests.Count(request => IsTourRequestOnLocation(location, request) && request.UserId == userId);
        }
        public List<string> GetCitiesByCountry(string country)
        {
            return _locationRepository.GetAll().Where(x => x.Country.Equals(country)).Select(x => x.City).ToList();
        }
    }
}
