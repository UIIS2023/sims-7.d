using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using Image = BookingApplication.Domain.Model.Image;


namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourForm.xaml
    /// </summary>

    public partial class TourForm : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Tour SelectedTour { get; set; }
        public Location SelectedLocation { get; set; }
        public Image SelectedThumbnailImage { get; set; }
        public static ObservableCollection<TourDate> TourDates { get; set; }
        public TourDate SelectedTourDate { get; set; }
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly ImageService _imageService;
        private readonly TourPointService _tourPointService;
        private readonly TourDateService _tourDateService;
        private readonly TourReservationService _tourReservationService;
        private readonly VoucherService _voucherService;
        public List<string> Countries {get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public enum FormStatus {UPDATE, ADD, LIVE}
        public FormStatus Status { get; set; }
        public User SelectedUser { get; set; }
        public List<TourReservation> TourReservations { get; set; }

        private string _tourDateInfo;
        public string TourDateInfo
        {
            get => _tourDateInfo;
            set
            {
                if (_tourDateInfo != value)
                {
                    _tourDateInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        public TourForm(int userId)
        {
            InitializeComponent();
            DataContext = this;
            Title = "Add Tour";

            _tourService = new TourService();
            _locationService = new LocationService();
            _imageService = new ImageService();
            _tourPointService = new TourPointService();
            _tourDateService = new TourDateService();
            _tourReservationService = new TourReservationService();
            _voucherService = new VoucherService();
            Countries = _locationService.GetLocations().Keys.ToList();

            SelectedTour = new Tour();
            SelectedTour.ImageId = new List<int>();

            SelectedLocation = new Location();
            SelectedThumbnailImage = new Image();
            TourDateInfo = "";
            TourDates = new ObservableCollection<TourDate>();
            //UserId = userId;
            Status = FormStatus.ADD;
            ConfigureButtons();
        }

        public TourForm(Tour selectedTour, FormStatus status)
        {
            InitializeComponent();
            DataContext = this;

            _tourService = new TourService();
            _locationService = new LocationService();
            _imageService = new ImageService();
            _tourPointService = new TourPointService();
            _tourDateService = new TourDateService();
            _tourReservationService = new TourReservationService();
            _voucherService = new VoucherService();
            Countries = _locationService.GetLocations().Keys.ToList();

            SelectedTour = selectedTour;
            TourDateInfo = "";
            InitializeSelectedLocation();
            TourDates = new ObservableCollection<TourDate>(_tourDateService.GetTourDatesByTour(SelectedTour));
            Status = status;
            ConfigureButtons();
        }

        private void ConfigureButtons()
        {
            if (Status == FormStatus.ADD || Status == FormStatus.UPDATE)
            {
                StartTourButton.IsEnabled = false;
                CancelTourButton.IsEnabled = false;
                EndTourButton.IsEnabled = false;
                ReturnButton.IsEnabled = false;
            }
            else
            {
                SaveButton.IsEnabled = false;
                CancelButton.IsEnabled = false;
            }
        }


        private void SaveTourClick(object sender, RoutedEventArgs e)
        {
            if (true/*SelectedTour.IsValid*/)
            {
                ConfigureSelectedLocation();
                if (Status == FormStatus.UPDATE)
                {
                    Tour updatedTour = _tourService.UpdateTour(SelectedTour);
                    Location updatedLocation = _locationService.UpdateLocation(SelectedLocation);
                    Image updatedImage = _imageService.Update(SelectedThumbnailImage);

                    if (updatedTour != null)
                    {

                        //needs to be resolved
                        //int index = TourOverview.Tours.IndexOf(SelectedTour);
                        //TourOverview.Tours.Remove(SelectedTour);
                        //TourOverview.Tours.Insert(index, updatedTour);
                        _tourPointService.SaveTourPointsInTour(updatedTour.Id);
                        Close();
                    }
                }
                else if (Status == FormStatus.ADD)
                {
                    SelectedTour.PossibleDates = TourDates.ToList();
                    var datesIds = SelectedTour.PossibleDates.Select(tourDate => tourDate.Id).ToList();
                    Tour newTour = new Tour(SelectedTour.Name, SelectedTour.LocationId, SelectedTour.Description,
                        SelectedTour.Language, SelectedTour.MaxGuests, datesIds ,SelectedTour.PossibleDates, SelectedTour.Duration,
                        false, SelectedTour.ImageId, SelectedTour.ThumbnailUrl, 6, false);
                    newTour = _tourService.SaveTour(newTour);
                    //also needs to be resolved
                    //TourOverview.Tours.Add(newTour);
                    _tourPointService.SaveTourPointsInTour(newTour.Id);
                    Close();
                }
            }
        }

        private void ConfigureSelectedLocation()
        {
            //checking if we already have this location stored in data
            if (_locationService.IsAlreadyInserted(SelectedLocation))
            {
                SelectedLocation.Id = _locationService.GetIdByCityAndCountry(SelectedLocation.City,
                    SelectedLocation.Country);
            }
            else
            {
                SelectedLocation = _locationService.SaveLocation(SelectedLocation);
            }

            SelectedTour.LocationId = SelectedLocation.Id;
        }
        
        private void CancelClick(object sender, EventArgs e)
        {
            DeleteTourPoints();
            Close();
        }

        private void DeleteTourPoints()
        {
                if (Status == FormStatus.ADD)
                {
                    _tourPointService.DeleteUnassignedTourPoints();
                }
        }

        private void EndTourClick(object sender, EventArgs e)
        {
            _tourPointService.ResetTourPoints(SelectedTour);
            SelectedTourDate = _tourService.GetActiveTourDateByTour(SelectedTour);
            _tourReservationService.EndTourReservations(SelectedTour);
            _tourDateService.DeleteTourDate(SelectedTourDate);
            TourDates.Remove(SelectedTourDate);
            TourDates.Clear();
            //TourDates = new ObservableCollection<TourDate>(_tourDateService.GetTourDatesByTour(SelectedTour));
            foreach (var tourDate in _tourDateService.GetTourDatesByTour(SelectedTour))
            {
                TourDates.Add(tourDate);
            }
        }

        private void StartTourClick(object sender, EventArgs e)
        {
            _tourReservationService.StartTourReservations(SelectedTour);
        }

        private void InitializeSelectedLocation()
        {
            Location initialLocation = _locationService.GetById(SelectedTour.LocationId);
            SelectedLocation = new Location(initialLocation.City, initialLocation.Country);
        }
        
        private void ShowGuideTourPointsOverview(object sender, RoutedEventArgs e)
        {
            if (Status == FormStatus.ADD || Status == FormStatus.UPDATE)
            {
                //GuideTourPointOverview guideTourPointOverview = new GuideTourPointOverview(SelectedTour, false);
                //guideTourPointOverview.Show();
            }
            else
            {
                //GuideTourPointOverview guideTourPointOverview = new GuideTourPointOverview(SelectedTour, true);
                //guideTourPointOverview.Show();
            }
        }

        private void ShowTourDateForm(object sender, RoutedEventArgs e)
        {
            if (SelectedTourDate != null)
            {
              //  TourDateForm tourDateForm = new TourDateForm(SelectedTour, SelectedTourDate,false);
                //tourDateForm.Show();
            }
            else
            {
                // TourDateForm tourDateForm = new TourDateForm(SelectedTour, false);
                //tourDateForm.Show();
            }            
        }

        private void ShowImagesOverview(object sender, RoutedEventArgs e)
        {
            TourImagesOverview tourImagesOverview = new TourImagesOverview(SelectedTour);
            tourImagesOverview.Show();
        }

        private void CountryComboBoxSelected(object sender, RoutedEventArgs e)
        {
            if (countryComboBox.SelectedIndex != -1)
            {
                cityComboBox.IsEnabled = true;
                Cities = new ObservableCollection<string>(_locationService.GetLocations()[SelectedLocation.Country]);
                cityComboBox.ItemsSource = Cities;
            }
        }

        private void TourDateDisplayClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTourDate != null)
            {
                TourReservations = _tourReservationService.GetReservationsByTourDate(SelectedTourDate);
                if (TourReservations.Count != 0)
                {
                    TourDateInfo = "There are " + TourReservations.Count.ToString() + " reservations, you can start a Tour!";
                }   
                else
                {
                    TourDateInfo = "There aren't any reservations yet.";
                }
            }
        }
        
        private void IncreaseMaxGuestsClick(object sender, RoutedEventArgs e)
        {
            SelectedTour.MaxGuests++;
        }

        private void DecreaseMaxGuestsClick(object sender, RoutedEventArgs e)
        {
            if(SelectedTour.MaxGuests!=0)
                SelectedTour.MaxGuests--;
        }
        private void IncreaseDurationClick(object sender, RoutedEventArgs e)
        {
            SelectedTour.Duration++;
        }

        private void DecreaseDurationClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTour.Duration != 0)
                SelectedTour.Duration--;
        }

        public void CancelTourClick(object sender, RoutedEventArgs e)
        {
            if (_tourDateService.IsTourDateCancellationPossible(SelectedTourDate))
            {
                MessageBoxResult result = MessageBox.Show("You can't cancel a Tour 48 hours before it begins",
                    "Note",MessageBoxButton.OK);
                return;
            }
            _voucherService.AssignVouchersToGuestsByTourDates(SelectedTourDate, SelectedTour);
            _tourReservationService.DeleteTourReservations(SelectedTour);
            _tourDateService.DeleteTourDate((SelectedTourDate));
            TourDates.Remove(SelectedTourDate);
        }

        private void ReturnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
