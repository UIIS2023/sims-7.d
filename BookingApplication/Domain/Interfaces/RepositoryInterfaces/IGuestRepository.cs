using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface IGuestRepository
    {
        public List<Guest> GetAll();

        public User GetByUsername(string username);
    }
}
