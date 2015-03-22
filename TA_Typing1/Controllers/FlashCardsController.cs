using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using TA_Typing1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace TA_Typing1.Controllers
{
    [Authorize]
    public class FlashCardsController : Controller
    {
        private DateTime initVal = DateTime.MinValue;
        private MyDbContext db;
        private UserManager<ApplicationUser> manager;
        public FlashCardsController()
        {
            db = new MyDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        
        // GET: FlashCards
        public ActionResult Index(string date_query = "", int week_query = 0)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            IEnumerable<Word> Words;
            if (week_query < 0 )
            {
                throw new FormatException();
            }

            if (date_query == "")
            {
                if (week_query == 0)
                {
                    DateTime currentDate = DateTime.Today;
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    DayOfWeek monDay =  DayOfWeek.Monday;
                    int diff = (7 + (currentDay - monDay)) % 7;
                    int totalDays = diff;
                    @ViewBag.boardInfo = "This week: " + DateTime.Today.AddDays(-diff).Date.ToString("dd-MM-yyyy") + " - " + DateTime.Today.Date.ToString("dd-MM-yyyy");

                    Words = db.Words.ToList().Where(w => w.User.Id == currentUser.Id).Where(w => w.CreatedTime >= (DateTime.Today.AddDays(-diff)));
                }
                else
                {
                    DateTime currentDate = DateTime.Today;
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    DayOfWeek monDay =  DayOfWeek.Monday;
                    int diff = (7+(currentDay - monDay))%7;
                    int totalDays = diff + (week_query-1)*7;
                    @ViewBag.boardInfo = week_query + " week(s): " + DateTime.Today.AddDays(-totalDays).Date.ToString("dd-MM-yyyy") + " - " + DateTime.Today.Date.ToString("dd-MM-yyyy");

                    Words = db.Words.ToList().Where(w => w.User.Id == currentUser.Id).Where(w => w.CreatedTime >= (DateTime.Today.AddDays(-totalDays)));
                }
            }
            else{
                string[] formats = {"d/M/yyyy", "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "yyyy/MM/dd", "yyyy/M/dd", "yyyy/MM/d", "yyyy/M/d",
                                       "d-M-yyyy", "dd-MM-yyyy", "d-MM-yyyy", "dd-M-yyyy","yyyy-MM-dd", "yyyy-M-dd", "yyyy-MM-d", "yyyy-M-d" };
                DateTime date_query_real;
                if (!DateTime.TryParseExact(date_query, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date_query_real))
                {
                    throw new KeyNotFoundException(); //no thing at all
                } 

                date_query_real = Convert.ToDateTime(date_query);

                @ViewBag.boardInfo = "Date: " + date_query_real.Date.ToString("dd-MM-yyyy");

                Words = db.Words.ToList().Where(w => w.User.Id == currentUser.Id).Where(w => w.CreatedTime.Date == date_query_real);
            }
            

            foreach (var word in Words)
            {
                // The list of word defs are added
                word.WordDefs = db.WordDefs.ToList().Where(worddef => worddef.wordId == word.Id);
            }

            return View(Words);
        }

        // POST: Words/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ICollection<Word> words)
        {
            foreach (var word in words)
            {
                Word wordFounded = db.Words.First(w => w.Id == word.Id);
                if (wordFounded == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    wordFounded.fColor = word.fColor;
                    wordFounded.CreatedTime = DateTime.Now;

                    db.Entry(wordFounded).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
