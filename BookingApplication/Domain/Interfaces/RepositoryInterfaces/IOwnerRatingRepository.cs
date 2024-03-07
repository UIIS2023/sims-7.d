using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface IOwnerRatingRepository
    {
        public List<OwnerRating> GetAll();
        public OwnerRating Save(OwnerRating ownerRating);
        public int NextId();
        public void Delete(OwnerRating ownerRating);
        public OwnerRating Update(OwnerRating ownerRating);
    }
}
