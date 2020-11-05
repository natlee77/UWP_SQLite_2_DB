using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcceessLibrary.Models
{
    public class Comment
    { 
        public Comment()
    {

    }

        public Comment(long id, long orderId, string description, DateTime created)
    {
        Id = id;
        OrderId = orderId;
        Description = description;
        Created = created;
    }

        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
