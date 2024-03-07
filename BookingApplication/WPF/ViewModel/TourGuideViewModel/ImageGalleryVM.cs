using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using CommunityToolkit.Mvvm.Input;
using Image = BookingApplication.Domain.Model.Image;
using FormStatus = BookingApplication.WPF.ViewModel.TourGuideViewModel.TourFormVMParent.FormStatus;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class ImageGalleryVM
    {
        private readonly ImageService _imageService;
        private readonly TourService _tourService;
        private readonly NavigationService  _navigationService;
        public User SelectedUser { get; set; }
        public Tour SelectedTour { get; set; }
        public Tour OldSelectedTour { get; set; }
        public FormStatus SelectedStatus { get; set; }
        public ImageGalleryDTO SelectedImageGalleryDTO { get; set; }
        public ObservableCollection<ImageGalleryDTO> ImagesGalleryDTO { get; set; }
        
        public ICommand ShowImageFormCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }
        public ICommand ChangeThumbnailImageCommand { get; set; }
        public ICommand SaveImageGalleryCommand { get; set; }
        public ICommand CancelImageGalleryCommand { get; set; }
        public Action DisableButtonsAction { get; set; }
        public ImageGalleryVM(User selectedUser, Tour selectedTour, NavigationService navigationService, FormStatus status)
        {
            SelectedUser = selectedUser;
            SelectedTour = selectedTour;
            OldSelectedTour = selectedTour;
            SelectedStatus = status;
            SelectedImageGalleryDTO = new ImageGalleryDTO();

            _imageService = new ImageService();
            _tourService = new TourService();
            _navigationService = navigationService;
            
            ImagesGalleryDTO = new ObservableCollection<ImageGalleryDTO>(_imageService.GetImageGalleryDTO(SelectedTour));

            if (SelectedStatus == FormStatus.LIVE)
            {
                DisableButtonsAction();
            }

            ShowImageFormCommand = new RelayCommand(ShowImageForm);
            DeleteImageCommand = new RelayCommand(DeleteImage);
            ChangeThumbnailImageCommand = new RelayCommand(ChangeThumbnailImage);
            SaveImageGalleryCommand = new RelayCommand(SaveImageGallery);
            CancelImageGalleryCommand = new RelayCommand(CancelImageGallery);
        }

        private void ShowImageForm()
        {
            TourImageForm tourImageForm = new TourImageForm(SelectedTour);
            tourImageForm.ShowDialog();
        }

        private void DeleteImage()
        {
            if (SelectedImageGalleryDTO != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the image?", "Delete Image",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (SelectedTour.ThumbnailUrl == SelectedImageGalleryDTO.Url)
                    {
                        SelectedTour.ThumbnailUrl = OldSelectedTour.ThumbnailUrl;
                    }
                    SelectedTour.ImageId.Remove(SelectedImageGalleryDTO.ImageId);
                    _imageService.Delete(_imageService.GetById(SelectedImageGalleryDTO.ImageId));
                    ImagesGalleryDTO.Remove(SelectedImageGalleryDTO);
                }
            }
        }

        private void ChangeThumbnailImage()
        {
            if (SelectedImageGalleryDTO != null)
            {
                SelectedTour.ThumbnailUrl = SelectedImageGalleryDTO.Url;
                ImagesGalleryDTO.Clear();
                foreach (var imageGalleryDTO in _imageService.GetImageGalleryDTO(SelectedTour))
                {
                    ImagesGalleryDTO.Add(imageGalleryDTO);
                }
            }
        }

        private void SaveImageGallery()
        {
            foreach (var imageGalleryDTO in ImagesGalleryDTO)
            {
                if (imageGalleryDTO.ImageId == 0)
                {
                    Image image = new Image(imageGalleryDTO.Url);
                    if (_imageService.IsAlreadyInserted(image))
                    {
                        image.Id = _imageService.GetIdByUrl(image.Url);
                    }
                    else
                    {
                        image = _imageService.Save(image);
                    }
                    SelectedTour.ImageId.Add(image.Id);
                }
            }
            _navigationService.NavigateTo(new TourFormPage(SelectedUser, SelectedTour, SelectedStatus, _navigationService));
        }

        private void CancelImageGallery()
        {
            _navigationService.NavigateTo(new TourFormPage(SelectedUser, OldSelectedTour, SelectedStatus, _navigationService));
        }


    }
}
