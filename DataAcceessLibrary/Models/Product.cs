using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcceessLibrary.Models
{
    public class Product
    {
       
        public Product()
        {
             
        }

        public Product(long id, string name, string description, decimal price,string status)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Status = status;
        }

      
        public long Id { get; set; } 
        public string Name { get; set; }      
        public string Description { get; set; }        
        public decimal Price { get; set; }        
        public string Status { get; set; }
       

        public virtual ICollection< Product>  Products { get; set; }
    }
}
