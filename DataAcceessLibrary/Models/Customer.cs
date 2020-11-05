using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Windows.UI.Xaml;

namespace DataAcceessLibrary.Models
{
    public  class Customer
    {
        public Customer()
        {
            Customers = new HashSet<Customer>();

        }

        public Customer(long id, string firstName, string lastName, string adress, string city, int postCode , DateTime created)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Adress = adress;
            City = city;
            PostCode = postCode;
            Created = created;
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(50)]

        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100)]
        public string Adress { get; set; }
        [Required]
        [StringLength(100)]
        public string City { get; set; }
        public int PostCode { get; set; }
        public DateTime Created { get; set; }



        [InverseProperty("Customer")]
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
