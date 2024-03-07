using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using BookingApplication.Serializer;

namespace BookingApplication.Domain.Model
{
    public class Image : ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        private int _id;
        private string _url;
        
        public Image(string url)
        {
            Url = url;
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
                    OnPropertyChanged();
                }
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                if (_url != value)
                {
                    _url = value;
                    OnPropertyChanged();
                }
            }
        }

        public Image() { }


        bool IsValidUrl(string url)
        {
            string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx.IsMatch(url);
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Url")
                {
                    if (string.IsNullOrEmpty(Url) || !IsValidUrl(Url))
                        return "It's not in a correct form!";
                }


                return null;
            }
        }

        private readonly string[] validatedProperties = { "Url" };

        public bool IsValid
        {
            get
            {
                foreach (var property in validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), Url
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Url = values[1];

        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }

}