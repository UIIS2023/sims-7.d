using System;
using System.Collections.ObjectModel;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Domain.Interfaces;
using BookingApplication.Services;
using System.Diagnostics;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace BookingApplication.WPF.View.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerOverview.xaml
    /// </summary>
    public partial class OwnerOverview : Window
    {

       
        private readonly GuestRatingRepository _guestRatingRepository;
        private readonly AccommodationReservationRepository _accommodationReservationRepository;
        private readonly AccommodationRepository _accommodationRepository;
        private readonly GuestRepository _guestRepository;
        
       
        public ObservableCollection<Accommodation> Accommodations { get; set; }

        
        public OwnerOverview()
        {
            InitializeComponent();
            this.DataContext = this;

            _guestRatingRepository = new GuestRatingRepository();
            _accommodationReservationRepository = new AccommodationReservationRepository();
            _accommodationRepository = new AccommodationRepository();
            _guestRepository = new GuestRepository();


        }
      
       
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            foreach (AccommodationReservation accommodationReservation in _accommodationReservationRepository.GetAll())
            {
                if (isRatePossible(accommodationReservation.EndDate) && !(IsAlreadyInserted(accommodationReservation.AccommodationId, accommodationReservation.GuestId)))
                {
                    NewRatingForm(accommodationReservation.AccommodationId, accommodationReservation.GuestId);
                }
               
            }
        }



        public String GetGuestById(int id)
        {
            foreach (var guest in _guestRepository.GetAll())
            {
                if (guest.Id == id) return guest.Username;
            }
            return null;
        }


        public String GetAccommodationById(int id)
        {
            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                if (accommodation.Id == id) return accommodation.Name;
            }
            return null;
        }

        private bool isRatePossible(DateTime date)
        {
            DateTime startRateDate = DateTime.Now;
            DateTime endRateDate = DateTime.Now.AddDays(-5);
            if (startRateDate >= date && endRateDate <= date)
            {
                return true;
            }
            return false;
        }


        public bool IsAlreadyInserted(int accommodationId, int guestId)
        {
            foreach (var iteration in _guestRatingRepository.GetAll())
            {
                if (accommodationId == iteration.AccommodationId && guestId == iteration.GuestId)
                {
                    return true;
                }
            }
            return false;
        }

        private void NewRatingForm(int accommodationId, int guestId)
        {
           
            MessageBoxResult result = MessageBox.Show($"Do you want to rate guest:{GetGuestById(guestId)} who was in the accommodation: {GetAccommodationById(accommodationId)}", "You can rate your guest!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                GuestRatingForm createGuestRatingForm = new GuestRatingForm(accommodationId, guestId);
                createGuestRatingForm.Show();

            }
        }
      
        

        private void ShowCreateAccommodationForm(object sender, RoutedEventArgs e)
        {
            AccommodationRegistration createAccommodationForm = new AccommodationRegistration();
            createAccommodationForm.Show();
        }


        private void ShowOwnerRatings(object sender, RoutedEventArgs e)
        {
            OwnerRatingWindow ownerRating = new OwnerRatingWindow();
            ownerRating.Show();
            Close();
        }
        private void ShowRenovations(object sender, RoutedEventArgs e)
        {
            RenovationsOverview renovationsOverview = new RenovationsOverview();
            renovationsOverview.Show();
            Close();
        }

        private void ShowMyAccommodations(object sender, RoutedEventArgs e)
        {
            MyAccommodationsWindow myAccommodations= new MyAccommodationsWindow();
            myAccommodations.Show();
            Close();
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void Review_Click(object sender, RoutedEventArgs e)
        {
            OwnerRatingWindow ownerRatingWindow = new OwnerRatingWindow();
            ownerRatingWindow.Show();
            Close();
        }
    }
}
