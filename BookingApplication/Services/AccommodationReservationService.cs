using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Model;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using BookingApplication.Repository;

namespace BookingApplication.Services
{
    public class AccommodationReservationService
    {
        private readonly IAccommodationReservationRepository reservationRepository;
      //  private AccommodationReservationRepository reservationRepository { get; set; } = new AccommodationReservationRepository();
        private AccommodationRepository accommodationRepository { get; set; } = new AccommodationRepository();

        private GuestRepository GuestsRepository { get; set; } = new GuestRepository();
        public AccommodationReservationService()
        {
            reservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
        }
        public class AcccommodationReservationData
        {
            public Accommodation Accommodation { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public int GuestsNumber { get; set; }
            public int ReservationId { get; set; }
        
        }

        public class DateTimeRange
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public int GetDays()
            {
                return (EndDate - StartDate).Days;
            }

            public override string ToString()
            {
                return $"{StartDate.ToString("d")} - {EndDate.ToString("d")}";
            }

            public bool Overlaps(DateTimeRange range)
            {
                return
                    (StartDate <= range.StartDate && EndDate >= range.EndDate) ||
                    (StartDate >= range.StartDate && EndDate <= range.EndDate) ||
                    (StartDate <= range.StartDate && EndDate >= range.StartDate) ||
                    (StartDate <= range.EndDate && EndDate >= range.EndDate);
            }
            public bool InsideRange(DateTime dateTime)
            {
                return dateTime >= StartDate && dateTime <= EndDate;
            }
        }
        public bool CheckAvailability(AcccommodationReservationData reservationData)
        {
            var allReservations = reservationRepository.GetAll();

            bool retval = true;

            foreach (var res in allReservations)
            {
                DateTimeRange resRange = new DateTimeRange()
                {
                    StartDate = res.StartDate,
                    EndDate = res.EndDate
                };

                DateTimeRange desiredRange = new DateTimeRange()
                {
                    StartDate = reservationData.DateFrom,
                    EndDate = reservationData.DateTo
                };

                if (res.AccommodationId == reservationData.Accommodation.Id && desiredRange.Overlaps(resRange))
                {
                    retval = false;
                    break;
                }
            }

            retval &= reservationData.GuestsNumber <= reservationData.Accommodation.MaxGuests;
            retval &= (reservationData.DateFrom - DateTime.Now).Days <= reservationData.Accommodation.MinReservationDays;

            return retval;
        }
        public List<DateTimeRange> FindOtherAvailableDates(AcccommodationReservationData reservationData)
        {
            List<DateTimeRange> otherAvailableDates = new List<DateTimeRange>();
            DateTimeRange selectedRange = new DateTimeRange()
            {
                StartDate = reservationData.DateFrom,
                EndDate = reservationData.DateTo
            };

            List<DateTimeRange> reservedRanges = reservationRepository.GetAll()
                .Where(x => x.AccommodationId == reservationData.Accommodation.Id)
                .Select(x =>
                {
                    return new DateTimeRange()
                    {
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    };
                }
            ).ToList();

            int maxDaysDifference = 30;
            int rangeDays = selectedRange.GetDays();
            int maxSuggestedDates = 3;

            DateTime differentStartDay = selectedRange.StartDate.AddDays(-1);

            while ((selectedRange.StartDate - differentStartDay).Days < maxDaysDifference)
            {
                DateTimeRange possibleRange = new DateTimeRange
                {
                    StartDate = differentStartDay,
                    EndDate = differentStartDay.AddDays(rangeDays)
                };

                bool isRangeAvailable = reservedRanges.All(x =>
                {
                    return !possibleRange.Overlaps(x);
                });

                if (isRangeAvailable)
                {
                    otherAvailableDates.Add(possibleRange);
                    if (otherAvailableDates.Count > maxSuggestedDates) { break; }
                }

                differentStartDay = differentStartDay.AddDays(-1);
            }

            differentStartDay = selectedRange.StartDate.AddDays(1);

            while ((differentStartDay - selectedRange.StartDate).Days < maxDaysDifference)
            {
                DateTimeRange possibleRange = new DateTimeRange
                {
                    StartDate = differentStartDay,
                    EndDate = differentStartDay.AddDays(rangeDays)
                };

                bool isRangeAvailable = reservedRanges.All(x =>
                {
                    return !possibleRange.Overlaps(x);
                });

                if (isRangeAvailable)
                {
                    otherAvailableDates.Add(possibleRange);
                    if (otherAvailableDates.Count > maxSuggestedDates) { break; }
                }

                differentStartDay = differentStartDay.AddDays(1);
            }

            return otherAvailableDates;
        }
        public void Reserve(AccommodationReservation accommodationReservation, User guest)
        {
            accommodationReservation.ReservationId = reservationRepository.GetAll().Max(x => x.ReservationId) + 1;
            reservationRepository.Save(accommodationReservation);

        }
        public List<AcccommodationReservationData> GetUpcomingReservationsForGuest(User guest)
        {
            List<Accommodation> allAccomodations = accommodationRepository.GetAll();
            List<AcccommodationReservationData> upcomingReservations = new List<AcccommodationReservationData>();

            List<AccommodationReservation> reservationsForGuest = reservationRepository.GetAll()
                .Where(x => x.GuestId == guest.Id).ToList();

            reservationsForGuest.ForEach(x =>
            {
                if (x.StartDate > DateTime.Now)
                {
                    Accommodation? currAccommodation = allAccomodations.FirstOrDefault(a => a.Id == x.AccommodationId);
                    if (currAccommodation != null)
                    {
                        upcomingReservations.Add(new AcccommodationReservationData()
                        {
                            Accommodation = currAccommodation,
                            DateFrom = x.StartDate,
                            DateTo = x.EndDate,
                            ReservationId = x.ReservationId
                        });
                    }
                }
            });

            return upcomingReservations;
        }
        public List<AcccommodationReservationData> GetPastReservationsForGuest(User guest)
        {
            List<Accommodation> allAccomodations = accommodationRepository.GetAll();
            List<AcccommodationReservationData> pastReservations = new List<AcccommodationReservationData>();

            List<AccommodationReservation> reservationsForGuest = reservationRepository.GetAll()
                .Where(x => x.GuestId == guest.Id).ToList();

            reservationsForGuest.ForEach(x =>
            {
                if (x.EndDate <= DateTime.Now)
                {
                    Accommodation? currAccommodation = allAccomodations.FirstOrDefault(a => a.Id == x.AccommodationId);
                    if (currAccommodation != null)
                    {
                        pastReservations.Add(new AcccommodationReservationData()
                        {
                            Accommodation = currAccommodation,
                            DateFrom = x.StartDate,
                            DateTo = x.EndDate,
                            ReservationId = x.ReservationId,
                        });
                    }
                }
            });

            return pastReservations;
        }

        public bool CancelReservation(AcccommodationReservationData reservationData)
        {
            if (reservationData.Accommodation.DaysBeforeCancelling <= (reservationData.DateFrom - DateTime.Now).Days)
            {
                reservationRepository.Delete(new AccommodationReservation()
                {
                    ReservationId = reservationData.ReservationId
                });
                return true;
            }
            else
            {
                return false;
            }
        }

        public Guest CheckSuperGuestCompleted(User guest)
        {
            bool isSuperGuest = false;

            int superGuestNumberOfReservations = 2;

            if (guest is Guest guestObj)
            {
                var oneYearRange = new DateTimeRange()
                {
                    StartDate = DateTime.Now.AddYears(-1),
                    EndDate = DateTime.Now
                };

                int previousYearNofReservations = reservationRepository.GetAll()
                    .Where(x => x.GuestId == guest.Id)
                    .Where(x => oneYearRange.InsideRange(x.StartDate))
                    .Count();

                bool justBecameSuperGuest = previousYearNofReservations == superGuestNumberOfReservations + 1;

                if (justBecameSuperGuest)
                {
                    guestObj.SuperGuestPoints = isSuperGuest ? 5 : 0;
                }

                guestObj.SuperGuest = guestObj.SuperGuestPoints > 0;
                GuestsRepository.Update(guestObj);
                return guestObj;
            }
            else
            {
                return null;
            }
        }
        public List<AccommodationReservation> GetAll()
        {
            return reservationRepository.GetAll();
        }
        public AccommodationReservation SaveAccommodationReservation(AccommodationReservation accommodationReservation)
        {
            return reservationRepository.Save(accommodationReservation);
        }

        public AccommodationReservation UpdateAccommodationReservation(AccommodationReservation accommodationReservation)
        {
            return reservationRepository.Update(accommodationReservation);
        }

    }
}
