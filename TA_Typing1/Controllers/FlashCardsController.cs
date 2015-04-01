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
using iTextSharp.text;
using MvcRazorToPdf;
 

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
        public ActionResult Index(string date_query = "", string start_date = "", string end_date = "", bool fav_list = false)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            IEnumerable<FlashCard> Cards;
            if (date_query == "")
            {
                if (start_date != "" || end_date != "")
                {
                    string[] formats = { "MM/dd/yyyy", "MM-dd-yyyy" };
                    DateTime start_query_real;
                    DateTime end_query_real;
                    if (!DateTime.TryParseExact(start_date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out start_query_real) || !DateTime.TryParseExact(end_date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out end_query_real))
                    {
                        throw new KeyNotFoundException(); //no thing at all
                    }

                    start_query_real = Convert.ToDateTime(start_date);
                    end_query_real = Convert.ToDateTime(end_date);

                    @ViewBag.boardInfo = start_query_real.Date.ToString("dd-MM-yyyy") + " - " + end_query_real.Date.ToString("dd-MM-yyyy");

                    Cards = db.FlashCard.ToList().Where(w => w.word.User.Id == currentUser.Id).Where(w => w.word.CreatedTime >= start_query_real.Date && w.word.CreatedTime <= end_query_real.Date);
                }
                else
                {
                    DateTime currentDate = DateTime.Today;
                    @ViewBag.boardInfo = "Today: " + currentDate.Date.ToString("dd-MM-yyyy");

                    Cards = db.FlashCard.ToList().Where(w => w.word.User.Id == currentUser.Id).Where(w => w.word.CreatedTime.Date == currentDate.Date);
                }
            }
            else
            {
                string[] formats = { "MM/dd/yyyy", "MM-dd-yyyy" };
                DateTime date_query_real;
                if (!DateTime.TryParseExact(date_query, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date_query_real))
                {
                    throw new KeyNotFoundException(); //no thing at all
                }

                date_query_real = Convert.ToDateTime(date_query);

                @ViewBag.boardInfo = date_query_real.Date.ToString("dd-MM-yyyy");

                Cards = db.FlashCard.ToList().Where(w => w.word.User.Id == currentUser.Id).Where(w => w.word.CreatedTime.Date == date_query_real);
            }

            @ViewBag.date_query = date_query;
            @ViewBag.start_date = start_date;
            @ViewBag.end_date = end_date;

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
        public bool CreateFlashCard(Word word, string card_type)
        {
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
        public bool EditColor(InputColorModelView card)
        {
            FlashCard fcard = db.FlashCard.Find(card.id);
            if (fcard != null)
            {
                fcard.fColor = card.fColor;
                db.SaveChanges();
                return true;
            }

            return false;
        }

        // POST: FlashCard/EditFavourite/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool EditFavourite(InputFavouriteCardModelView cards)
        {
            FlashCard fcard = db.FlashCard.Find(cards.id);
            if (fcard != null)
            {
                fcard.fFavourite = cards.fFavourite;
                db.SaveChanges();
                return true;
            }

            return false;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PDF(string date_query = "", string start_date = "", string end_date = "", bool fav_list = false)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            IEnumerable<FlashCard> Cards;

            if (date_query == "")
            {
                if (start_date != "" || end_date != "")
                {
                    string[] formats = { "MM/dd/yyyy", "MM-dd-yyyy" };
                    DateTime start_query_real;
                    DateTime end_query_real;
                    if (!DateTime.TryParseExact(start_date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out start_query_real) || !DateTime.TryParseExact(end_date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out end_query_real))
                    {
                        throw new KeyNotFoundException(); //no thing at all
                    }

                    start_query_real = Convert.ToDateTime(start_date);
                    end_query_real = Convert.ToDateTime(end_date);

                    @ViewBag.boardInfo = start_query_real.Date.ToString("dd-MM-yyyy") + " - " + end_query_real.Date.ToString("dd-MM-yyyy");

                    Cards = db.FlashCard.ToList().Where(w => w.word.User.Id == currentUser.Id).Where(w => w.word.CreatedTime >= start_query_real.Date && w.word.CreatedTime <= end_query_real.Date);
                }
                else
                {
                    DateTime currentDate = DateTime.Today;
                    @ViewBag.boardInfo = "Today: " + currentDate.Date.ToString("dd-MM-yyyy");

                    Cards = db.FlashCard.ToList().Where(w => w.word.User.Id == currentUser.Id).Where(w => w.word.CreatedTime.Date == currentDate.Date);
                }
            }
            else
            {
                string[] formats = { "MM/dd/yyyy", "MM-dd-yyyy" };
                DateTime date_query_real;
                if (!DateTime.TryParseExact(date_query, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date_query_real))
                {
                    throw new KeyNotFoundException(); //no thing at all
                }

                date_query_real = Convert.ToDateTime(date_query);

                @ViewBag.boardInfo = date_query_real.Date.ToString("dd-MM-yyyy");

                Cards = db.FlashCard.ToList().Where(w => w.word.User.Id == currentUser.Id).Where(w => w.word.CreatedTime.Date == date_query_real);
            }

            @ViewBag.date_query = date_query;
            @ViewBag.start_date = start_date;
            @ViewBag.end_date = end_date;

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

            return new PdfActionResult(Cards, (writer, document) =>
            {
                document.SetPageSize(new Rectangle(500f, 500f, 90));
                document.NewPage();
            });
        }

        [HttpPost]
        public bool save_background(string bg_option)
        {
            HttpCookie aCookie = new HttpCookie("userInfo");
            aCookie.Values["backgroundUrl"] = bg_option;
            aCookie.Expires = DateTime.Now.AddDays(365);
            Response.Cookies.Add(aCookie);

            return true;
        }

        public bool save_grid_size(int grid_size)
        {
            HttpCookie aCookie = new HttpCookie("grid_size");
            aCookie.Value= grid_size.ToString();
            aCookie.Expires = DateTime.Now.AddDays(365);
            Response.Cookies.Add(aCookie);

            return true;
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