using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Serializer;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using BookingApplication.Domain.Model;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Repository
{

    public class AccommodationRepository : IAccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accommodations;

        private LocationRepository _locationRepository = new LocationRepository();

        public AccommodationRepository()
        {

            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCSV(FilePath);

        }

        public List<Accommodation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.Add(accommodation);
            _serializer.ToCSV(FilePath, _accommodations);
            return accommodation;
        }

        public int NextId()
        {
            _accommodations = _serializer.FromCSV(FilePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(ac => ac.Id) + 1;
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation founded = _accommodations.Find(ac => ac.Id == accommodation.Id);
            _accommodations.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodations);
        }
        public Accommodation Update(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation current = _accommodations.Find(ac => ac.Id == accommodation.Id);
            int index = _accommodations.IndexOf(current);
            _accommodations.Remove(current);
            _accommodations.Insert(index, accommodation);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _accommodations);

            return accommodation;
        }

        public ObservableCollection<Accommodation> FilterAccommodations(int? minReservationDays, AccommodationType type, int? maxGuest, string name, string city, string country)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            ObservableCollection<Accommodation> filteredAccommodations = new ObservableCollection<Accommodation>();

            foreach (var accommodation in _accommodations)
            {
                bool isName = string.IsNullOrEmpty(name) || name == accommodation.Name;
                bool isMinReservationDays = minReservationDays is null || minReservationDays < accommodation.MinReservationDays;
                bool isMaxGuest = maxGuest is null || maxGuest <= accommodation.MaxGuests;
                bool isLocation = IsAccommodationOnLocation(country, city, accommodation);
                bool isAccommodationType = ((int)(type)) == -1 || type == accommodation.AccommodationType;

                if (isLocation && isMaxGuest && isMinReservationDays && isLocation && isAccommodationType && isName)
                {
                    filteredAccommodations.Add(accommodation);
                }
            }
            return filteredAccommodations;
        }


        public bool IsAccommodationOnLocation(string country, string city, Accommodation accommodation)
        {
            var locationRepository = new LocationRepository();
            Location location = locationRepository.GetById(accommodation.LocationId);
            return (location.Country == country || String.IsNullOrEmpty(country)) && (location.City == city || String.IsNullOrEmpty(city));

        }


    }













}