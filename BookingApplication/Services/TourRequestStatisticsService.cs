using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Model;

namespace BookingApplication.Services
{
    public class TourRequestStatisticsService
    {
        public Tuple<double, double, double> GetCountByRequestStatus(ObservableCollection<SimpleTourRequest> requests, int userId)
        {
            var accepted = 0;
            var denied = 0;
            var pending = 0;
            foreach (var request in requests)
            {
                switch (request.Status)
                {
                    case Status.Accepted when request.UserId == userId:
                        accepted++;
                        break;
                    case Status.Denied when request.UserId == userId:
                        denied++;
                        break;
                    case Status.Waiting when request.UserId == userId:
                        pending++;
                        break;
                }
            }
            return new Tuple<double, double, double>(accepted, denied, pending);
        }
    }

}
