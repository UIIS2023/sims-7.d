using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;

namespace BookingApplication.WPF.View.OwnerView
{
    /// <summary>
    /// Interaction logic for GuestRatingForm.xaml
    /// </summary>
    public partial class GuestRatingForm : Window, INotifyPropertyChanged
    {
        public GuestRating NewGuestRating { get; set; }
        private readonly GuestRatingRepository _guestRatingRepository;
       
        public GuestRatingForm(int accommodationId,int guestId)
        {
            InitializeComponent();
            DataContext = this;
            NewGuestRating = new GuestRating();
            _guestRatingRepository = new GuestRatingRepository();
            NewGuestRating.AccommodationId= accommodationId;
            NewGuestRating.GuestId= guestId;
        
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void SaveGuestRate(object sender, RoutedEventArgs e)
        {
            if (NewGuestRating.IsValid)
            {
                GuestRating newGuestRating = new GuestRating(NewGuestRating.Cleanness, NewGuestRating.RulesFollowing,
                           NewGuestRating.Comment,NewGuestRating.AccommodationId,NewGuestRating.GuestId);

                _guestRatingRepository.Save(newGuestRating);

                MessageBox.Show("Guest rating saved.");
                Close();
            }
            else
            {
                MessageBox.Show("Guest rating cannot be made, not all fields are filled in correctly.");
            }
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

       
    }
}
