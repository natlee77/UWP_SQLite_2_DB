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
             Products = new HashSet< Product>();
        }

        public Product(long id, string name, string description, decimal price,string status)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Status = status;
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        
        public string Status { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection< Product>  Products { get; set; }
    }
}
