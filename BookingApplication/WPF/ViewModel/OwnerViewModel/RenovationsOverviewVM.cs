using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingApplication.Services;
using BookingApplication.WPF.View.OwnerView;
using BookingApplication.WPF.ViewModel.OwnerViewModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using NavigationService = BookingApplication.Services.NavigationService;
using System.Threading.Channels;

namespace BookingApplication.WPF.ViewModel.OwnerViewModel
{
    public class RenovationsOverviewVM
    {
        public User User { get; set; }
        public NavigationService NavigationService { get; set; }

        private readonly AccommodationRenovationService _accommodationRenovationService;
        public AccommodationRenovation SelectedRenovation { get; set; }
      
        public ObservableCollection<AccommodationRenovation> AccommodationRenovations { get; set; }
        public ICommand CancelRenovateCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public RenovationsOverviewVM()
        {
            //User = user;
           // NavigationService = navigationService;

            _accommodationRenovationService = new AccommodationRenovationService();
            AccommodationRenovations = new ObservableCollection<AccommodationRenovation>(_accommodationRenovationService.GetAll());

            CancelRenovateCommand = new RelayCommand(CancelRenovate);
            BackCommand = new RelayCommand(Back);
        }
        private void Back()
        {
            
            OwnerOverview ownerOverview = new OwnerOverview();
            ownerOverview.Show();
          
        }

        private void CancelRenovate()
        {
            if (SelectedRenovation != null)
            {
               MessageBoxResult result = MessageBox.Show("Are you sure?", "Cancel your renovation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _accommodationRenovationService.DeleteAccommodationRenovation(SelectedRenovation);
                  

            }
            }
            else
            {
                MessageBox.Show("Please select renovation for cancelling.");
            }

        }

    }
}
