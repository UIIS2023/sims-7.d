using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApplication.Serializer;

namespace BookingApplication.Domain.Model
{
    public enum RescheduleRequestState
    {
        ON_HOLD = 0,
        APPROVED = 1,
        DECLINED = 2
    }

    public class ReservationRescheduleRequest : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime NewDateStart { get; set; }
        public DateTime NewDateEnd { get; set; }
        public RescheduleRequestState State { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            NewDateStart = DateTime.ParseExact(values[2], DateTimeFormat.Format, CultureInfo.InvariantCulture);
            NewDateEnd = DateTime.ParseExact(values[3], DateTimeFormat.Format, CultureInfo.InvariantCulture);
            State = (RescheduleRequestState)Convert.ToInt32(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                ReservationId.ToString(),
                NewDateStart.ToString(DateTimeFormat.Format, CultureInfo.InvariantCulture),
                NewDateEnd.ToString(DateTimeFormat.Format, CultureInfo.InvariantCulture),
                ((int)State).ToString()
            };
            return csvValues;
        }
    }
}