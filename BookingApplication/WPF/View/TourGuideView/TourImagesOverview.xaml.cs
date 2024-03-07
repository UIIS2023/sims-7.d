using System.Collections.ObjectModel;
using System.Windows;
using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourImagesOverview.xaml
    /// </summary>
    public partial class TourImagesOverview : Window
    {
        
        public  static ObservableCollection<Image> Images { get; set; }
        public Tour SelectedTour { get; set; }
        public Image SelectedImage { get; set; }

        private readonly TourRepository _tourRepository;
        private readonly ImageService _imageService;

        public TourImagesOverview(Tour selectedTour)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _imageService = new ImageService();

            SelectedTour = selectedTour;
            Images = new ObservableCollection<Image>();

            GetImagesByTour();

        }


        private void GetImagesByTour()
        {   
            if(SelectedTour.ImageId == null)
                return;
            
            foreach (var image in _imageService.GetAll())
            {
                foreach (int imageId in SelectedTour.ImageId)
                {
                    if (image.Id == imageId)
                    {
                        Images.Add(image);
                    }
                }
            }
        }

        private void ShowAddImageForm(object sender, RoutedEventArgs e)
        {
            TourImageForm tourImageForm = new TourImageForm(SelectedTour);
            tourImageForm.ShowDialog();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (SelectedImage != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Delete comment",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _imageService.Delete(SelectedImage);
                    Images.Remove(SelectedImage);
                }
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MakeThumbnailImageClick(object sender, RoutedEventArgs e)
        {
            SelectedTour.ThumbnailUrl = SelectedImage.Url;
        }
        
    }
}
