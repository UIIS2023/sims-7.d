using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.View.AccommodationGuestView;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApplication.WPF.ViewModel.GuestViewModel
{
    public class MakeReservationVM
    {
        public User LoggedUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public ObservableCollection<string> Types { get; set; }

        private readonly AccommodationService _accommodationService;
        private readonly LocationService _locationService;

        public List<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; } = new ObservableCollection<string>();
        public Accommodation SelectedAccommodation { get; set; } = null;
        public Location SelectedLocation { get; set; } = new Location();
        public int? SelectedMaxGuest { get; set; }
        public string SelectedName { get; set; }
        public AccommodationType SelectedType { get; set; }
        public bool TypeSelected { get; set; } = false;
        public int? SelectedMinReservationDays { get; set; }

        public MakeReserationCommands Commands { get; set; } = new MakeReserationCommands();

        public MakeReservationVM(User loggedUser, NavigationService navigationService)
        {
            LoggedUser = loggedUser;
            NavigationService = navigationService;

            _accommodationService = new AccommodationService();
            _locationService = new LocationService();

            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAll());
            Countries = _locationService.GetAll().Select(x => x.Country).Distinct().ToList();

            Commands.OnCancel = new RelayCommand(OnCancel);
            Commands.OnReserve = new RelayCommand(OnReserve);
            Commands.OnAcommodationSelected = new RelayCommand<object>(OnSelected);
            Commands.OnResetFilter = new RelayCommand(OnResetFilter);
            Commands.OnApplyFilter = new RelayCommand(OnApplyFilter);
            Commands.OnCountrySelected = new RelayCommand<object>(OnCountrySelected);
            Commands.OnTypeSelected = new RelayCommand(OnTypeSelected);
        }

        private void OnCancel()
        {
            NavigationService.NavigateTo(null);
        }

        private void OnSelected(object selectedAcomm)
        {
            if (selectedAcomm is Accommodation accom)
            {
                SelectedAccommodation = accom;
            }
        }

        private void OnReserve()
        {
            if (SelectedAccommodation != null)
            {
                NavigationService.NavigateTo(new ReserveAccommodationPage(LoggedUser, NavigationService, SelectedAccommodation));
            }
            else
            {
                MessageBox.Show("Please select an accommodation.");
            }
        }

        private void OnResetFilter()
        {
            Accommodations.Clear();
            _accommodationService.GetAll().ForEach(x => Accommodations.Add(x));
        }

        private void OnApplyFilter()
        {
            List<Accommodation> current = Accommodations.ToList();

            if (!string.IsNullOrEmpty(SelectedName))
            {
                current = current.Where(x => x.Name.ToUpper().Contains(SelectedName.ToUpper())).ToList();
            }

            if (TypeSelected)
            {
                current = current.Where(x => x.AccommodationType == SelectedType).ToList();
            }

            if (SelectedMaxGuest != null)
            {
                current = current.Where(x => x.MaxGuests >= SelectedMaxGuest).ToList();
            }

            if (SelectedMinReservationDays != null)
            {
                current = current.Where(x => x.MinReservationDays >= SelectedMinReservationDays).ToList();
            }

            current = FilterByCountryAndCity(current);

            Accommodations.Clear();
            current.ForEach(x => Accommodations.Add(x));
        }

        private List<Accommodation> FilterByCountryAndCity(List<Accommodation> accommodations)
        {
            List<Location> allLocations = _locationService.GetAll();

            if (!string.IsNullOrEmpty(SelectedLocation.Country))
            {
                allLocations = allLocations.Where(x => x.Country == SelectedLocation.Country).ToList();
            }

            if (!string.IsNullOrEmpty(SelectedLocation.City))
            {
                allLocations = allLocations.Where(x => x.City == SelectedLocation.City).ToList();
            }

            return accommodations.Where(x => allLocations.Any(l => l.Id == x.LocationId)).ToList();
        }

        private void OnCountrySelected(object countrySel)
        {
            if (countrySel is string countryStr && countrySel is not null)
            {
                Cities.Clear();
                _locationService.GetCitiesByCountry(countryStr).ForEach(x => Cities.Add(x));
            }
        }

        private void OnTypeSelected()
        {
            TypeSelected = true;
        }

        public class MakeReserationCommands
        {
            public ICommand OnCancel { get; set; }
            public ICommand OnAcommodationSelected { get; set; }
            public ICommand OnReserve { get; set; }
            public ICommand OnApplyFilter { get; set; }
            public ICommand OnResetFilter { get; set; }
            public ICommand OnCountrySelected { get; set; }
            public ICommand OnTypeSelected { get; set; }
        }
    }
}
