using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Model;
using BookingApplication.Services;

namespace BookingApplication.WPF.ViewModel.TourGuestViewModel
{
    public class AcceptedRequestsVm
    {
        public ObservableCollection<SimpleTourRequest> Requests { get; set; }
        public AcceptedRequestsVm(User user, NavigationService navigationService)
        {
            var requestService = new SimpleTourRequestService();
            Requests = new ObservableCollection<SimpleTourRequest>(requestService.GetAllAcceptedRequests(user.Id));
        }
    }
}
