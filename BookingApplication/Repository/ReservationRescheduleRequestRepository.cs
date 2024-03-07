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
    public class ReservationRescheduleRequestRepository : IReservationRescheduleRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/rescheduleRequests.csv";

        private readonly Serializer<ReservationRescheduleRequest> _serializer = new Serializer<ReservationRescheduleRequest>();

        private List<ReservationRescheduleRequest> _requests = new List<ReservationRescheduleRequest>();

        public ReservationRescheduleRequestRepository()
        {
            _requests = _serializer.FromCSV(FilePath);
        }

        public void MakeRequest(ReservationRescheduleRequest request)
        {
            request.Id = NextId();
            _requests = _serializer.FromCSV(FilePath);
            _requests.Add(request);
            _serializer.ToCSV(FilePath, _requests);
        }

        public int NextId()
        {
            _requests = _serializer.FromCSV(FilePath);
            if (_requests.Count < 1)
            {
                return 1;
            }
            return _requests.Max(ac => ac.Id) + 1;
        }
    }
}