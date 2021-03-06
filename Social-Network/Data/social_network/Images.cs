using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Network.Data.social_network
{
    public class Images
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string URL { get; set;}
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
