using BookingApplication.Domain.Model;
using BookingApplication.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApplication.WPF.ViewModel.GuestViewModel
{
    class ShowOneUpcomingReservationVM : INotifyPropertyChanged
    {
        private User LoggedUser;
        private NavigationService NavigationService;
        public AccommodationReservationService.AcccommodationReservationData ReservationData { get; set; }

        private AccommodationReservationService reservationService = new AccommodationReservationService();
        private ReservationRescheduleRequestService rescheduleRequestService = new ReservationRescheduleRequestService();

        public Commands CommandsHandler { get; set; } = new Commands();

        public DateTime? NewDateFrom { get; set; }
        public DateTime? NewDateTo { get; set; }

        public ShowOneUpcomingReservationVM(User loggedUser, NavigationService navigationService, AccommodationReservationService.AcccommodationReservationData reservationData)
        {
            LoggedUser = loggedUser;
            NavigationService = navigationService;
            ReservationData = reservationData;

            CommandsHandler.OnBack = new RelayCommand(OnBack);
            CommandsHandler.OnCancelReservation = new RelayCommand(OnCancelReservation);
            CommandsHandler.OnRescheduleReservation = new RelayCommand(OnRescheduleReservation);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public class Commands
        {
            public ICommand OnBack { get; set; }
            public ICommand OnCancelReservation { get; set; }
            public ICommand OnRescheduleReservation { get; set; }
        }

        private void OnBack()
        {
            NavigationService.NavigateTo(null);
        }

        private void OnCancelReservation()
        {
            bool canceResult = reservationService.CancelReservation(ReservationData);
            MessageBox.Show(canceResult ? "You've canceled Your reservation" : "This reservation can not be canceled");
            if (canceResult)
            {
                NavigationService.NavigateTo(null);
            }
        }

        private void OnRescheduleReservation()
        {
            if (!DatesSelected())
            {
                MessageBox.Show("Please fill out dates.");
                return;
            }

            var newReservation = new AccommodationReservationService.AcccommodationReservationData()
            {
                Accommodation = ReservationData.Accommodation,
                DateFrom = (DateTime)NewDateFrom,
                DateTo = (DateTime)NewDateTo,
                GuestsNumber = ReservationData.GuestsNumber,
                ReservationId = ReservationData.ReservationId
            };

            bool availableForNewDates = reservationService.CheckAvailability(newReservation);

            if (availableForNewDates)
            {
                rescheduleRequestService.MakeRequest(new ReservationRescheduleRequest()
                {
                    NewDateStart = newReservation.DateFrom,
                    NewDateEnd = newReservation.DateTo,
                    ReservationId = newReservation.ReservationId,
                    State = RescheduleRequestState.ON_HOLD
                });

                MessageBox.Show("You've made a request to change dates.");
                NavigationService.NavigateTo(null);
            }
            else
            {
                MessageBox.Show("Reservation is not available for selected dates.");
            }
        }

        public bool DatesSelected()
        {
            return NewDateFrom.HasValue && NewDateTo.HasValue;
        }
    }
}
