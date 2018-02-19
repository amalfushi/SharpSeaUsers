using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDashboard.Models;

namespace UserDashboard.Controllers
{
    public class DashboardController : Controller
    {
        private UserDashboardContext _context;
        // private PasswordHasher<User> _hasher;

        public DashboardController(UserDashboardContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            int? LogId = HttpContext.Session.GetInt32("UserId");
            if (LogId == null)
            {
                return Redirect("/");
            }
            User loggedUser = _context.users.Where(u => u.UserId == LogId).SingleOrDefault();
            ViewBag.LoggedUserName = loggedUser.FirstName;

            List<User> users = _context.users.ToList();

            if (loggedUser.UserLevel == 9)
            {
                // should probably just be a ViewBag.Admin == true;
                ViewBag.Admin = true;
                // return View("AdminDashboard");
            }
            return View(users);
        }

        [HttpGet]
        [Route("user/{UID}")]
        public IActionResult ViewUser(int UID)
        {
            int? LogId = HttpContext.Session.GetInt32("UserId");
            if (LogId == null)
            {
                return Redirect("/");
            }
            User loggedUser = _context.users.Include(u => u.Conversations).ThenInclude(c => c.Messages).Where(u => u.UserId == LogId).SingleOrDefault();
            loggedUser.Conversations = loggedUser.Conversations.OrderByDescending(c => c.UpdatedAt);
            foreach (Conversation conv in loggedUser.Conversations)
            {
                conv.Messages = conv.Messages.OrderBy(m => m.CreatedAt).ToList();
            }

            User targetUser;

            if (UID != loggedUser.UserId)
            {
                targetUser = _context.users.Include(u => u.Conversations).ThenInclude(c => c.Messages).Where(u => u.UserId == UID).SingleOrDefault();
                ///this seems janky couldn't do it here ^  for some reason^
                targetUser.Conversations = targetUser.Conversations.OrderByDescending(c => c.UpdatedAt);
                foreach (Conversation convs in targetUser.Conversations)
                {
                    convs.Messages = convs.Messages.OrderBy(m => m.CreatedAt).ToList();
                }
                ///
            }
            else
            {
                targetUser = loggedUser;
                return View("ViewSelf", targetUser);
            }

            return View(targetUser);

        }

        [HttpPost]
        [Route("newConversation")]
        public IActionResult MessageUser(int recipient, string message)
        {
            int? LogId = HttpContext.Session.GetInt32("UserId");
            if (LogId == null)
            {
                return Redirect("/");
            }
            DateTime now = DateTime.Now;

            Conversation nc = new Conversation();
            nc.RecipientId = recipient;
            nc.CreatedAt = now;
            nc.UpdatedAt = now;

            _context.conversations.Add(nc);
            _context.SaveChanges();

            Message nm = new Message();

            nm.Text = message;
            nm.ConversationId = nc.ConversationId;
            nm.SenderId = (int)LogId;
            nm.CreatedAt = now;
            nm.UpdatedAt = now;

            _context.messages.Add(nm);
            _context.SaveChanges();

            return Redirect("/dashboard");
        }

        [HttpPost]
        [Route("newMessage")]
        public IActionResult NewMessage(int convId, string text)
        {
            int? LogId = HttpContext.Session.GetInt32("UserId");
            if (LogId == null)
            {
                return Redirect("/");
            }
            // System.Console.WriteLine(conv);
            // int convId = Int32.Parse(conv);
            Conversation conversation = _context.conversations.Where(c => c.ConversationId == convId).SingleOrDefault();

            Message nm = new Message();
            nm.Conversation = conversation;
            nm.SenderId = (int) LogId;
            nm.Text = text;

            DateTime now = DateTime.Now;

            nm.CreatedAt = now;
            nm.UpdatedAt = now;

            conversation.UpdatedAt = now;

            _context.messages.Add(nm);
            _context.conversations.Update(conversation);

            _context.SaveChanges();
            return Redirect($"/user/{conversation.RecipientId}");
        }
    }
}
