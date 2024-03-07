using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApplication.Domain.Regexes;
using BookingApplication.Serializer;


namespace BookingApplication.Domain.Model
{
    public class TourPoint : ValidationBase, ISerializable
    {
        private int _id;
        private string _name;
        private int _tourId;
        private int _order;
        private bool _active;
        private int _imageId;

        public TourPoint() { }
        public TourPoint(string name, int tourId, int order, bool active, int imageId)
        {
            Name = name;
            TourId = tourId;
            Order = order;
            Active = active;
            ImageId = imageId;
        }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public int TourId
        {
            get
            {
                return _tourId;
            }
            set
            {
                if (_tourId != value)
                {
                    _tourId = value;
                    OnPropertyChanged("TourID");
                }
            }
        }

        public int Order
        {
            get
            {
                return _order;
            }
            set
            {
                if (_order != value)
                {
                    _order = value;
                    OnPropertyChanged("Order");
                }
            }
        }

        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    OnPropertyChanged("Active");
                }
            }
        }

        public int ImageId
        {
            get
            {
                return _imageId;
            }
            set
            {
                if (_imageId != value)
                {
                    _imageId = value;
                    OnPropertyChanged("ImageID");
                }
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), Name, TourId.ToString(), Order.ToString(), Active.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            TourId = Convert.ToInt32(values[2]);
            Order = Convert.ToInt32(values[3]);
            Active = Convert.ToBoolean(values[4]);
        }

        protected override void ValidateSelf()
        {
            NameRegex nameRegex = new NameRegex();
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                this.ValidationErrors["Name"] = "Name is required.";
            }
            else if (!nameRegex.Regex.IsMatch(this.Name))
            {
                this.ValidationErrors["Name"] = "You can only use letters and numbers in a name.";
            }
            
        }

    }

}
