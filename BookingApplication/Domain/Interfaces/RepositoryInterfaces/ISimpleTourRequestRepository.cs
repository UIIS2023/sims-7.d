using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Domain.Model;

namespace BookingApplication.Domain.Interfaces.RepositoryInterfaces
{
    public interface ISimpleTourRequestRepository
    {
        public List<SimpleTourRequest> GetAll();
        public SimpleTourRequest Save(SimpleTourRequest simpleTourRequest);
        public int NextId();
        public void Delete(SimpleTourRequest simpleTourRequest);
        public SimpleTourRequest Update(SimpleTourRequest simpleTourRequest);
        public SimpleTourRequest GetById(int id);
    }
}
