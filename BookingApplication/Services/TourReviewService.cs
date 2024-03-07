using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.DTO;
using BookingApplication.Repository;
using System.Collections.ObjectModel;

namespace BookingApplication.Services
{
    public class TourReviewService
    {
        private readonly ITourReviewRepository _tourReviewRepository;
        private readonly TourPointService _tourPointService;
        private readonly TourService _tourService;
        private readonly UserService _userService;

        public TourReviewService()
        {
            _tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            _tourPointService = new TourPointService();
            _userService = new UserService();
            _tourService = new TourService();
        }

        public void DeleteTourReview(TourReview tourReview)
        {
            _tourReviewRepository.Delete(tourReview);
        }

        public void SaveTourReview(TourReview tourReview)
        {
            _tourReviewRepository.Save(tourReview);
        }

        public TourReview UpdateTourReview(TourReview tourReview)
        {
            return _tourReviewRepository.Update(tourReview);
        }

        public List<TourReview> GetAllTourReviews()
        {
            return _tourReviewRepository.GetAll();
        }
        public List<TourPoint> GetTourPointsByReview(TourReview tourReview)
        {
            TourPoint reviewTourPoint = _tourPointService.GetById(tourReview.TourPointId);
            Tour reviewTour = _tourService.GetById(reviewTourPoint.TourId);
            List<TourPoint> reviewTourPoints = new List<TourPoint>();

            foreach (var tourPoint in _tourPointService.GetTourPointsByTour(reviewTour))
            {
                if (tourPoint.Order >= reviewTourPoint.Order)
                {
                    reviewTourPoints.Add(tourPoint);
                }
            }
            return reviewTourPoints;
        }
        
        public List<TourReview> GetReviewsByTour(Tour tour)
        {
            List<TourReview> tourReviews = new List<TourReview>();
            foreach (var tourReview in _tourReviewRepository.GetAll())
            {
                if (tourReview.TourId == tour.Id)
                    tourReviews.Add(tourReview);
            }
            return tourReviews;
        }

        public List<TourReview> GetReviewsByGuide(User guide)
        {
            List<TourReview> tourReviews = new List<TourReview>();
            foreach (Tour tour in _tourService.GetToursByGuide(guide))
                tourReviews.AddRange(GetReviewsByTour(tour));
            
            return tourReviews;
        }

        public List<TourReviewDTO> AssignReviewsToDTO(ObservableCollection<TourReview> tourReviews)
        {
            List<TourReviewDTO> tourReviewsDTO = new List<TourReviewDTO>();
            foreach (var tourReview in tourReviews)
            {
                TourReviewDTO tourReviewDTO = new TourReviewDTO(tourReview.Id, GetTourNameByReview(tourReview)
                    , GetUsernameByReview(tourReview), GetTourRating(tourReview), tourReview.IsValid);
                tourReviewsDTO.Add(tourReviewDTO);
            }
            return tourReviewsDTO;
        }

        public String GetUsernameByReview(TourReview tourReview)
        {
            foreach (var user in _userService.GetAll().Where(user => tourReview.UserId == user.Id))
                return user.Username;
            return "";
        }
        public String GetTourNameByReview(TourReview tourReview)
        {
            foreach (var tour in _tourService.GetAll().Where(tour => tourReview.TourId == tour.Id))
                return tour.Name;
            return "";
        }
        public String GetTourRating(TourReview tourReview)
        {
            return tourReview.TourRating.ToString() + "/5";
        }

        public TourReview GetTourReviewByDTO(TourReviewDTO tourReviewDTO)
        {
            foreach (TourReview tourReview in _tourReviewRepository.GetAll())
            {
                if (tourReview.Id == tourReviewDTO.TourReviewId)
                    return tourReview;
            }
            return null;
        }

        public void MarkTourReviewInvalid(TourReview tourReview)
        {
            tourReview.IsValid = false;
            _tourReviewRepository.Update(tourReview);
        }
    }
}
