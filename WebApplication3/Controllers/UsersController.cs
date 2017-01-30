using DBase.Abstact;
using DBase.Concrete;
using DBase.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    
    public class UsersController : Controller
    {
       // private Context context;
        private readonly UserRepository repository;
        public UsersController(IDbContext context)
        {
            //IDbContext context = new Context();
            repository = new UserRepository(context);
        }
        public ActionResult Index()// GET: Users
        {
            //var listUsers = context.Users;
            //IQueryable<UserViewModel> listUsers = from data in repository.GetAllUsers() select new UserViewModel() { ID = data.Id, Email = data.Email };
            IQueryable<UserViewModel> listUsers = from data in repository.GetAllUsers().Include(c=>c.UserProfile)
                                                  .Include(r=>r.Roles)
                                                  select new UserViewModel()
                                                  {
                                                      ID = data.Id,
                                                      Email = data.Email,
                                                      FullName = data.UserProfile != null ? data.UserProfile.LastName +" "+ data.UserProfile.Name +" "+ data.UserProfile.SecondName : "",
                                                      Roles=data.Roles.Select(r=>new RoleViewModel() { Id=r.Id, Name=r.Name})
                                                  };
            return View(listUsers);            
        }
        //user profile
        [Authorize]
        public ActionResult UserProfile()
        {
            var userEmail = User.Identity.Name;
            var user=repository.FindUserByEmail(userEmail);
            UserDetailViewModel model = new UserDetailViewModel()
            {
                Email = user.Email, ID = user.Id,
                Posts=user.Posts.Select(p=>new PostItemViewModel() {PostId=p.Id, PostName=p.Name })
            };

            return View(model);
        }

        //[Authorize(Roles = "Admin")]
        public ActionResult AddUserToPost()// GET: Users
        {
            Context context = new Context();
            AddUserToPostViewModel model = new AddUserToPostViewModel();
            //Заповнити всі пости
            model.ListPosts = context.Posts.Select(p => new ListBoxItems() { Id = p.Id, Name = p.Name });
            //Заповнити всі юзери
            model.ListUsers = context.Users.Select(p => new ListBoxItems() { Id = p.Id, Name = p.UserProfile.LastName+" "+ p.UserProfile.Name });
            return View(model);
        }
        [HttpPost]
        public ActionResult AddUserToPost(AddUserToPostViewModel model)// GET: Users
        {
            Context context = new Context();
            if(ModelState.IsValid)
            {
                Post findPost = context.Posts.SingleOrDefault(p => p.Id == model.PostIdSected);
                if(findPost!=null)
                {
                    User findUser = context.Users.SingleOrDefault(u => u.Id == model.UserIdSected);
                    if(findUser!=null)
                    {
                        findPost.Users.Add(findUser);
                        context.SaveChanges();
                    }
                }
            }
            //Заповнити всі пости
            model.ListPosts = context.Posts.Select(p => new ListBoxItems() { Id = p.Id, Name = p.Name });
            //Заповнити всі юзери
            model.ListUsers = context.Users.Select(p => new ListBoxItems() { Id = p.Id, Name = p.UserProfile.LastName + " " + p.UserProfile.Name });
            return View(model);
        }
        /*
        //[Authorize(Roles = "Admin")]
        public ActionResult AddRoleToUser()// GET: Users
        {

        }
        //[Authorize(Roles = "Admin")]
        public ActionResult AddRoleToUser()// GET: Users
        {

        }
        //[Authorize(Roles = "Admin")]
        public ActionResult AddRoleToPost()// GET: Users
        {

        }
        */
        public ActionResult Create()
        {
            CreateUserViewModel model = new CreateUserViewModel();
            model.RoleId = 1;
            var listRoles = repository.GetAllRoles().Select(r => new ListBoxItems () { Id=r.Id, Name= r.Name }).ToList();
            //listRoles.Insert(0, new ListBoxItems() { Id = 0, Name = "" });
            ViewBag.LisRoles = new SelectList(listRoles, "Id", "Name", model.RoleId);
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateUserViewModel model)
        {
            if(ModelState.IsValid)
            {
                //User user = new User() { Email = model.Email, Password = model.Password };
                //context.Users.Add(user);
                //context.SaveChanges();
                var user = repository.CreateUser(model.Email, model.Password, model.LastName, model.Name, model.SecondName);
                if (user != null)
                {
                    repository.AddRoleUser(user, model.RoleId);
                    return RedirectToAction("Index", "Users");

                }
                else
                {
                    ModelState.AddModelError("", "Користувач з таким Email вже існує");
                }
            }
            var listRoles = repository.GetAllRoles().Select(r => new ListBoxItems () { Id=r.Id, Name= r.Name }).ToList();
            //listRoles.Insert(0, new ListBoxItems() { Id = 0, Name = "" });
            ViewBag.LisRoles = new SelectList(listRoles, "Id", "Name", model.RoleId);
            //ViewBag.LisRoles = new MultiSelectList(listRoles, "Id", "Name", model.RoleId);
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            User user = repository.GetUserById(id);
            UserEditViewModel model = new UserEditViewModel()
            {
                ID = user.Id,
                Email = user.Email
            };
            return View(model); 
        }
        [HttpPost]
        public ActionResult Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                repository.EditeUser(model.ID, model.Email);
                repository.ChangePassword(model.ID, model.OldPassword, model.Password);
            }
            return RedirectToAction("Index", "Users");
            
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            User user = repository.GetUserById(id);
            UserInfoViewModel model=new UserInfoViewModel()
            {
                ID=user.Id,
                Email=user.Email
            };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Delete(UserInfoViewModel model)
        {
            repository.RemoveUserById(model.ID);
            return RedirectToAction("Index", "Users");
        }
        public ActionResult AddRole()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddRole(AddRoleViewModel model)
        {
            repository.AddRole(model.Name);
            return RedirectToAction("Index", "Users");
        }
        
    }
}