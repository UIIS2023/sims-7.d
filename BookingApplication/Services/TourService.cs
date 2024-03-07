using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using BookingApplication.Domain.Model;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.DTO;

namespace BookingApplication.Services
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly TourReservationService _tourReservationService;
        private readonly TourDateService _tourDateService;
        public TourService()
        {
            _tourRepository = Injector.CreateInstance<ITourRepository>();
            _tourReservationService = new TourReservationService();
            _tourDateService = new TourDateService();
        }
        public List<Tour> GetAll()
        {
            return _tourDateService.GetPossibleDatesForTours(_tourRepository.GetAll());
        }
        public Tour SaveTour(Tour tour)
        {
            return _tourRepository.Save(tour);
        }
        public Tour GetById(int id)
        {
            return _tourRepository.GetById(id);
        }
        public Tour UpdateTour(Tour tour)
        {
            return _tourRepository.Update(tour);
        }

        public void DeleteTour(Tour tour)
        {
            _tourRepository.Delete(tour);
        }

        public ObservableCollection<Tour> FilterTours(TourFilterDto tourFilterDto)
        {
            var tours = _tourRepository.GetAll();
            var filteredTours = new ObservableCollection<Tour>();
            var locationService = new LocationService();

            foreach (var tour in tours)
            {
                var isLanguage = string.IsNullOrEmpty(tourFilterDto.Language) || tour.Language.ToLower().Contains(tourFilterDto.Language.ToLower());
                var isDuration = tourFilterDto.Duration is null || tourFilterDto.Duration == tour.Duration;
                var isGroupSize = tourFilterDto.GroupSize is null || tourFilterDto.GroupSize <= tour.MaxGuests;
                var isLocation = locationService.IsTourOnLocation(tourFilterDto.Location, tour);

                if (isLanguage && isDuration && isGroupSize && isLocation)
                {
                    filteredTours.Add(tour);
                }
            }
            return filteredTours;
        }
        public ObservableCollection<Tour> GetAlternativeToursOnLocation(int locationId, int tourId)
        {
            var tours = new ObservableCollection<Tour>();
            foreach (var tour in _tourRepository.GetAll())
            {
                if (tour.LocationId == locationId && tour.Id != tourId)
                {
                    tours.Add(tour);
                }
            }
            return tours;
        }

        public void ActivateTours(ObservableCollection<TourReservation> reservations, ObservableCollection<Tour> tours)
        {
            foreach (var reservation in reservations)
            {
                foreach (var tour in tours.Where(tour => tour.Id == reservation.TourId).Where(tour => reservation.HasStarted))
                {
                    tour.Began = true;
                }
            }
        }

        public Tour GetTourByTourPoint(TourPoint tourPoint)
        {
            foreach (var tour in _tourRepository.GetAll())
            {
                if (tour.Id == tourPoint.TourId)
                {
                    return tour;
                }
            }
            return null;
        }

        public List<double> GetTourStatistics(Tour tour)
        {
            double guestNumber = 0;
            double checkedGuestNumber = 0;
            double minorGuestNumber = 0;
            double regularGuestNumber = 0;
            double seniorGuestNumber = 0;
            double voucherGuestNumber = 0;
            double noVoucherGuestNumber = 0;
            foreach (var tourReservation in _tourReservationService.GetReservationsByTour(tour))
            {
                if (tourReservation.IsCompleted)
                {
                    guestNumber += tourReservation.GroupSize;
                    if (tourReservation.AverageAge <= 18)
                        minorGuestNumber += tourReservation.GroupSize;
                    else if (tourReservation.AverageAge <= 50)
                        regularGuestNumber += tourReservation.GroupSize;
                    else
                        seniorGuestNumber += tourReservation.GroupSize;
                    if (tourReservation.HasVoucher)
                        voucherGuestNumber += tourReservation.GroupSize;
                    else noVoucherGuestNumber += tourReservation.GroupSize;
                    if (tourReservation.IsChecked)
                        checkedGuestNumber += tourReservation.GroupSize;
                }
            }
            return new List<double>()
            {
                guestNumber, checkedGuestNumber, (checkedGuestNumber * 100) / guestNumber , minorGuestNumber, (minorGuestNumber * 100) / checkedGuestNumber,
                regularGuestNumber, (regularGuestNumber * 100) / checkedGuestNumber, seniorGuestNumber, (seniorGuestNumber * 100) / checkedGuestNumber, 
                (voucherGuestNumber * 100) / checkedGuestNumber, (noVoucherGuestNumber * 100) / checkedGuestNumber
            };
        }
        public List<TourStatisticsDTO> AssignStatisticsToDTO(ObservableCollection<Tour> tours)
        {
            List<TourStatisticsDTO> tourStatisticsDTOs = new List<TourStatisticsDTO>();
            foreach (var tour in tours)
            {
                TourStatisticsDTO tourStatisticsDTO = new TourStatisticsDTO(tour.Id, tour.Name, tour.Language,
                    tour.Duration, Convert.ToInt32(GetTourStatistics(tour)[0]),tour.ThumbnailUrl);
                tourStatisticsDTOs.Add(tourStatisticsDTO);

            }
            return tourStatisticsDTOs;
        }

        public List<Tour> GetFinishedToursByReservations()
        {
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                foreach(TourReservation tourReservation in _tourReservationService.GetFinishedReservations())
                {
                    if(tourReservation.TourId == tour.Id && !finishedTours.Contains(tour))
                        finishedTours.Add(tour);
                }
            }
            return finishedTours;
        }
        public List<Tour> GetFinishedToursByYear(int year)
        {
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                foreach (TourReservation tourReservation in _tourReservationService.GetFinishedReservations())
                {
                    if (tourReservation.TourId == tour.Id && !finishedTours.Contains(tour) && tourReservation.Date.Year == year)
                        finishedTours.Add(tour);
                }
            }
            return finishedTours;
        }
        public List<Tour> GetToursByGuide(User user)
        {
            List<Tour> tours = new List<Tour>();
            foreach (Tour tour in _tourRepository.GetAll())
            {
                if(tour.GuideId == user.Id)
                    tours.Add(tour);
            }
            return tours;
        }

        public List<Tour> GetActiveTours(ObservableCollection<Tour> tours)
        {
            var startedTours = new List<Tour>();
            foreach (var tourReservation in _tourReservationService.GetAllTourReservations())
            {
                if (tourReservation.HasStarted && (!tourReservation.IsCompleted))
                    startedTours.Add(GetById(tourReservation.TourId));
            }
            return startedTours;
        }

        public TourDate GetActiveTourDateByTour(Tour tour)
        {
            foreach (TourDate tourDate in _tourDateService.GetTourDatesByTour(tour))
            {
                foreach (TourReservation tourReservation in _tourReservationService.GetReservationsByTourDate(tourDate))
                {
                    if (tourReservation.HasStarted && !tourReservation.IsCompleted && tourReservation.TourId == tour.Id)
                        return tourDate;
                }
            }
            return null;
        }
        public Tour GetTourByStatisticsDTO(TourStatisticsDTO tourStatisticsDTO)
        {
            foreach (Tour tour in GetAll())
            {
                if (tour.Id == tourStatisticsDTO.TourId)
                    return tour;
            }
            return null;
        }
        
        public ObservableCollection<Tour> GetNewTours()
        {
            var newTours = new ObservableCollection<Tour>();
            foreach (var tour in GetAll().Where(tour => tour.IsNewlyCreated))
            {
                newTours.Add(tour);
            }
            return newTours;
        }

        public void SetNewTours(ObservableCollection<Tour> tours)
        {
            foreach (var tour in tours)
            {
                tour.IsNewlyCreated = false;
                UpdateTour(tour);
            }
        }


    }
}
