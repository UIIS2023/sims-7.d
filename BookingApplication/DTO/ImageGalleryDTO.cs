using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Core;

namespace BookingApplication.DTO
{
    public class ImageGalleryDTO : INotifyPropertyChanged
    {
        private int _imageId;
        private string _url;
        private bool _isThumbnailImage;
        private string _type;

        public ImageGalleryDTO() {}
        public ImageGalleryDTO(int imageId, string url, bool isThumbnailImage)
        {
            ImageId = imageId;
            Url = url;
            IsThumbnailImage = isThumbnailImage;
            if (IsThumbnailImage)
                Type = "Thumbnail Image";
            else
                Type = "Regular Image";
        }

        public int ImageId
        {
            get => _imageId; 
            set => _imageId = value;
        }

        public string Url
        {
            get => _url;
            set => _url = value;
        }

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

        public string Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value; 
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
