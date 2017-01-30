using DBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        public string PostName { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
        
    }
}