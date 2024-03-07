using BookingApplication.Domain.Model;
using BookingApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;
using System.Xml.Linq;

namespace BookingApplication.Services
{
    public class VoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly TourReservationService _tourReservationService;
        private readonly TourDateService _tourDateService;
        public VoucherService()
        {
            _voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourReservationService = new TourReservationService();
            _tourDateService = new TourDateService();
        }

        public void DeleteVoucher(Voucher voucher)
        {
            _voucherRepository.Delete(voucher);
        }

        public void SaveVoucher(Voucher voucher)
        {
            _voucherRepository.Save(voucher);
        }

        public Voucher UpdateVoucher(Voucher voucher)
        {
            return _voucherRepository.Update(voucher);
        }

        public List<Voucher> GetAllVouchers()
        {
            return _voucherRepository.GetAll();
        }

        public void AssignVouchersToGuestsByTour(Tour tour)
        {
            foreach (TourReservation tourReservation in _tourReservationService.GetAllTourReservations())
            {
                if (tourReservation.TourId == tour.Id)
                {
                    Voucher voucher = new Voucher(tourReservation.UserId, tour.GuideId, "$100 off on any purchase -Name: \"100OFF\"", DateTime.Now,
                    DateTime.Now.AddDays(365), 365);
                    SaveVoucher(voucher);
                }
            }
        }

        public void AssignVouchersToGuestsByTourDates(TourDate tourDate, Tour tour)
        {
            foreach (TourReservation tourReservation in _tourReservationService.GetReservationsByTourDate(tourDate))
            {
                if (tourReservation.TourId == tour.Id)
                {
                    Voucher voucher = new Voucher(tourReservation.UserId, tour.GuideId, "$100 off on any purchase -Name: \"100OFF\"", DateTime.Now,
                        DateTime.Now.AddDays(365), 365);
                    SaveVoucher(voucher);
                }
            }
        }

        public void RemoveExpiredVouchers()
        {
            foreach (var voucher in GetAllVouchers())
            {
                if (voucher.ExpirationDate < DateTime.Today)
                {
                    DeleteVoucher(voucher);
                }
            }
        }

        public List<Voucher> GetVouchersByGuestId(int guestId)
        {
            return _voucherRepository.GetAll().Where(voucher => voucher.GuestId == guestId && voucher.ExpirationDate > DateTime.Today).ToList();
        }

    }
}
