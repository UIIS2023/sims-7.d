using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Domain.Model;
using BookingApplication.DTO;

namespace BookingApplication.Services
{
    public class SimpleTourRequestService
    {
        private readonly ISimpleTourRequestRepository _simpleTourRequestRepository;
        private readonly LocationService _locationService;

        public SimpleTourRequestService()
        {
            _simpleTourRequestRepository = Injector.CreateInstance<ISimpleTourRequestRepository>();
            _locationService = new LocationService();
        }

        public List<SimpleTourRequest> GetAll()
        {
            var locationService = new LocationService();
            return locationService.GetLocationsForRequests(_simpleTourRequestRepository.GetAll());
        }

        public void UpdateTourRequest(SimpleTourRequest tourRequest)
        {
            _simpleTourRequestRepository.Update(tourRequest);
        }
        public SimpleTourRequest Save(SimpleTourRequest simpleTourRequest)
        {
            return _simpleTourRequestRepository.Save(simpleTourRequest);
        }

        public SimpleTourRequest GetById(int id)
        {
            return _simpleTourRequestRepository.GetById(id);
        }
        public void Update(SimpleTourRequest simpleTourRequest)
        {
            _simpleTourRequestRepository.Update(simpleTourRequest);
        }

        public void InvalidateRequests(ObservableCollection<SimpleTourRequest> requests)
        {
            foreach (var request in requests)
            {
                if ((DateTime.Today.AddDays(2) >= request.StartDate) && (request.Status == Status.Waiting))
                {
                    request.Status = Status.Denied;
                    Update(request);
                }
            }
        }

        public List<SimpleTourRequest> GetWaitingTourRequests()
        {
            List<SimpleTourRequest> tourRequests = new List<SimpleTourRequest>();
            foreach (var tourRequest in GetAll())
            {
                if (tourRequest.Status == Status.Waiting)
                {
                    tourRequests.Add(tourRequest);
                }
            }
            return tourRequests;
        }

        public ObservableCollection<SimpleTourRequest> FilterTourRequestsByDTO(TourRequestFilterDTO tourRequestFilterDTO)
        {
            var tourRequests = _simpleTourRequestRepository.GetAll();
            var filteredTourRequests = new ObservableCollection<SimpleTourRequest>();
            foreach (var tourRequest in tourRequests)
            {
                var isLanguage = string.IsNullOrEmpty(tourRequestFilterDTO.Language) || tourRequest.Language.ToLower().Contains(tourRequestFilterDTO.Language.ToLower());
                var isLocation = _locationService.IsTourRequestOnLocation(tourRequestFilterDTO.TourLocation, tourRequest);
                var isGuestsNumbers = (tourRequestFilterDTO.MaxGuestsNumber is null &&
                                          tourRequestFilterDTO.MinGuestsNumber is null)
                                         || (tourRequestFilterDTO.MaxGuestsNumber >= tourRequest.GuestsNumber &&
                                             tourRequestFilterDTO.MinGuestsNumber <= tourRequest.GuestsNumber);

                DateTime? nullableStartTime = tourRequestFilterDTO.StartDate;
                DateTime? nullableEndTime = tourRequestFilterDTO.EndDate;

                var isTourDate = (nullableStartTime is null &&
                                  nullableEndTime is null) || (nullableStartTime == DateTime.MinValue && nullableStartTime == DateTime.MinValue)
                                 || (DateTime.Compare(tourRequest.StartDate.Value, tourRequestFilterDTO.StartDate) >= 0 &&
                                 DateTime.Compare(tourRequest.EndDate.Value, tourRequestFilterDTO.EndDate) <= 0);

                if (isLanguage && isGuestsNumbers && isLocation && isTourDate)
                {
                    filteredTourRequests.Add(tourRequest);
                }
            }

            return filteredTourRequests;
        }
        

        public List<int> GetTourRequestStatistics(TourRequestFilterDTO tourRequestFilterDTO, int year, int month)
        {
            int totalRequestsNumber = 0;
            int acceptedRequestsNumber = 0;


            ObservableCollection<SimpleTourRequest> tourRequests = FilterTourRequestsByDTO(tourRequestFilterDTO);
            if (year != 0)
            {
                if (month != 0)
                {
                    tourRequests =
                        new ObservableCollection<SimpleTourRequest>(
                            FilterRequestsByYearAndMonthOnly(tourRequests, year, month));
                }
                else
                {
                    tourRequests =
                        new ObservableCollection<SimpleTourRequest>(FilterRequestsByYearOnly(tourRequests, year));
                }
            }
            else
            {
                tourRequests = new ObservableCollection<SimpleTourRequest>(GetAll());
            }

            foreach (var tourRequest in tourRequests)
            {
                if (tourRequest.Status == Status.Accepted)
                    acceptedRequestsNumber++;
                totalRequestsNumber++;
                
            }

            return new List<int>
            {
                totalRequestsNumber, acceptedRequestsNumber};
        }
        public List<SimpleTourRequest> GetRequestsByYear(int year)
        {
            return GetAll().Where(tourRequest => tourRequest.StartDate.Value.Year == year || tourRequest.EndDate.Value.Year == year).ToList();
        }

        public List<SimpleTourRequest> FilterRequestsByYearOnly(ObservableCollection<SimpleTourRequest> tourRequests,int year)
        {
            List<SimpleTourRequest> filteredTourRequests = new List<SimpleTourRequest>();
            foreach (var tourRequest in tourRequests)
            {
                if (tourRequest.Status == Status.Accepted)
                {
                    if (tourRequest.AcceptedDate.Value.Year == year)
                    {
                        filteredTourRequests.Add(tourRequest);
                    }
                }
                else
                {
                    if (tourRequest.StartDate.Value.Year == year || tourRequest.EndDate.Value.Year == year)
                    {
                        filteredTourRequests.Add(tourRequest);
                    }
                }
            }
            return filteredTourRequests;
        }

        public List<SimpleTourRequest> FilterRequestsByYearAndMonthOnly(ObservableCollection<SimpleTourRequest> tourRequests, int year, int month)
        {
            List<SimpleTourRequest> filteredTourRequests = new List<SimpleTourRequest>();
            foreach (var tourRequest in tourRequests)
            {
                if (tourRequest.Status == Status.Accepted)
                {
                    if (tourRequest.AcceptedDate.Value.Year == year && tourRequest.AcceptedDate.Value.Month == month)
                    {
                        filteredTourRequests.Add(tourRequest);
                    }
                }
                else
                {
                    if ((tourRequest.StartDate.Value.Year == year || tourRequest.EndDate.Value.Year == year) && 
                        (tourRequest.StartDate.Value.Month == month || tourRequest.EndDate.Value.Month == month))
                    {
                        filteredTourRequests.Add(tourRequest);
                    }
                }
            }
            return filteredTourRequests;
        }



        public string GetMostRequestedLanguage()
        {
            Dictionary<String, int> languages = new Dictionary<String, int>();
            foreach (var tourRequest in GetRequestsByYear(DateTime.Now.Year))
            {
                if (languages.ContainsKey(tourRequest.Language))
                {
                    languages[tourRequest.Language]++;
                }
                else
                {
                    languages.Add(tourRequest.Language, 1);
                }
            }

            foreach (var tourRequest in languages)
            {
                if (tourRequest.Value == languages.Values.Max())
                {
                    return tourRequest.Key;
                }
            }
            return null;
        }

        public int GetMostRequestedLocation()
        {
            Dictionary<int, int> locations = new Dictionary<int, int>();
            foreach (var tourRequest in GetRequestsByYear(DateTime.Now.Year))
            {
                if (locations.ContainsKey(tourRequest.LocationId.Value))
                {
                    locations[tourRequest.LocationId.Value]++;
                }
                else
                {
                    locations.Add(tourRequest.LocationId.Value, 1);
                }
            }

            foreach (var tourRequest in locations)
            {
                if (tourRequest.Value == locations.Values.Max())
                {
                    return tourRequest.Key;
                }
            }
            return 0;
        }

        public List<int> GetYearsOfAllTourRequests()
        {
            List<int> years = new List<int>();

            foreach (var tourRequest in GetAll())
            {
                if (!years.Contains(tourRequest.StartDate.Value.Year))
                {
                    years.Add(tourRequest.StartDate.Value.Year);
                }
                else if (!years.Contains(tourRequest.EndDate.Value.Year))
                {
                    years.Add(tourRequest.EndDate.Value.Year);
                }
            }
            return years;
        }

        public ObservableCollection<SimpleTourRequest> GetStatisticsByYear(DateTime? date)
        {
            var filteredTourRequests = new ObservableCollection<SimpleTourRequest>();
            if (date is null)
                return new ObservableCollection<SimpleTourRequest>(GetAll());
            foreach (var request in GetAll())
            {
                if (request.StartDate.Value.Year == date.Value.Year && request.EndDate.Value.Year == date.Value.Year)
                {
                    filteredTourRequests.Add(request);
                }
            }
            return filteredTourRequests;
        }

        public int GetRequestNumberByLanguage(string language, ObservableCollection<SimpleTourRequest> requests, int userId)
        {
            int number = 0;
            foreach (var tourRequest in requests)
            {
                if (tourRequest.Language == language && tourRequest.UserId == userId)
                {
                    number++;
                }
            }
            return number;
        }

        public int GetAverageGroupSize(ObservableCollection<SimpleTourRequest> requests)
        {
            

            var average = 0;
            var count = 0;
            foreach (var request in requests)
            {
                if (request.Status == Status.Accepted)
                {
                    average += (int)request.GuestsNumber;
                    count++;
                }
            }
            if (requests.Count == 0 || count == 0 || average == 0)
            {
                return 0;
            }
            return average / count;
        }

        private List<string> GetLanguagesForDeniedRequests(int userId)
        {
            var languages = new List<string>();
            foreach (var request in GetAll())
            {
                if(request.Status == Status.Denied && request.UserId == userId)
                    languages.Add(request.Language);
            }
            return languages;
        }

        private List<int?> GetLocationsForDeniedRequests(int userId)
        {
            var locations = new List<int?>();
            foreach (var request in GetAll())
            {
                if (request.Status == Status.Denied && request.UserId == userId)
                    locations.Add(request.LocationId);
            }
            return locations;
        }

        public ObservableCollection<Tour> GetToursWithMatchingLocationOrLanguage(int userId)
        {
            var toursWithMatchingLocationOrLanguage = new ObservableCollection<Tour>();
            var tourService = new TourService();
            var deniedLanguages = GetLanguagesForDeniedRequests(userId);
            var deniedLocations = GetLocationsForDeniedRequests(userId);

            foreach (var tour in tourService.GetNewTours())
            {
                if (deniedLanguages.Contains(tour.Language) || deniedLocations.Contains(tour.LocationId))
                {
                    toursWithMatchingLocationOrLanguage.Add(tour);
                }
            }

            return toursWithMatchingLocationOrLanguage;
        }

        public List<SimpleTourRequest> GetAllAcceptedRequests(int userId)
        {
            return GetAll().Where(request => request.Status == Status.Accepted && request.UserId == userId).ToList();
        }

        public List<SimpleTourRequest> GetUnnotifiedAcceptedRequests(int userId)
        {
            return GetAll().Where(request => request is { Status: Status.Accepted, IsSeen: false } && request.UserId == userId).ToList();
        }

        public void SetRequestsAsNotified(int userId)
        {
            foreach (var request in GetUnnotifiedAcceptedRequests(userId).Where(request => !request.IsSeen))
            {
                request.IsSeen = true;
                Update(request);
            }
        }

        public List<SimpleTourRequest> GetAllRequestsByUser(int userId)
        {
            return GetAll().Where(request => request.UserId == userId).ToList();
        }
    }
}
