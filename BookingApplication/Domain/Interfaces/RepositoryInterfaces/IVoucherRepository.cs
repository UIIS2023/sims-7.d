using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface IVoucherRepository
    {
        public List<Voucher> GetAll();
        public int NextId();
        public Voucher Save(Voucher tourDate);
        public void Delete(Voucher voucher);
        public Voucher Update(Voucher voucher);
        public Voucher GetById(int id);
    }
}
