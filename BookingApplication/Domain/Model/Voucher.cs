using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using BookingApplication.Serializer;

namespace BookingApplication.Domain.Model
{
    public class Voucher : ISerializable, INotifyPropertyChanged
    {
        private int _id;
        private int _guestId;
        private int _guideId;
        private string _name;
        private DateTime _awardDate;
        private DateTime _expirationDate;
        private int _duration;

        public Voucher() { }

        public Voucher(int guestId, int guideId, string name, DateTime awardDate, DateTime expirationDate, int duration)
        {
            GuestId = guestId;
            GuideId = guideId;
            Name = name;
            AwardDate = awardDate;
            ExpirationDate = expirationDate;
            Duration = duration;
        }

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GuestId
        {
            get => _guestId;
            set
            {
                if (_guestId != value)
                {
                    _guestId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GuideId
        {
            get => _guideId;
            set
            {
                if (_guideId != value)
                {
                    _guideId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime AwardDate
        {
            get => _awardDate;
            set
            {
                if (_awardDate != value)
                {
                    _awardDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set
            {
                if (_expirationDate != value)
                {
                    _expirationDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {

                Id.ToString(), GuestId.ToString(), GuideId.ToString(), Name, AwardDate.ToString(), ExpirationDate.ToString(), Duration.ToString()
            };
            return csvValues;
        }

        public override string ToString()
        {
            return Name;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            GuideId = Convert.ToInt32(values[2]);
            Name = values[3];
            AwardDate = Convert.ToDateTime(values[4]);
            ExpirationDate = Convert.ToDateTime(values[5]);
            Duration = Convert.ToInt32(values[6]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
