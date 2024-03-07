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

    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/acc_reservation.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservation;

        private AccommodationRepository _accommodationRepository;

        public AccommodationReservationRepository()
        {

            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservation = _serializer.FromCSV(FilePath);
            _accommodationRepository = new AccommodationRepository();

        }

        public List<AccommodationReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.ReservationId = NextId();
            _accommodationReservation = _serializer.FromCSV(FilePath);
            _accommodationReservation.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservation);
            return accommodationReservation;
        }

        public int NextId()
        {
            _accommodationReservation = _serializer.FromCSV(FilePath);
            if (_accommodationReservation.Count < 1)
            {
                return 1;
            }

            return _accommodationReservation.Max(ac => ac.ReservationId) + 1;
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            _accommodationReservation = _serializer.FromCSV(FilePath);
            AccommodationReservation founded = _accommodationReservation.Find(ac => ac.ReservationId == accommodationReservation.ReservationId);
            _accommodationReservation.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationReservation);
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            _accommodationReservation = _serializer.FromCSV(FilePath);
            AccommodationReservation current = _accommodationReservation.Find(ac => ac.ReservationId == accommodationReservation.ReservationId);
            int index = _accommodationReservation.IndexOf(current);
            _accommodationReservation.Remove(current);
            _accommodationReservation.Insert(index, accommodationReservation); // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _accommodationReservation);

            return accommodationReservation;
        }

        /*

        public List<Tuple<DateTime, DateTime>> GetAvailableDates(Accommodation selectedAccommodation, int daysForRenovation, DateTime startDate, DateTime endDate)
        {
            List<DateTime> reservedDates = GetReservedDates(selectedAccommodation);
            List<DateTime> availableDates = new List<DateTime>();
            List<Tuple<DateTime, DateTime>> availableDatesCombo = new List<Tuple<DateTime, DateTime>>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!reservedDates.Contains(date))
                {
                    availableDates.Add(date);
                }
                else
                {
                    availableDates.Clear();
                }

                if (availableDates.Count == daysForRenovation)
                {
                    availableDatesCombo.Add(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date));
                    availableDates.RemoveAt(0);
                }
            }
            return availableDatesCombo;
        }

        public List<DateTime> GetReservedDates(Accommodation selectedAccommodation)
        {
            List<DateTime> reservedDates = new List<DateTime>();

            foreach (AccommodationReservation reservation in _accommodationReservation)
            {
                if (selectedAccommodation.Id == reservation.AccommodationId)
                {
                    DateTime start = reservation.StartDate;
                    DateTime end = reservation.EndDate;

                    for (DateTime currentDate = start; currentDate <= end; currentDate = currentDate.AddDays(1))
                    {
                        reservedDates.Add(currentDate);
                    }
                }
            }
            return reservedDates;
        }
        */

    }

}