using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Models
{
    public class Business
    {
        public Business(string name, string address, string city, string state, string zip, double rating, string price)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            Zip = zip;
            Rating = rating;
            Price = price;
        }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public double Rating { get; set; }

        public string Price { get; set; }
    }
}
