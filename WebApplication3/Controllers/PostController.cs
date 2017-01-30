using DBase.Abstact;
using DBase.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using System.Data.Entity;
using DBase.Entity;

namespace WebApplication3.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        private readonly PostRepository repository;
        public PostController(IDbContext context)
        {
            //IDbContext context = new Context();
            repository = new PostRepository(context);
        }

        //admin profile
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()// GET: Users
        {
            
            IEnumerable<PostViewModel> model = (from data in repository.GetAllPosts().Include(c => c.Users)
                                                  select new PostViewModel()
                                                  {
                                                      PostId=data.Id,
                                                      PostName=data.Name,
                                                      Users = data.Users.Select(r => new UserViewModel() { ID = r.Id, Email = r.Email }).ToList()
                                                  }).ToList();

            return View(model);
        }





        }
}