using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Windows.UI.Xaml;

namespace DataAcceessLibrary.Models
{
    public class Customer
    {
        public Customer()
        {
            

        }

        public Customer(long id, string firstName, string lastName, string adress, string city, int postCode, DateTime created)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Adress = adress;
            City = city;
            PostCode = postCode;
            Created = created;
        }

      
        public long Id { get; set; }       
        public string FirstName { get; set; }        
        public string LastName { get; set; }        
        public string Adress { get; set; }       
        public string City { get; set; }
        public int PostCode { get; set; }
        public DateTime Created { get; set; }


        public string Name => "@ {FirstName } {LastName} ";



       
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
