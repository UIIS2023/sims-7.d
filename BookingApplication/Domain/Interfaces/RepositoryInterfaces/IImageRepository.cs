using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Model;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface IImageRepository
    {
        public List<Image> GetAll();
        public Image Save(Image image);
        public int NextId();
        public void Delete(Image image);
        public Image Update(Image image);
        public Image GetById(int id);
    }
}
