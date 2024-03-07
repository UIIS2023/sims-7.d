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
    public class TourReviewRepository : ITourReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReviews.csv";

            private readonly Serializer<TourReview> _serializer;

            private List<TourReview> _tourReviews;

            private readonly TourReviewRepository _tourReviewRepository;

            public TourReviewRepository()
            {

                _serializer = new Serializer<TourReview>();
                _tourReviews = _serializer.FromCSV(FilePath);

            }

            public List<TourReview> GetAll()
            {
                return _serializer.FromCSV(FilePath);
            }

            public int NextId()
            {
                _tourReviews = _serializer.FromCSV(FilePath);
                if (_tourReviews.Count < 1)
                {
                    return 1;
                }
                return _tourReviews.Max(c => c.Id) + 1;
            }

            public TourReview Save(TourReview tourDate)
            {
                tourDate.Id = NextId();
                _tourReviews = _serializer.FromCSV(FilePath);
                _tourReviews.Add(tourDate);
                _serializer.ToCSV(FilePath, _tourReviews);
                return tourDate;
            }

            public void Delete(TourReview tourReview)
            {
                _tourReviews = _serializer.FromCSV(FilePath);
                TourReview founded = _tourReviews.Find(c => c.Id == tourReview.Id);
                _tourReviews.Remove(founded);
                _serializer.ToCSV(FilePath, _tourReviews);
            }

            public TourReview Update(TourReview tourReview)
            {
                _tourReviews = _serializer.FromCSV(FilePath);
                TourReview current = _tourReviews.Find(c => c.Id == tourReview.Id);
                int index = _tourReviews.IndexOf(current);
                _tourReviews.Remove(current);
                _tourReviews.Insert(index, tourReview);       // keep ascending order of ids in file 
                _serializer.ToCSV(FilePath, _tourReviews);
                return tourReview;
            }

            public TourReview GetById(int id)
            {
                _tourReviews = _serializer.FromCSV(FilePath);
                return _tourReviews.Find(c => c.Id == id);
            }
        

    }
}
