using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface IGuestRatingRepository
    {
        public List<GuestRating> GetAll();
        public GuestRating Save(GuestRating guestRating);
        public int NextId();
        public void Delete(GuestRating guestRating);
        public GuestRating Update(GuestRating guestRating);
       // public bool IsInFile(OwnerRating ownerRating);
     
    }
}
