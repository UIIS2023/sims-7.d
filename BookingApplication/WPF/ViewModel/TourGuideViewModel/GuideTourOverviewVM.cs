using BookingApplication.Domain.Model;
using BookingApplication.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookingApplication.WPF.View.TourGuideView;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{ 
    public class GuideTourOverviewVM : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }

        private readonly TourService _tourService;
        private readonly NavigationService _navigationService;
        private readonly VoucherService _voucherService;

        public ICommand ViewAddTourFormCommand { get; set; }
        public ICommand ViewUpdateTourFormCommand { get; set; }
        public ICommand ViewLiveTourFormCommand { get; set; }
        public ICommand DeleteTourCommand { get; set; }
        public ICommand FilterToursCommand { get; set; }
        public User SelectedUser { get; set; }

        private string _activeTourInfo;
        public string ActiveTourInfo
        {
            get => _activeTourInfo;
            set
            {
                if (_activeTourInfo != value)
                {
                    _activeTourInfo = value;
                    OnPropertyChanged();
                }
            }
        }
        public GuideTourOverviewVM(User user, NavigationService navigationService)
        {
            SelectedUser = user;
            _navigationService = navigationService;
            _tourService = new TourService();
            _voucherService = new VoucherService();
            Tours = new ObservableCollection<Tour>(new ObservableCollection<Tour>(_tourService.GetToursByGuide(SelectedUser)));
            ViewAddTourFormCommand = new RelayCommand(ShowAddTourForm);
            ViewUpdateTourFormCommand = new RelayCommand(ShowUpdateTourForm);
            ViewLiveTourFormCommand = new RelayCommand(ShowLiveTourForm);
            DeleteTourCommand = new RelayCommand(DeleteTour);
            FilterToursCommand = new RelayCommand(FilterTours);

            ActiveTourInfo = "Test display";
            DisplayActiveTourInfo();
        }

        private void ShowAddTourForm()
        {
            _navigationService.NavigateTo(new TourFormPage(SelectedUser, new Tour(),
                    TourFormVMParent.FormStatus.ADD, _navigationService));
        }

        private void ShowUpdateTourForm()
        {
            if (SelectedTour != null)
            {
                _navigationService.NavigateTo(new TourFormPage(SelectedUser, SelectedTour, TourFormVMParent.FormStatus.UPDATE, _navigationService));
            }
        }
        private void ShowLiveTourForm()
        {
            if (SelectedTour != null)
            {
                _navigationService.NavigateTo(new TourFormLivePage(SelectedUser, SelectedTour, TourFormVMParent.FormStatus.LIVE, _navigationService));
            }
        }
        private void DeleteTour()
        {
            if (SelectedTour != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Delete comment",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _voucherService.AssignVouchersToGuestsByTour(SelectedTour);
                    _tourService.DeleteTour(SelectedTour);
                    Tours.Remove(SelectedTour);
                }
            }
        }

        private void DisplayActiveTourInfo()
        {
            Tour activeTour = new Tour();
            List<Tour> activeTours = _tourService.GetActiveTours(Tours);
            activeTour = activeTours.First();
            if (activeTour != null)
            {
                ActiveTourInfo = "Tour " + activeTour.Name + " is currently active!";
            }
            else
            {
                ActiveTourInfo = "There aren't any active tours currently.";
            }
        }

        private void FilterTours()
        {
            DisplayActiveTourInfo();
        }
        
        
    }
}
