using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Model;

namespace BookingApplication.WPF.ViewModel.TourGuideViewModel
{
    public class ImagePanelVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Image SelectedImage { get; set; }
        private bool _isThumbnailImage;

        public bool IsThumbnailImage
        {
            get => _isThumbnailImage;
            set
            {
                if (_isThumbnailImage != value)
                {
                    _isThumbnailImage = value;
                    OnPropertyChanged();
                }
            }
        }
        public ImagePanelVM(Image selectedImage, bool isThumbnailImage)
        {
            SelectedImage = selectedImage;
            IsThumbnailImage = isThumbnailImage;
        }

    }
}
