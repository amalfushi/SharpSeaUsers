using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserDashboard.Models;

namespace UserDashboard.Controllers
{
    public class UserController : Controller
    {
        private UserDashboardContext _context;
        private PasswordHasher<User> _hasher;

        public UserController(UserDashboardContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
        }

        //***************Start Page */
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        //***************Register Page */
        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("createUser")]
        public IActionResult CreateUser(RegisterUser u)
        {
            if (ModelState.IsValid)
            {
                List<User> users = _context.users.Where(nu => nu.Email == u.Email).ToList();
                if (users.Count > 0)
                {//checks for already used email
                    ViewBag.InvalidEmail = true;
                    return View("Register");
                }

                //generate new user
                User nUser = new User { FirstName = u.FirstName, LastName = u.LastName, Email = u.Email };
                nUser.Password = _hasher.HashPassword(nUser, u.Password);
                if (_context.users.Count() == 0)
                {
                    nUser.UserLevel = 9;
                }
                else
                {
                    nUser.UserLevel = 1;
                }
                nUser.CreatedAt = DateTime.Now;
                
                _context.Add(nUser);
                _context.SaveChanges();

                HttpContext.Session.SetInt32("UserId", nUser.UserId);
                return Redirect("/login");
            }
            return View("Register");
        }

        //***************Login Page */
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            LoginUser lazyLogin = new LoginUser { LogEmail = "dps@gmail.com", LogPassword = "password" };
            // return ValidateLogin(lazyLogin);
            return View();
        }

        [HttpPost]
        [Route("validateLogin")]
        public IActionResult ValidateLogin(LoginUser lu)
        {
            if (ModelState.IsValid)
            {
                User user = _context.users.Where(u => u.Email == lu.LogEmail).SingleOrDefault();

                if (user != null)
                {
                    if (_hasher.VerifyHashedPassword(user, user.Password, lu.LogPassword) != 0)
                    {
                        HttpContext.Session.SetInt32("UserId", user.UserId);
                        return Redirect("/dashboard");
                    }
                }

                ViewBag.InvalidLogin = true;
                return View("Login");
            }
            return View("Login");
        }

        //*****************Logout */
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        //*****************User editing */
        [HttpGet]
        [Route("edit/{UID}")]
        public IActionResult EditUser(int UID)
        {
            int? LogId = HttpContext.Session.GetInt32("UserId");
            if (LogId == null)
            {
                return Redirect("/");
            }
            User user = _context.users.Where(u => u.UserId == UID).SingleOrDefault();

            return View("EditUser", user);
        }

        [HttpPost]
        [Route("SaveUserChanges")]
        public IActionResult SaveUserChanges(User eu)
        {
            int? LogId = HttpContext.Session.GetInt32("UserId");
            if (LogId == null)
            {
                return Redirect("/");
            }
            if (ModelState.IsValid)
            {
                User user = _context.users.Where(u => u.Email == eu.Email).SingleOrDefault();
                System.Console.WriteLine(user);
                System.Console.WriteLine(user.Email);
                user.Email = eu.Email;
                user.FirstName = eu.FirstName;
                user.LastName = eu.LastName;

                if (eu.Description != null)
                {
                    user.Description = eu.Description;
                }

                System.Console.WriteLine(eu.UserLevel);
                if (eu.UserLevel != 0)
                {
                    user.UserLevel = eu.UserLevel;
                }

                _context.SaveChanges();
                return Redirect("/dashboard");
            }
            return View("EditUser");
        }

        //*****************User deletion*/
        [HttpGet]
        [Route("delete/{UID}")]
        public IActionResult DeleteUser(int UID)
        {
            System.Console.WriteLine("get fucked");
            int? LogId = HttpContext.Session.GetInt32("UserId");
            if (LogId == null)
            {
                return Redirect("/");
            }

            User loggedUser = _context.users.Where(u => u.UserId == LogId).SingleOrDefault();

            User delUser = _context.users.Where(u => u.UserId == UID).SingleOrDefault();

            if (loggedUser.UserLevel != 9 && delUser.UserLevel != loggedUser.UserId)
            {
                ViewBag.Errors = "You don't have enough permissions to do that!";
            }
            else
            {
                _context.users.Remove(delUser);
                _context.SaveChanges();
            }

            return Redirect("/dashboard");
        }
    }


}
