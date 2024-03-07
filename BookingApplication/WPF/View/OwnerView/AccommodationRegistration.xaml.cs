using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.View;
using Image = BookingApplication.Domain.Model.Image;

namespace BookingApplication.WPF.View.OwnerView
{
    /// <summary>
    /// Interaction logic for AccommodationRegistration.xaml
    /// </summary>
    public partial class AccommodationRegistration : Window, INotifyPropertyChanged
    {
        public Accommodation NewAccommodation { get; set; }
        public Location NewLocation { get; set; }

        private readonly AccommodationRepository _accommodationRepository;
        private readonly LocationRepository _locationRepository; 
        private readonly ImageService _imageService;
        public static ObservableCollection<Image> Images { get; set; }
        

        public AccommodationRegistration()
        {
            InitializeComponent();
            DataContext = this;
            NewAccommodation = new Accommodation();
            NewAccommodation.DaysBeforeCancelling = 1;
            NewLocation = new Location();

            Images = new ObservableCollection<Image>();

            _accommodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository();
           // _imageRepository = new ImageRepository();
            _imageService = new ImageService();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SaveAccommodationClick(object sender, EventArgs e)
        {

            if (NewAccommodation.IsValid && NewLocation.IsValid)
            {
                
               
                AccType();
                if (ConfigureSelectedLocation()==true) { 
                ConfigureSelectedImage();
                Accommodation newAccommodation = new Accommodation(NewAccommodation.Name, NewAccommodation.LocationId, NewAccommodation.AccommodationType,
                        NewAccommodation.MaxGuests, NewAccommodation.MinReservationDays, NewAccommodation.DaysBeforeCancelling, NewAccommodation.ImagesIds);

                _accommodationRepository.Save(newAccommodation);

                NewAccomodationForm();
                Close();}
            }
            else
            {
                MessageBox.Show("Accommodation cannot be made, not all fields are filled in correctly.");
            }
        }


         private AccommodationType AccType() { 
         switch (TypeCombo.Text)
                {
                    case "Apartment":
                        NewAccommodation.AccommodationType = AccommodationType.Apartment;
                        break;

                    case "House":
                        NewAccommodation.AccommodationType = AccommodationType.House;
                        break;

                    case "Shack":
                        NewAccommodation.AccommodationType = AccommodationType.Shack;
                        break;
                }
            return NewAccommodation.AccommodationType;

}

    private void NewAccomodationForm()
        {

            MessageBoxResult result = MessageBox.Show("Do you want to add a new one?", "New accommodation added!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                AccommodationRegistration createAccommodationForm = new AccommodationRegistration();
                createAccommodationForm.Show();
            }

        }

       
        private bool ConfigureSelectedLocation()
        {

            if (_locationRepository.IsAlreadyInserted(NewLocation))
            {
                NewLocation.Id = _locationRepository.GetIdByCityAndCountry(NewLocation.City,
                    NewLocation.Country); 
                NewAccommodation.LocationId = NewLocation.Id;
                return true;

            }
            else
            {
                MessageBox.Show("Location does not exist.Try again.");
                return false;
            }

           
        }

        private void ConfigureSelectedImage()
        {
            foreach (var image in Images)
            {
                if (_imageService.IsAlreadyInserted(image))
                    {
                        image.Id = _imageService.GetIdByUrl(image.Url);
                    }
                    else
                    {
                        _imageService.Save(image);

                    }
                    NewAccommodation.ImagesIds.Add(image.Id);
               
            }
        }

        private void HideUrlColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            
            if (e.PropertyName == "Id")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
            if (e.PropertyName == "Error")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

       

        private void AddUrlClick(object sender, RoutedEventArgs e)
        {
            Image NewImage = new Image();
            NewImage.Url = ImageBox.Text;
           if(NewImage.IsValid) { 
            Images.Add(NewImage);}
           else { MessageBox.Show("Not correct!"); }
          
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}