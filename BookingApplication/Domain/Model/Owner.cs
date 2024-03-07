using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Model
{
    public class Owner : User, ISerializable
    {
        public int Id { get; set; }
       // public string Username { get; set; }
      //  public string Password { get; set; }
       

        public List<Accommodation> accommodations { get; set; }


        public Owner()
        {
            accommodations = new List<Accommodation>();
        }

        public Owner(string username, string password)
        {
            Username = username;
            Password = password;
          
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            
        }
    }
}
