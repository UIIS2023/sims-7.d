using BookingApplication.Domain.Model;
using BookingApplication.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApplication.WPF.ViewModel.GuestViewModel
{
    class ReceivedRatingsVM
    {
        private User LoggedUser { get; set; }
        private NavigationService NavigationService { get; set; }

        private GuestRatingService GuestRatingService { get; set; } = new GuestRatingService();

        public ObservableCollection<GuestRatingByOwner> GuestRatingsByOwner { get; set; } = new ObservableCollection<GuestRatingByOwner>();
        public ReceivedRatingsVM(User loggedUser, NavigationService navigationService)
        {
            LoggedUser = loggedUser;
            NavigationService = navigationService;
            OnBack = new RelayCommand(OnBackHandler);

            GuestRatingsByOwner.Clear();
            GuestRatingService.GetRatingsForUser(loggedUser).ForEach(x => GuestRatingsByOwner.Add(x));
        }

        public ICommand OnBack { get; set; }

        private void OnBackHandler()
        {
            NavigationService.NavigateTo(null);
        }
    }
}