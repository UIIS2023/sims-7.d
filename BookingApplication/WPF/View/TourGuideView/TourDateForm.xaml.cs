using System;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuideViewModel;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourDateForm.xaml
    /// </summary>
    public partial class TourDateForm : Window
    {
        public TourDateForm(Tour tour)
        {
            InitializeComponent();
            TourDateFormVM tourDateFormVM = new TourDateFormVM(tour);
            DataContext = tourDateFormVM;
            if (tourDateFormVM.CloseAction == null)
            {
                tourDateFormVM.CloseAction = new Action(() => this.Close());
            }
        }   
    }
}



