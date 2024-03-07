using System;
using System.Collections.Generic;
using BookingApplication.Domain.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface IOwnerRepository
    {
        public List<Owner> GetAll();
        public OwnerRating Save(Owner owner);
        public int NextId();
        public void Delete(Owner owner);
        public Owner Update(Owner owner);
    }
}
