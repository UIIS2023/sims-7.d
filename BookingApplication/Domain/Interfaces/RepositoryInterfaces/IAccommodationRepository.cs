using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Model;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface IAccommodationRepository
    {
        public List<Accommodation> GetAll();
        public Accommodation Save(Accommodation accommodation);
        public int NextId();
        public void Delete(Accommodation accommodation);
        public Accommodation Update(Accommodation accommodation);
        public ObservableCollection<Accommodation> FilterAccommodations(int? minReservationDays, AccommodationType type, int? maxGuest, string name, string city, string country);
        public bool IsAccommodationOnLocation(string country, string city, Accommodation accommodation);
    }
}
