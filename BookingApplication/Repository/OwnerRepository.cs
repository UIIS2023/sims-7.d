using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Repository
{
    public class OwnerRepository 
    {
        private const string FilePath = "../../../Resources/Data/owners.csv";

        private readonly Serializer<Owner> _serializer;

        private List<Owner> _owners;

        public OwnerRepository()
        {
            _serializer = new Serializer<Owner>();
            _owners = _serializer.FromCSV(FilePath);
        }

        public List<Owner> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            _owners = _serializer.FromCSV(FilePath);
            return _owners.FirstOrDefault(u => u.Username == username);
        }


    }
}
