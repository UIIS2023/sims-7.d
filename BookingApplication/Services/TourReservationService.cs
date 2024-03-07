using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Services
{
    public class TourReservationService
    {
        private readonly ITourReservationRepository _tourReservationRepository;
        private readonly  TourDateService _tourDateService;

        public TourReservationService()
        {
            _tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            _tourDateService = new TourDateService();
        }

        public void DeleteTourReservation(TourReservation tourReservation)
        {
            _tourReservationRepository.Delete(tourReservation);
        }

        public void SaveTourReservation(TourReservation tourReservation)
        {
            _tourReservationRepository.Save(tourReservation);
        }

        public TourReservation UpdateTourReservation(TourReservation tourReservation)
        {
            return _tourReservationRepository.Update(tourReservation);
        }

        public List<TourReservation> GetAllTourReservations()
        {
            return _tourReservationRepository.GetAll();
        }

        public List<TourReservation> GetReservationsByTour(Tour tour)
        {
            List<TourReservation> tourReservations = new List<TourReservation>();
            foreach (var reservation in GetAllTourReservations())
            {
                if (reservation.TourId == tour.Id)
                {
                    tourReservations.Add(reservation);
                }
            }
            return tourReservations;
        }

        public List<TourReservation> GetUncheckedReservationsByTour(Tour tour)
        {
            List<TourReservation> tourReservations = new List<TourReservation>();
            foreach (var reservation in GetAllTourReservations())
            {
                if (reservation.TourId == tour.Id && reservation.IsChecked == false)
                {
                    tourReservations.Add(reservation);
                }
            }
            return tourReservations;
        }

        public List<TourReservation> GetReservationsByTourDate(TourDate tourDate)
        {
            List<TourReservation> tourReservations = new List<TourReservation>();
            foreach (TourReservation tourReservation in GetAllTourReservations())
            {
                if(DateTime.Compare(tourReservation.Date, tourDate.StartTime)==0)
                    tourReservations.Add(tourReservation);
            }
            return tourReservations;
        }
        
        

        public void StartTourReservations(Tour tour)
        {
            foreach (TourDate tourDate in _tourDateService.GetTourDatesByTour(tour))
            {
                foreach (TourReservation tourReservation in GetAllTourReservations())
                {
                    if (tourReservation.TourId == tour.Id &&
                        DateTime.Compare(tourReservation.Date, tourDate.StartTime) == 0)
                    {
                        tourReservation.HasStarted = true;
                    }
                }
            }
        }
        public void EndTourReservations(Tour tour)
        {
            foreach (TourDate tourDate in _tourDateService.GetTourDatesByTour(tour))
            {
                foreach (TourReservation tourReservation in GetAllTourReservations())
                {
                    if (tourReservation.TourId == tour.Id &&
                        DateTime.Compare(tourReservation.Date, tourDate.StartTime) == 0)
                    {
                        tourReservation.IsCompleted = true;
                        _tourReservationRepository.Update(tourReservation);
                    }
                }
            }
        }

        public List<TourReservation> GetFinishedReservations()
        {
            List<TourReservation> tourReservations = new List<TourReservation>();
            foreach (TourReservation tourReservation in GetAllTourReservations())
            {
                if (tourReservation.IsCompleted)
                {
                    tourReservations.Add(tourReservation);
                }
            }
            return tourReservations;
        }

        public void DeleteTourReservations(Tour tour)
        {
            foreach (TourDate tourDate in _tourDateService.GetTourDatesByTour(tour))
            {
                foreach (TourReservation tourReservation in GetAllTourReservations())
                {
                    if (tourReservation.TourId == tour.Id &&
                        DateTime.Compare(tourReservation.Date, tourDate.StartTime) == 0)
                    {
                        DeleteTourReservation(tourReservation);
                    }
                }
            }
        }

        public List<Tour> GetCompletedToursForUser(int userId)
        {
            var tourService = new TourService();
            var completedTours = new List<Tour>();
            foreach (var tour in _tourReservationRepository.GetAll())
            {
                if(tour.IsCompleted && tour.UserId == userId)
                    completedTours.Add(tourService.GetById(tour.TourId));
            }
            return completedTours;
        }

       

        public List<TourReservation> GetReservationsForUser(int userId)
        {
            var reservations = new List<TourReservation>();
            foreach (var tourReservation in _tourReservationRepository.GetAll())
            {
                if (tourReservation.UserId == userId)
                    reservations.Add(tourReservation);
            }
            return reservations;
        }

        public List<Tour> GetBookedToursFromReservations(ObservableCollection<TourReservation> reservations)
        {
            var tours = new List<Tour>();
            var tourService = new TourService();
            foreach (var reservation in reservations)
            {
                if(!reservation.IsCompleted)
                    tours.Add(tourService.GetById(reservation.TourId));
            }
            return tours;
        }

        public TourReservation GetReservationForUserAndTour(Tour tour, int userId)
        {
            foreach (var reservation in _tourReservationRepository.GetAll())
            {
                if (reservation.UserId == userId && reservation.TourId == tour.Id && tour.PossibleDates.Any(date => date.StartTime == reservation.Date))
                    return reservation;
            }
            return null;
        }

        public int GetGuestNumberByTourReservations(List<TourReservation> tourReservations)
        {
            int guestNumber = 0;
            foreach (var tourReservation in tourReservations)
            {
                guestNumber += tourReservation.GroupSize;
            }
            return guestNumber;
        }
    }
}
