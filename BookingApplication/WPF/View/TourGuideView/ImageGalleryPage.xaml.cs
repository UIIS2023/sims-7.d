using System;
using System.Collections.Generic;
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
using BookingApplication.Domain.Model;
using BookingApplication.Services;
using BookingApplication.WPF.ViewModel.TourGuideViewModel;
using Image = BookingApplication.Domain.Model.Image;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for ImageGalleryPage.xaml
    /// </summary>
    public partial class ImageGalleryPage : Page
    {
        public static ImageGalleryVM imageGalleryVM;
        public ImageGalleryPage(User user, Tour tour, NavigationService navigationService, TourFormVMParent.FormStatus formStatus)
        {
            InitializeComponent();
            imageGalleryVM = new ImageGalleryVM(user, tour, navigationService, formStatus);
            //ImageGalleryVM imageGalleryVM = new ImageGalleryVM(user, tour, navigationService, formStatus);
            DataContext = imageGalleryVM;
        }


    }
}
