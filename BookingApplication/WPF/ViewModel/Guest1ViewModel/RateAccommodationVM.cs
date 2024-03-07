using BookingApplication.Domain.Model;
using BookingApplication.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static BookingApplication.Services.AccommodationReservationService;

namespace BookingApplication.WPF.ViewModel.GuestViewModel
{

    public class RateAccommodationVM
    {
        private User LoggedUser { get; set; }
        private NavigationService NavigationService { get; set; }
        private AcccommodationReservationData ReservationData { get; set; }

        private OwnerRateService ratingService { get; set; } = new OwnerRateService();

        public OwnerRating Rating { get; set; } = new OwnerRating();

        public ICommand SubmitReview { get; set; }

        public RateAccommodationVM(User loggedUser, NavigationService navigationService, AcccommodationReservationData reservationData)
        {
            LoggedUser = loggedUser;
            NavigationService = navigationService;
            ReservationData = reservationData;
            SubmitReview = new RelayCommand(OnSubmit);
        }

        private void OnSubmit()
        {
            if (!AreRequiredFieldsFilled())
            {
                MessageBox.Show("Please fill out required fields");
                return;
            }

            Rating.AccommodationId = ReservationData.Accommodation.Id;
            Rating.GuestId = LoggedUser.Id;
            Rating.OwnerId = ReservationData.Accommodation.OwnerId;
            Rating.ImagesIds = new List<int>();

            ratingService.Save(Rating);
            MessageBox.Show("You've rated accommodation.");

            NavigationService.NavigateTo(null);
        }

        private bool AreRequiredFieldsFilled()
        {
            return Rating.Cleanness != 0 && Rating.OwnerCorrectness != 0
                && !string.IsNullOrEmpty(Rating.Comment) && !string.IsNullOrWhiteSpace(Rating.Comment);
        }
    }
}
