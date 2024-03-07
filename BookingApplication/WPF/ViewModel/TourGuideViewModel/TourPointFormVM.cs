using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using BookingApplication.Services;
using BookingApplication.WPF.View.TourGuideView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.CompilerServices;
using BookingApplication.Domain;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class TourPointFormVM : ValidationBase
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private readonly TourPointService _tourPointService;
        private readonly ImageService _imageService;
        public Tour SelectedTour { get; set; }
        public TourPoint SelectedTourPoint { get; set; }
        public Image SelectedImage { get; set; }
        public ICommand SaveTourPointFormCommand { get; set; }
        public ICommand CancelTourPointFormCommand { get; set; }
        public ICommand DisplayImageCommand { get; set; }
        public Action CloseAction { get; set; }

        private string _inputUrl;
        public string InputUrl
        {
            get => _inputUrl;
            set
            {
                if (value != _inputUrl)
                {
                    _inputUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        public TourPointFormVM(Tour selectedTour, TourPoint selectedTourPoint, Image selectedImage)
        {
            _tourPointService = new TourPointService();
            _imageService = new ImageService();

            SelectedTour = selectedTour;
            SelectedTourPoint = selectedTourPoint;
            SelectedImage = selectedImage;

            SaveTourPointFormCommand = new RelayCommand(SaveTourPointForm);
            CancelTourPointFormCommand = new RelayCommand(CancelTourPointForm);
            DisplayImageCommand = new RelayCommand(DisplayImage);
        }
        private void SaveTourPointForm()
        {
            SelectedTourPoint.Validate();
            this.Validate();
            if (SelectedTourPoint.IsValid && this.IsValid)
            {
                ConfigureSelectedImage();
                
                    TourPoint newTourPoint = new TourPoint(SelectedTourPoint.Name, SelectedTour.Id,
                        TourPointsOverviewVM.TourPoints.Count + 1,
                        SelectedTourPoint.Active, SelectedTourPoint.ImageId);
                //_tourPointService.SaveTourPoint(newTourPoint);
                TourPointsOverviewVM.TourPoints.Add(newTourPoint);
                CloseAction();
            }
        }
        private void CancelTourPointForm()
        {
            CloseAction();
        }

        private void DisplayImage()
        {
            if (InputUrl != null)
                SelectedImage.Url = InputUrl;
        }
        private void ConfigureSelectedImage()
        {
            if (_imageService.IsAlreadyInserted(SelectedImage))
            {
                SelectedImage.Id = _imageService.GetIdByUrl(SelectedImage.Url);
            }
            else
            {
                SelectedImage = _imageService.Save(SelectedImage);
            }
            //image saving regarding tour points needs to be fixed
            SelectedTourPoint.ImageId = SelectedImage.Id;
            SelectedTour.ImageId.Add(SelectedImage.Id);
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.InputUrl))
            {
                this.ValidationErrors["InputUrl"] = "Url is required.";
            }
        }
    }
}
