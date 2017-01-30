using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class UserDetailViewModel
    {
        public int ID { get; set; }
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        public IEnumerable<PostItemViewModel> Posts { get; set; }
    }
}