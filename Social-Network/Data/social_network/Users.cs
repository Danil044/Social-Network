using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Social_Network.Data.social_network
{
    public class User : IdentityUser
    {
        public List<Friend> Friends { get; set;}
        public List<User> Folowers { get; set;}
        public List<User> Folowing { get; set;}
        
        public string TelegramId { get; set; }
        public string Description { get; set; }
        public string Surname_Name { get; set; }
        public string? AvatarURL { get; set; }
        public DateTime YearOfBirth { get; set; }
        public string Location { get; set;}
        public string? Speciality_or_SducationalInstitution { get; set;}
        public string? Status { get; set; }


        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
        public DateTime CreateAt { get; set; }

    }
}
