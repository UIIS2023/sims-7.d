using System.Collections.ObjectModel;
using BookingApplication.Domain.Model;
using BookingApplication.Services;

namespace BookingApplication.WPF.ViewModel.TourGuestViewModel
{


    public class VouchersVm
    {
        public ObservableCollection<Voucher> Vouchers { get; set; }

        public VouchersVm(User user, NavigationService navigationService)
        {
            var voucherService = new VoucherService();
            voucherService.RemoveExpiredVouchers();
            Vouchers = new ObservableCollection<Voucher>(voucherService.GetVouchersByGuestId(user.Id));

        }
    }
}
