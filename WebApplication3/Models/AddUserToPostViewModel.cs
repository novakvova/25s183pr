using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class AddUserToPostViewModel
    {
        public IEnumerable<ListBoxItems> ListUsers { get; set; }
        public IEnumerable<ListBoxItems> ListPosts { get; set; }

        public int UserIdSected { get; set; }
        public int PostIdSected { get; set; }
    }
}