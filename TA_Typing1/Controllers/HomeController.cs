using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TA_Typing1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace TA_Typing1.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext db;
        private UserManager<ApplicationUser> manager;
        
        public HomeController()
        {
            db = new MyDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public ActionResult Index(string optPrg = "1_pb", string skp_ad = "off")
        {
            ViewBag.optPrg = optPrg;    // the option of the program
            ViewBag.skp_ad = skp_ad;    // skip the advertisement
            ApplicationUser adminID = db.Users.Where(user => user.UserName =="admin").First();

            if (User.Identity.IsAuthenticated)
            {
                var currentUser = manager.FindById(User.Identity.GetUserId());

                // to check if the words database is empty
                int count = 0;
                IEnumerable<Word> wordAmount = db.Words.ToList().Where(word => word.User.Id == currentUser.Id);
                foreach (var item in wordAmount)
                {
                    count++;
                }
                ViewBag.count = count;
            }
    
            switch (optPrg)
            {
                case "1_pb":
                    ViewBag.wordList = db.Words.ToList().Where(word => word.User.Id == adminID.Id && word.Level == 1);
                    return View();
                case "2_pb":
                    ViewBag.wordList = db.Words.ToList().Where(word => word.User.Id == adminID.Id && (word.Level == 1 || word.Level == 2));
                    return View();
                case "3_pb":
                    ViewBag.wordList = db.Words.ToList().Where(word => word.User.Id == adminID.Id && (word.Level == 1 || word.Level == 2 || word.Level == 3));
                    return View();
                case "1_pr":
                    if (User.Identity.IsAuthenticated)
                    {
                        var currentUser = manager.FindById(User.Identity.GetUserId());
                        ViewBag.wordList = db.Words.ToList().Where(word => word.User.Id == currentUser.Id);
                        return View();
                    }
                    else
                        return View();
                default:
                    return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}