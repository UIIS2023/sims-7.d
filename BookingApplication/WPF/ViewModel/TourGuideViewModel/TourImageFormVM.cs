using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourImageFormVM
    {
        private readonly TourService _tourService;
        private readonly ImageService _imageService;

        public Tour SelectedTour { get; set; }
        public Image SelectedImage { get; set; }
        
        public ICommand SaveImageFormCommand { get; set; }
        public ICommand CancelImageFormCommand { get; set; }
        public ICommand DisplayCommand { get; set; }
        public Action CloseAction { get; set; }
        public TourImageFormVM(Tour tour)
        {
            SelectedTour = tour;
            SelectedImage = new Image();

            _tourService = new TourService();
            _imageService = new ImageService();

            SaveImageFormCommand = new RelayCommand(SaveImageForm);
            CancelImageFormCommand = new RelayCommand(CancelImageForm);
            DisplayCommand = new RelayCommand(Display);
        }

        private void SaveImageForm()
        {
            if (SelectedImage.IsValid)
            {
                ImageGalleryDTO imageGalleryDTO = new ImageGalleryDTO(SelectedImage.Id, SelectedImage.Url, false);
                ImageGalleryPage.imageGalleryVM.ImagesGalleryDTO.Add(imageGalleryDTO);
                CloseAction();
            }
        }

        private void Display()
        {

        }
        private void CancelImageForm()
        {
            CloseAction();
        }

        private void ConfigureSelectedImage()
        {
            //checking if we already have this image stored in data
            if (_imageService.IsAlreadyInserted(SelectedImage))
            {
                SelectedImage.Id = _imageService.GetIdByUrl(SelectedImage.Url);
            }
        }

    }
}
