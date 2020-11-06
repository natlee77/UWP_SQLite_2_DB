using DataAcceessLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcceessLibrary.Models
{
    public  class Order
    {
        public Order()
        {
            
        }

        public Order(long id, long customerId, long productId, int quantity, string description, string status, DateTime created)
        {
            Id = id;
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
            Description = description;
            Status = status;
            Created = created;
        }

       
        public long  Id { get; set; }
        public long  CustomerId { get; set; }    
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }

        




        public virtual  Customer Customer { get; set; } //populera kund info / virtual -skapa virtuel
      
        public  virtual Product Product { get; set; } //POPULERA PRODUCT INFO
        public virtual ICollection<Comment> Comments { get; set; } //koppling till alla comment
    }
}
