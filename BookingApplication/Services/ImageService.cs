using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Repository;
using System.Windows.Media.Imaging;
using BookingApplication.Domain.Model;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.DTO;

namespace BookingApplication.Services
{
    public class ImageService
    {
        private readonly IImageRepository _imageRepository;
        public int CurrentImage { get; set; } = 1;

        public ImageService()
        {
            //   _imageRepository = new ImageRepository();
            _imageRepository = Injector.CreateInstance<IImageRepository>();
        }

        public List<Image> GetAll()
        {
            return _imageRepository.GetAll();
        }

        public Image Save(Image image)
        {
            return _imageRepository.Save(image);
        }

        public void Delete(Image image)
        {
            _imageRepository.Delete(image);
        }

        public Image Update(Image image)
        {
            return _imageRepository.Update(image);
        }

        public Image GetById(int id)
        {
            return _imageRepository.GetById(id);
        }

        public bool IsAlreadyInserted(Image image)
        {
            foreach (var iteration in _imageRepository.GetAll())
            {
                if (iteration.Url == image.Url)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetIdByUrl(string urlLink)
        {
            foreach (var image in _imageRepository.GetAll())
            {
                if (image.Url == urlLink)
                {
                    return image.Id;
                }
            }
            return -1;
        }

        /*private void ShowImage(string url)
        {
            try
            {
                var bitmap = new BitmapImage(new Uri(url));
                MyImage.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }*/

        /*private void ShowImages()
        {
            var images = _imageRepository.GetAll();
            ShowImage(GetImageUrl(CurrentImage));
        }*/

        public string ChangeImageForward()
        {
            if (_imageRepository.GetAll().Count > CurrentImage + 1)
                return GetImageUrl(++CurrentImage);
            CurrentImage = 1;
            return GetImageUrl(CurrentImage);
        }

        public string ChangeImageBackward()
        {
            if (CurrentImage - 1 > 0)
                return GetImageUrl(--CurrentImage);
            CurrentImage = _imageRepository.GetAll().Count - 1 ;
            return GetImageUrl(CurrentImage);
        }

        public string GetImageUrl(int imageId)
        {
            var images = _imageRepository.GetAll();
            return images.Find(image => image.Id == imageId).Url;
        }

        public List<Image> GetImagesByImageIds(List<int> imageIds)
        {
            List<Image> images = new List<Image>();
            foreach (var imageId in imageIds)
            {
                Image image = _imageRepository.GetById(imageId);
                images.Add(image);
            }
            return images;
        }


        /*public List<Image> GetTourImages(int tourId)
        {
            var tour = new TourService().GetById(tourId);
            return _imageRepository.GetAll().FindAll(image => image.Id == tour.ImageId);
        }*/
        public List<ImageGalleryDTO> GetImageGalleryDTO(Tour tour)
        {
            List<ImageGalleryDTO> imagesGalleryDTO = new List<ImageGalleryDTO>();
            foreach(var image in GetImagesByImageIds(tour.ImageId))
            {
                if (image.Url == tour.ThumbnailUrl)
                {
                    ImageGalleryDTO imageGalleryDTO = new ImageGalleryDTO(image.Id, image.Url, true);
                    imagesGalleryDTO.Add(imageGalleryDTO);
                }
                else
                {
                    ImageGalleryDTO imageGalleryDTO = new ImageGalleryDTO(image.Id, image.Url, false);
                    imagesGalleryDTO.Add(imageGalleryDTO);
                }
            }
            return imagesGalleryDTO;
        }
    }
}
