using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using TA_Typing1.Models;
using TA_Typing1.ViewModels;
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
        public ActionResult Index(string date_query = "", int week_query = 0, bool fav_list = false)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            IEnumerable<FlashCard> Cards;
            if (week_query < 0)
            {
                throw new FormatException();
            }

            if (date_query == "")
            {
                if (week_query == 0)
                {
                    DateTime currentDate = DateTime.Today;
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    DayOfWeek monDay = DayOfWeek.Monday;
                    int diff = (7 + (currentDay - monDay)) % 7;
                    int totalDays = diff;
                    @ViewBag.boardInfo = "This week: " + DateTime.Today.AddDays(-diff).Date.ToString("dd-MM-yyyy") + " - " + DateTime.Today.Date.ToString("dd-MM-yyyy");

                    Cards = db.FlashCard.ToList().Where(w => w.word.User.Id == currentUser.Id).Where(w => w.word.CreatedTime >= (DateTime.Today.AddDays(-diff)));
                }
                else
                {
                    DateTime currentDate = DateTime.Today;
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    DayOfWeek monDay = DayOfWeek.Monday;
                    int diff = (7 + (currentDay - monDay)) % 7;
                    int totalDays = diff + (week_query - 1) * 7;
                    @ViewBag.boardInfo = week_query + " week(s): " + DateTime.Today.AddDays(-totalDays).Date.ToString("dd-MM-yyyy") + " - " + DateTime.Today.Date.ToString("dd-MM-yyyy");

                    Cards = db.FlashCard.ToList().Where(w => w.word.User.Id == currentUser.Id).Where(w => w.word.CreatedTime >= (DateTime.Today.AddDays(-totalDays)));
                }
            }
            else
            {
                string[] formats = {"d/M/yyyy", "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "yyyy/MM/dd", "yyyy/M/dd", "yyyy/MM/d", "yyyy/M/d",
                                       "d-M-yyyy", "dd-MM-yyyy", "d-MM-yyyy", "dd-M-yyyy","yyyy-MM-dd", "yyyy-M-dd", "yyyy-MM-d", "yyyy-M-d" };
                DateTime date_query_real;
                if (!DateTime.TryParseExact(date_query, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date_query_real))
                {
                    throw new KeyNotFoundException(); //no thing at all
                }

                date_query_real = Convert.ToDateTime(date_query);

                @ViewBag.boardInfo = "Date: " + date_query_real.Date.ToString("dd-MM-yyyy");

                Cards = db.FlashCard.ToList().Where(w => w.word.User.Id == currentUser.Id).Where(w => w.word.CreatedTime.Date == date_query_real);
            }

            @ViewBag.date_query = date_query;
            @ViewBag.week_query = week_query;

            if (fav_list == true)
            {
                Cards = Cards.ToList().Where(card => card.fFavourite == true);
                @ViewBag.fav_title = "Favourite List";
                @ViewBag.fav_list = true.ToString();
            }

            foreach (var card in Cards)
            {
                // The list of word defs are added
                card.word.WordDefs = db.WordDefs.ToList().Where(worddef => worddef.wordId == card.word.Id);
            }

            return View(Cards);
        }

        // function called from the function create word in Word controller
        public bool CreateFlashCard(Word word, string card_type){
            FlashCard card = new FlashCard();

            card.createdTime = DateTime.Today;
            card.fColor = card_type;
            card.fFavourite = false;
            card.wordId = word.Id;

            db.FlashCard.Add(card);
            db.SaveChanges();
            return true;
        }

        // POST: FlashCard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ICollection<FlashCard> cards)
        {
            foreach (var card in cards)
            {
                FlashCard cardFounded = db.FlashCard.First(w => w.id == card.id);
                
                if (cardFounded == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    cardFounded.fFavourite = card.fFavourite;
                    cardFounded.fColor = card.fColor;
                    cardFounded.createdTime = DateTime.Today;
                    db.Entry(cardFounded).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("index");
        }

        // POST: FlashCard/EditFavourite/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFavourite(ICollection<FlashCard> cards)
        {
            foreach (var card in cards)
            {
                FlashCard cardFounded = db.FlashCard.First(w => w.id == card.id);

                if (cardFounded == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    cardFounded.fFavourite = card.fFavourite;
                    cardFounded.createdTime = DateTime.Today;
                    db.Entry(cardFounded).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("index");
        }


        // Statistic
        public ActionResult Statistic(DateTime dateOfWeek)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            int days_in_week = 7;
            DayOfWeek monDay = DayOfWeek.Monday;
            int diff = (7 + (dateOfWeek.DayOfWeek - monDay)) % 7;
            DateTime monDayOfWeek = dateOfWeek.AddDays(-diff);

            List<InputWordsStatistic> inputWords = new List<InputWordsStatistic>();

            for (int i = 0; i < days_in_week; ++i)
            {
                InputWordsStatistic wordsInDay = new InputWordsStatistic();
                wordsInDay.countWords = db.Words.ToList().Where(w => w.CreatedTime.Date == monDayOfWeek.AddDays(i).Date && w.User == currentUser).Count();
                wordsInDay.createdDate = monDayOfWeek.AddDays(i);
                wordsInDay.id = i;
                inputWords.Add(wordsInDay);
            }

            return View(inputWords);
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
