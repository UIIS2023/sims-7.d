using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Serializer;

namespace BookingApplication.Repository
{
    public class SimpleTourRequestRepository : ISimpleTourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/simpleTourRequests.csv";

        private readonly Serializer<SimpleTourRequest> _serializer;

        private List<SimpleTourRequest> _simpleTourRequests;

        //private SimpleTourRequestRepository _simpleTourRequestRepository = new SimpleTourRequestRepository();

        public SimpleTourRequestRepository()
        {

            _serializer = new Serializer<SimpleTourRequest>();
            _simpleTourRequests = _serializer.FromCSV(FilePath);

        }

        public List<SimpleTourRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public SimpleTourRequest Save(SimpleTourRequest simpleTourRequest)
        {
            simpleTourRequest.Id = NextId();
            _simpleTourRequests = _serializer.FromCSV(FilePath);
            _simpleTourRequests.Add(simpleTourRequest);
            _serializer.ToCSV(FilePath, _simpleTourRequests);
            return simpleTourRequest;
        }

        public int NextId()
        {
            _simpleTourRequests = _serializer.FromCSV(FilePath);
            if (_simpleTourRequests.Count < 1)
            {
                return 1;
            }

            return _simpleTourRequests.Max(c => c.Id) + 1;
        }

        public void Delete(SimpleTourRequest simpleTourRequest)
        {
            _simpleTourRequests = _serializer.FromCSV(FilePath);
            SimpleTourRequest founded = _simpleTourRequests.Find(c => c.Id == simpleTourRequest.Id);
            _simpleTourRequests.Remove(founded);
            _serializer.ToCSV(FilePath, _simpleTourRequests);
        }

        public SimpleTourRequest Update(SimpleTourRequest simpleTourRequest)
        {
            _simpleTourRequests = _serializer.FromCSV(FilePath);
            SimpleTourRequest current = _simpleTourRequests.Find(c => c.Id == simpleTourRequest.Id);
            int index = _simpleTourRequests.IndexOf(current);
            _simpleTourRequests.Remove(current);
            _simpleTourRequests.Insert(index, simpleTourRequest); // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _simpleTourRequests);
            return simpleTourRequest;
        }

        public SimpleTourRequest GetById(int id)
        {
            return _simpleTourRequests.Find(c => c.Id == id);
        }
    }
}
