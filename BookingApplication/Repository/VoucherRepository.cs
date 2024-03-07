using BookingApplication.Domain.Model;
using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Interfaces.RepositoryInterfaces;

namespace BookingApplication.Repository
{
    class VoucherRepository : IVoucherRepository
    {
        private const string FilePath = "../../../Resources/Data/vouchers.csv";

        private readonly Serializer<Voucher> _serializer;

        private List<Voucher> _vouchers;

        private readonly VoucherRepository _voucherRepository;

        public VoucherRepository()
        {

            _serializer = new Serializer<Voucher>();
            _vouchers = _serializer.FromCSV(FilePath);

        }

        public List<Voucher> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _vouchers = _serializer.FromCSV(FilePath);
            if (_vouchers.Count < 1)
            {
                return 1;
            }
            return _vouchers.Max(c => c.Id) + 1;
        }

        public Voucher Save(Voucher tourDate)
        {
            tourDate.Id = NextId();
            _vouchers = _serializer.FromCSV(FilePath);
            _vouchers.Add(tourDate);
            _serializer.ToCSV(FilePath, _vouchers);
            return tourDate;
        }

        public void Delete(Voucher voucher)
        {
            _vouchers = _serializer.FromCSV(FilePath);
            Voucher founded = _vouchers.Find(c => c.Id == voucher.Id);
            _vouchers.Remove(founded);
            _serializer.ToCSV(FilePath, _vouchers);
        }

        public Voucher Update(Voucher voucher)
        {
            _vouchers = _serializer.FromCSV(FilePath);
            Voucher current = _vouchers.Find(c => c.Id == voucher.Id);
            int index = _vouchers.IndexOf(current);
            _vouchers.Remove(current);
            _vouchers.Insert(index, voucher);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _vouchers);
            return voucher;
        }

        public Voucher GetById(int id)
        {
            _vouchers = _serializer.FromCSV(FilePath);
            return _vouchers.Find(c => c.Id == id);
        }

    }
}
