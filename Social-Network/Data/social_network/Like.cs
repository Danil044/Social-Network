using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Network.Data.social_network
{
    public class Like
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public int LikeInt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}
