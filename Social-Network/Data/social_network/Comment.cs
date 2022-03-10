﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Network.Data.social_network
{
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Text { get; set; }
        [Required]
        public Post Post { get; set;}
        [Required]

        public User Author { get; set; }
        
        public Comment? ParentComments { get; set; }
        public List<Comment> ChidrenComments { get; set; }
        
        public DateTime CreateAt { get; set; }
    }
}
