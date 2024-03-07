using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService()
        {
            _userRepository = Injector.CreateInstance<IUserRepository>();
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }
    }
}
