using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Services
{
    class ReservationRescheduleRequestService
    {
        private readonly IReservationRescheduleRequestRepository rescheduleRequestRepository;

        public ReservationRescheduleRequestService()
        {
            rescheduleRequestRepository = Injector.CreateInstance<IReservationRescheduleRequestRepository>();
        }

        public void MakeRequest(ReservationRescheduleRequest request)
        {
            rescheduleRequestRepository.MakeRequest(request);
        }
    }
}