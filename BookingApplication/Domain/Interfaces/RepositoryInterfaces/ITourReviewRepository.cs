using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface ITourReviewRepository
    {
        public List<TourReview> GetAll();
        public int NextId();
        public TourReview Save(TourReview tourDate);
        public void Delete(TourReview tourReview);
        public TourReview Update(TourReview tourReview);
        public TourReview GetById(int id);
    }
}
