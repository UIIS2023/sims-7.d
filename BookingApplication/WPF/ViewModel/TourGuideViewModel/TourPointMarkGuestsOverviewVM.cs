using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Repository;
using BookingApplication.Services;
using CommunityToolkit.Mvvm.Input;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourPointMarkGuestsOverviewVM
    {
        public static ObservableCollection<TourReservation> NotPresentGuestTourReservations { get; set; }
        public static ObservableCollection<TourReservation> PresentGuestTourReservations { get; set; }

        public TourPoint SelectedTourPoint { get; set; }
        public TourReservation SelectedTourReservation { get; set; }
        public TourGuestDTO SelectedTourGuestDTO { get; set; }

        private readonly TourReservationService _tourReservationService;
        private readonly UserService _userService;

        private readonly TourPointService _tourPointService;
        private readonly TourNotificationService _tourNotificationService;
        private readonly TourService _tourService;

        public static ObservableCollection<TourGuestDTO> NotPresentGuestTourReservationsDTO { get; set; }
        public static ObservableCollection<TourGuestDTO> PresentGuestTourReservationsDTO { get; set; }

        public ICommand MarkGuestCommand { get; set; }
        public ICommand UnmarkGuestCommand { get; set; }
        public ICommand CancelOverviewCommand { get; set; }
        public ICommand SaveOverviewCommand { get; set; }
        public Action CloseAction { get; set; }
        public TourPointMarkGuestsOverviewVM(TourPoint selectedTourPoint)
        {
            _tourReservationService = new TourReservationService();
            _userService = new UserService();
            _tourPointService = new TourPointService();
            _tourService = new TourService();
            _tourNotificationService = new TourNotificationService();

            SelectedTourPoint = selectedTourPoint;
            SelectedTourReservation = new TourReservation();
            SelectedTourGuestDTO = new TourGuestDTO();

            PresentGuestTourReservations = new ObservableCollection<TourReservation>();
            InitializeNotPresentGuestTourReservations();

            PresentGuestTourReservationsDTO = new ObservableCollection<TourGuestDTO>();
            NotPresentGuestTourReservationsDTO = new ObservableCollection<TourGuestDTO>();

            AssignReservationsToDTO(NotPresentGuestTourReservations, NotPresentGuestTourReservationsDTO);
            MarkGuestCommand = new RelayCommand(MarkGuest);
            UnmarkGuestCommand = new RelayCommand(UnmarkGuest);
            CancelOverviewCommand = new RelayCommand(CancelOverview);
            SaveOverviewCommand = new RelayCommand(SaveOverview);
        }

        private void AssignReservationsToDTO(ObservableCollection<TourReservation> guestReservations,
            ObservableCollection<TourGuestDTO> tourGuestReservationsDTO)
        {
            foreach (var guestReservation in guestReservations)
            {
                /*tourId needs to be replaced by tourReservationId*/
                TourGuestDTO guestReservationDTO = new TourGuestDTO(guestReservation.UserId,
                    GetUsername(guestReservation), guestReservation.TourId, guestReservation.GroupSize);
                tourGuestReservationsDTO.Add(guestReservationDTO);
            }
        }

        private String GetUsername(TourReservation guestReservation)
        {
            foreach (var user in _userService.GetAll().Where(user => guestReservation.UserId == user.Id))
            {
                return user.Username;
            }
            return "";
        }

        private void InitializeNotPresentGuestTourReservations()
        {
            Tour selectedTour = _tourService.GetTourByTourPoint(SelectedTourPoint);
            NotPresentGuestTourReservations = new ObservableCollection<TourReservation>(_tourReservationService.GetUncheckedReservationsByTour(selectedTour));
        }

        private TourReservation GetTourReservationByGuestDTO(TourGuestDTO tourGuestDTO)
        {
            foreach (var guestReservation in PresentGuestTourReservations)
            {
                /*tourId needs to be replaced by tourReservationId*/
                if (tourGuestDTO.TourReservationId == guestReservation.TourId)
                {
                    return guestReservation;
                }
            }

            foreach (var guestReservation in NotPresentGuestTourReservations)
            {
                if (tourGuestDTO.TourReservationId == guestReservation.TourId)
                {
                    return guestReservation;
                }
            }

            return null;
        }
        private void MarkGuest()
        {
            SelectedTourReservation = GetTourReservationByGuestDTO(SelectedTourGuestDTO);

            SelectedTourReservation.TourPointId = SelectedTourPoint.Id;
            SelectedTourReservation.IsChecked = true;

            TourGuestDTO newSelectedTourGuestDTO = new TourGuestDTO(SelectedTourGuestDTO.UserId,
                SelectedTourGuestDTO.UserName, SelectedTourGuestDTO.TourReservationId,
                SelectedTourGuestDTO.TourReservationGroupSize);

            NotPresentGuestTourReservationsDTO.Remove(SelectedTourGuestDTO);
            NotPresentGuestTourReservations.Remove(SelectedTourReservation);


            PresentGuestTourReservationsDTO.Add(newSelectedTourGuestDTO);
            PresentGuestTourReservations.Add(SelectedTourReservation);
        }

        private void UnmarkGuest()
        {
            SelectedTourReservation = GetTourReservationByGuestDTO(SelectedTourGuestDTO);

            SelectedTourReservation.TourPointId = -1;
            SelectedTourReservation.IsChecked = false;

            PresentGuestTourReservationsDTO.Remove(SelectedTourGuestDTO);
            PresentGuestTourReservations.Remove(SelectedTourReservation);

            NotPresentGuestTourReservationsDTO.Add(SelectedTourGuestDTO);
            NotPresentGuestTourReservations.Add(SelectedTourReservation);
        }

        private void SaveOverview()
        {
            SelectedTourPoint.Active = true;
            _tourPointService.UpdateTourPoint(SelectedTourPoint);
            SaveTourReservations(PresentGuestTourReservations);
            SaveTourReservations(NotPresentGuestTourReservations);

            CloseAction();
        }

        private void CancelOverview()
        {
            CloseAction();
        }

        private void SaveTourReservations(ObservableCollection<TourReservation> tourReservations)
        {
            foreach (var tourReservation in tourReservations)
            {
                _tourReservationService.UpdateTourReservation(tourReservation);
                _tourNotificationService.SendNotifications(tourReservations.ToList());
            }
        }


    }
}
