using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Repository;

namespace BookingApplication.Services
{
    public class LanguageService
    {
        private readonly ITourRepository _tourRepository;

        public LanguageService() 
        {
            _tourRepository =  Injector.CreateInstance<ITourRepository>(); ;
        }

        public ObservableCollection<string> GetLanguages()
        {
            var tours = _tourRepository.GetAll();
            var languages = new ObservableCollection<string>();
            foreach (var tour in tours.Where(tour => !languages.Contains(tour.Language)))
            {
                languages.Add(tour.Language);
            }

            return languages;
        }
    }
}
