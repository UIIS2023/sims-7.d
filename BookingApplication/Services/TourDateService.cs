using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;

namespace BookingApplication.Services
{
    public class TourDateService
    {
        private readonly ITourDateRepository _tourDateRepository;
        public TourDateService()
        {
            _tourDateRepository = Injector.CreateInstance<ITourDateRepository>();
        }

        public void DeleteTourDate(TourDate tourDate)
        {
            _tourDateRepository.Delete(tourDate);
        }

        public TourDate SaveTourDate(TourDate tourDate)
        {
             return _tourDateRepository.Save(tourDate);
        }

        public TourDate UpdateTourDate(TourDate tourDate)
        {
            return _tourDateRepository.Update(tourDate);
        }

        public List<TourDate> GetTourDatesByTour(Tour tour)
        {
            List<TourDate> tourDates = new List<TourDate>();
            foreach (var tourDate in _tourDateRepository.GetAll())
            {
                if (tourDate.TourId == tour.Id)
                {
                    tourDates.Add(tourDate);
                }
            }
            return tourDates;
        }

        public List<string> PossibleDatesToString(List<DateTime> possibleDates)
        {
            var dates = new List<string>();
            foreach (var date in possibleDates)
            {
                dates.Add(date.ToString("dd/MM/yyyy"));
            }
            return dates;
        }

        public bool IsTourDateCancellationPossible(TourDate tourDate)
        {
            var timeBeforeCancelation = DateTime.Now - tourDate.StartTime;
            if (timeBeforeCancelation < new TimeSpan(48, 0, 0))
            {
                return false;
            }

            return true;
        }

        public List<int> GetActiveToursYears(ObservableCollection<Tour> tours)
        {
            List<TourDate> tourDates = new List<TourDate>();
            List<int> years = new List<int>();
            foreach (Tour tour in tours)
            {
                tourDates.AddRange(GetTourDatesByTour(tour));
            }
            foreach (TourDate tourDate in tourDates)
            {
                if(!years.Contains(tourDate.StartTime.Year))
                    years.Add(tourDate.StartTime.Year);
            }
            return years;
        }

        
        public List<Tour> GetPossibleDatesForTours(List<Tour> tours)
        {
            foreach (var tour in tours)
            {
                tour.PossibleDates = GetTourDatesByIds(tour.PossibleDatesIds);
            }
            return tours;
        }

        public List<TourDate> GetTourDatesByIds(List<int> tourDateIds)
        {
            List<TourDate> tourDates = new List<TourDate>();
            foreach (var tourDateId in tourDateIds)
            {
                tourDates.Add(_tourDateRepository.GetById(tourDateId));
            }
            return tourDates;
        }
    }
}
