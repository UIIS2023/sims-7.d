using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BookingApplication.WPF.View.OwnerView;
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


namespace BookingApplication.WPF.View.OwnerView
{
    /// <summary>
    /// Interaction logic for MyAccommodationsWindow.xaml
    /// </summary>
    public partial class MyAccommodationsWindow : Window
    {
        public Accommodation SelectedAccommodation { get; set; }
        private readonly AccommodationRepository _accommodationRepository;
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public MyAccommodationsWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _accommodationRepository = new AccommodationRepository();
            Accommodations = new ObservableCollection<Accommodation>(_accommodationRepository.GetAll());

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            OwnerOverview ownerOverview = new OwnerOverview();
            ownerOverview.Show();
            Close();
        }

        private void Renovate_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                AccommodationRenovationWindow renovationWindow = new AccommodationRenovationWindow(SelectedAccommodation);
                renovationWindow.Show();
            }
            else
            {
                MessageBox.Show("Please select accommodation for renovation.");
            }
        }
    }
}
