using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BookingApplication.Domain.Model;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.DTO;
using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using BookingApplication.Repository;

namespace BookingApplication.Services
{
    public class AccommodationRenovationService
    {
        private readonly IAccommodationRenovationRepository _accommodationRenovationRepository;
        private readonly AccommodationReservationService _accommodationReservationService;

        public AccommodationRenovationService()
        {
            _accommodationRenovationRepository = Injector.CreateInstance<IAccommodationRenovationRepository>();
            _accommodationReservationService = new AccommodationReservationService();

        }
        public List<AccommodationRenovation> GetAll()
        {
            return _accommodationRenovationRepository.GetAll();
        }
        public AccommodationRenovation SaveAccommodationRenovation(AccommodationRenovation accommodationRenovation)
        {
            return _accommodationRenovationRepository.Save(accommodationRenovation);
        }
       
        public AccommodationRenovation UpdateAccommodationRenovation(AccommodationRenovation accommodationRenovation)
        {
            return _accommodationRenovationRepository.Update(accommodationRenovation);
        }

        public void DeleteAccommodationRenovation(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovationRepository.Delete(accommodationRenovation);
        }


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

            foreach (AccommodationReservation reservation in _accommodationReservationService.GetAll())
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





    }
}
