using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookingApplication.Services;
using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using System.ComponentModel;
using BookingApplication.Serializer;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.WPF.View.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerRating.xaml
    /// </summary>
    public partial class OwnerRatingWindow : Window
    {
       
        
        public ObservableCollection<OwnerRating> OwnerRatings { get ; set; }

        private readonly OwnerRateService _ownerRateService;
        private readonly GuestRatingService _guestRatingService;
        public OwnerRatingWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            
            _ownerRateService = new OwnerRateService();
            _guestRatingService = new GuestRatingService();
            OwnerRatings = new ObservableCollection<OwnerRating>(FilterOwnerRatings());
        }

        public ObservableCollection<OwnerRating> FilterOwnerRatings()
        {

            ObservableCollection<OwnerRating> filteredOwnerRatings = new ObservableCollection<OwnerRating>();

            foreach (var or in _ownerRateService.GetAll())
            {
                foreach (var g in _guestRatingService.GetAll()) { 
                    if (or.GuestId==g.GuestId)
                {
                    filteredOwnerRatings.Add(or);

                }
             }
            }

            return filteredOwnerRatings;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            
                OwnerOverview ownerOverview = new OwnerOverview();
                ownerOverview.Show();
            Close();
            
        }
    }
}
