using BookingApplication.Serializer;
using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace BookingApplication.Domain.Model
{
    public class Guest : User, ISerializable
    {
        public string UserType { get; set; }
        public bool SuperGuest { get; set; }
        public int SuperGuestPoints { get; set; } = 0;
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
   

        public List<AccommodationReservation> accommodationsReservations { get; set; }


        public Guest()
        {
            accommodationsReservations = new List<AccommodationReservation>();
        }

        public Guest(string username, string password, string userType)
        {
            Username = username;
            Password = password;
            UserType = userType;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, UserType };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            UserType = values[3];
        }
    }
}