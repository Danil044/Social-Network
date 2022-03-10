using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Network.Data.social_network
{
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set;}


        public User Author { get; set; }

        public string Text { get; set; }

        public List<Like> Likes { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Images> Image { get; set; }

        public DateTime CreateAt { get; set; }

    }
}
