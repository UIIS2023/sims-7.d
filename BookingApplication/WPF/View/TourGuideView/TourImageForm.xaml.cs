using System;
using System.Windows;
using Image = BookingApplication.Domain.Model.Image;
using BookingApplication.Domain.Interfaces.ServiceInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuideViewModel;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourImageForm.xaml
    /// </summary>
    public partial class TourImageForm : Window
    {
        public TourImageForm(Tour tour)
        {
            InitializeComponent();
            Title = "Add Image";
            TourImageFormVM tourImageFormVM = new TourImageFormVM(tour);
            DataContext = tourImageFormVM;
            if (tourImageFormVM.CloseAction == null)
            {
                tourImageFormVM.CloseAction = new Action(() => this.Close());
            }
        }
        

    }
}
