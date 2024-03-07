using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;

namespace BookingApplication.Services
{
    public class OwnerService:UserService
    {
        private readonly IOwnerRepository _ownerRepository;
        public OwnerService()
        {
            _ownerRepository = Injector.CreateInstance<IOwnerRepository>();
        }

        public List<Owner> GetAll()
        {
            return _ownerRepository.GetAll();
        }

      
    }
}
