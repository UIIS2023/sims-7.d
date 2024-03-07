using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.View.AccommodationGuestView;
using BookingApplication.WPF.View.OwnerView;
using BookingApplication.WPF.View.TourGuestView;
using BookingApplication.WPF.View.TourGuideView;

namespace BookingApplication.WPF.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

//        private readonly UserRepository _userRepository;
        private readonly UserService _userService;
            

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            //_repository = new UserRepository();
            _userService = new UserService();
        }

        /*  private void SignIn(object sender, RoutedEventArgs e)
          {
              User user = _repository.GetByUsername(Username);
              if (user != null)
              {
                  if(user.Password == txtPassword.Password)
                  {
                      CommentsOverview commentsOverview = new CommentsOverview(user);
                      commentsOverview.Show();
                      Close();
                  } 
                  else
                  {
                      MessageBox.Show("Wrong password!");
                  }
              }
              else
              {
                  MessageBox.Show("Wrong username!");
              }

          }*/
        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _userService.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == txtPassword.Password)
                {
                    switch (user.Username) {
                        case "owner":
                            OwnerOverview ownerOverview = new OwnerOverview();
                            ownerOverview.Show();
                            Close();
                            break;

                        case "guest1":
                            AccommodationsOverallView guestAccommodations= new AccommodationsOverallView(user);
                            guestAccommodations.Show();
                            Close();

                            break;


                        case "guest2":
                            GuestTourOverview guestTourOverview = new GuestTourOverview(user);
                            guestTourOverview.Show();
                            Close();
                            break;


                        case "guider":
                            GuideTourOverview guideTourOverview = new GuideTourOverview(user);
                            guideTourOverview.Show();
                            Close();
                            break;

                       

                    }


                    
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
        }
        }
}
