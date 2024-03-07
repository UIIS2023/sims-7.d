using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using CommunityToolkit.Mvvm.Input;
using FormStatus = BookingApplication.WPF.ViewModel.TourGuideViewModel.TourFormVMParent.FormStatus;
using System.Runtime.CompilerServices;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourPointsOverviewVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public static ObservableCollection<TourPoint>? TourPoints { get; set; }
        public static ObservableCollection<TourReservation>? TourReservations { get; set; }
        public User SelectedUser { get; set; }
        public Tour SelectedTour { get; set; }
        public TourPoint SelectedTourPoint { get; set; }
        public TourReservation SelectedTourReservation { get; set; }
        public FormStatus SelectedStatus { get; set; }

        protected TourReservationService _tourReservationService;
        protected TourPointService _tourPointService;
        protected ImageService _imageService;
        protected NavigationService _navigationService;

        public ICommand ShowAddTourPointFormCommand { get; set; }
        public ICommand ShowUpdateTourPointFormCommand { get; set; }
        public ICommand DeleteTourPointCommand { get; set; }
        public ICommand ActivateTourPointCommand { get; set; }
        public ICommand SaveTourPointOverviewCommand { get; set; }
        public ICommand CancelTourPointOverviewCommand { get; set; }

        protected static int _finishedTourPointsCount;
        public static int FinishedTourPointsCount
        {
            get { return _finishedTourPointsCount; }
            set
            {
                if (_finishedTourPointsCount != value)
                {
                    _finishedTourPointsCount = value;
                }
            }
        }
        private bool _isLiveStatus;
        public bool IsLiveStatus
        {
            get => _isLiveStatus;
            set
            {
                if (_isLiveStatus != value)
                {
                    _isLiveStatus = value;
                }
            }
        }
        private bool _isEditStatus;
        public bool IsEditStatus
        {
            get => _isEditStatus;
            set
            {
                if (_isEditStatus != value)
                {
                    _isEditStatus = value;
                }
            }
        }

        public TourPointsOverviewVM(User user, Tour selectedTour, FormStatus status, NavigationService navigationService)
        {
            _tourReservationService = new TourReservationService();
            _tourPointService = new TourPointService();
            _imageService = new ImageService();
            _navigationService = navigationService;

            SelectedStatus = status;
            SelectedUser = user;
            SelectedTour = selectedTour;
            SelectedTourPoint = new TourPoint();
            SelectedTourReservation = new TourReservation();

            ConfigureButtons();

            TourPoints = new ObservableCollection<TourPoint>(_tourPointService.GetTourPointsByTour(SelectedTour));
            FinishedTourPointsCount = GetFinishedTourPointsCount();

            TourReservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAllTourReservations());

            ShowUpdateTourPointFormCommand = new RelayCommand(ShowUpdateTourPointForm);
            ShowAddTourPointFormCommand = new RelayCommand(ShowAddTourPointForm);
            DeleteTourPointCommand = new RelayCommand(DeleteTourPoint);
            ActivateTourPointCommand = new RelayCommand(ActivateTourPoint);
            CancelTourPointOverviewCommand = new RelayCommand(CancelTourPointOverview);
            SaveTourPointOverviewCommand = new RelayCommand(SaveTourPointOverview);
        }

        protected void ConfigureButtons()
        {
            if (SelectedStatus == FormStatus.LIVE)
            {
                IsEditStatus = false;
                IsLiveStatus = true;
            }
            else
            {
                IsEditStatus = true;
                IsLiveStatus = false;
            }
        }
        protected void ShowAddTourPointForm()
        {
            TourPointForm createTourPointForm = new TourPointForm(SelectedTour, new TourPoint(), new Image());
            createTourPointForm.Show();
        }
        protected void ShowUpdateTourPointForm()
        {
            TourPointForm createTourPointForm = new TourPointForm(SelectedTour, SelectedTourPoint, _imageService.GetById(SelectedTourPoint.ImageId));
            createTourPointForm.Show();
        }
        protected void DeleteTourPoint()
        {
            if (SelectedStatus == FormStatus.LIVE)
                MessageBox.Show("You can't delete Tour Points while the Tour is live!");
            else
            {
                if (SelectedTourPoint != null)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the Tour Point?",
                        "Delete Tour Point",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _tourPointService.DeleteTourPoint(SelectedTourPoint);
                        TourPoints.Remove(SelectedTourPoint);
                    }
                }
            }
        }
        protected void CancelTourPointOverview()
        {
            if (TourPoints.Count < 2)
            {
                var result = MessageBox.Show("Are you sure you want to exit the Tour Point Form? You haven't input" +
                                                          "atleast two Tour Points.",
                    "Tour Point Cancellation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteRemainingTourPoint();
                    //SaveTourPoints();
                    NavigateToTourForm();
                }
                else
                    return;
            }
            //SaveTourPoints();
            NavigateToTourForm();
        }

        protected void SaveTourPointOverview()
        {
            foreach (var tourPoint in TourPoints)
            {
                _tourPointService.UpdateTourPoint(tourPoint);
                NavigateToTourForm();
            }
        }
        protected void DeleteRemainingTourPoint()
        {
            if (TourPoints.Count == 1)
                _tourPointService.DeleteTourPoint(TourPoints[0]);

            TourPoints.Clear();
        }
        protected void ActivateTourPoint()
        {
            if (SelectedStatus != FormStatus.LIVE)
                MessageBox.Show("You can't activate Tour Points if the Tour isn't live!");
            else
            {
                if (SelectedTourPoint == null)
                    MessageBox.Show("Tour Point not selected!");
                else
                {
                    if (SelectedTourPoint.Order == FinishedTourPointsCount + 1 && SelectedTourPoint.Active == false)
                    {
                        TourPointMarkGuests tourPointMarkGuests = new TourPointMarkGuests(SelectedTourPoint);
                        tourPointMarkGuests.Show();
                    }
                    else
                    {
                        if (SelectedTourPoint.Order <= FinishedTourPointsCount)
                            MessageBox.Show("Tour Point is already activated.");
                        else
                            MessageBox.Show("Previous Tour Points have not been activated.");
                    }
                }
            }
        }
        protected int GetFinishedTourPointsCount()
        {
            int finishedTourPointsCount = 0;
            foreach (var tourPoint in TourPoints)
            {
                if (tourPoint.Active)
                    finishedTourPointsCount++;
            }
            return finishedTourPointsCount;
        }
        protected void NavigateToTourForm()
        {
            if (SelectedStatus == FormStatus.LIVE)
            _navigationService.NavigateTo(new TourFormLivePage(SelectedUser, SelectedTour, SelectedStatus,
                _navigationService));
            else
                _navigationService.NavigateTo(new TourFormPage(SelectedUser, SelectedTour, SelectedStatus,
                    _navigationService));
        }
        
    }
}   

