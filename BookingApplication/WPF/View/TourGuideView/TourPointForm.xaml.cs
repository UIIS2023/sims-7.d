using System;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuideViewModel;
using Image = BookingApplication.Domain.Model.Image;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourPointForm.xaml
    /// </summary>
    public partial class TourPointForm : Window
    {
        public TourPointForm(Tour tour, TourPoint tourPoint, Image image)
        {
            InitializeComponent();
            TourPointFormVM tourPointFormVM = new TourPointFormVM(tour, tourPoint, image);
            DataContext = tourPointFormVM;
            if (tourPointFormVM.CloseAction == null)
            {
                tourPointFormVM.CloseAction = new Action(() => this.Close());
            }
        }
    }
}
