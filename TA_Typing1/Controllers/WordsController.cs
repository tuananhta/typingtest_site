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
    public class WordsController : Controller
    {
        static string fColor = "flash-default";
        private MyDbContext db;
        private UserManager<ApplicationUser> manager;
        public WordsController()
        {
            db = new MyDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Words
        public ActionResult Index(string s_word = null, string s_date = "")
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            IEnumerable<Word> Words;
            if (s_word != null)
            {
                Words = db.Words.ToList().Where(w => w.WordDetail.WContext.ToLower().Contains(s_word.ToLower()));              
                return View(Words);
            }
            else if (s_date != "")
            {
                string[] formats = {"d/M/yyyy", "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "yyyy/MM/dd", "yyyy/M/dd", "yyyy/MM/d", "yyyy/M/d",
                                       "d-M-yyyy", "dd-MM-yyyy", "d-MM-yyyy", "dd-M-yyyy","yyyy-MM-dd", "yyyy-M-dd", "yyyy-MM-d", "yyyy-M-d" };
                DateTime date_query_real;
                if (!DateTime.TryParseExact(s_date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date_query_real))
                {
                    throw new KeyNotFoundException(); //no thing at all
                }

                date_query_real = Convert.ToDateTime(s_date);

                Words = db.Words.ToList().Where(w => w.CreatedTime.Date == date_query_real);
                return View(Words);
            }

            return View(db.Words.ToList().Where(word => word.User.Id == currentUser.Id));
        }

        // GET: Words/Details/5
        public ActionResult Details(int? id)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Word word = db.Words.Find(id);

            if (word == null)
            {
                return HttpNotFound();
            }
            else if (word.User != currentUser)
            {
                // not authorized
                return HttpNotFound();
            }

            IEnumerable<Word> nextWords = db.Words.ToList().Where(w => w.Id > word.Id && w.User.Id == currentUser.Id).OrderByDescending( w=> w.Id);
            IEnumerable<Word> prevWords = db.Words.ToList().Where(w => w.Id > 0 && w.Id < word.Id && w.User.Id == currentUser.Id).OrderByDescending(w=>w.Id);
            if (nextWords.Count() == 0)
            {
                @ViewBag.nextWord = null;
            }
            else
            {
                @ViewBag.nextWord = nextWords.Last().Id;
            }

            if (prevWords.Count() == 0)
            {
                @ViewBag.prevWord = null;
            }
            else
            {
                @ViewBag.prevWord = prevWords.First().Id;
            }
            // The list of word defs are added
            word.WordDefs = db.WordDefs.ToList().Where(worddef => worddef.wordId == word.Id);

            return View(word);
        }

        // GET: Words/Create
        public ActionResult CreateSingle()
        {
            return View();
        }

        // POST: Words/CreateSingle
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSingle(Word word)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());          

            if (ModelState.IsValid)
            {
                word.WordDetail.WContext = word.WordDetail.WContext.Trim();
                word.WordDetail.Level = 1; // phrase
                word.User = currentUser;
                word.CreatedTime = DateTime.UtcNow;
                word.WordDetail.CreatedTime = DateTime.UtcNow;
                db.Words.Add(word);
                db.WordDetail.Add(word.WordDetail);
                db.SaveChanges();
                ViewBag.redirectUrl = Url.Action("details", "words", new { id = word.Id });
                new FlashCardsController().CreateFlashCard(word, fColor);

                return PartialView("_RedirectPage");
            }

            return View();
        }

        // GET: Words/CreateMultiple
        public ActionResult CreateMultiple()
        {
            return View();
        }

        // POST: Words/CreateMultiple
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMultiple(Word words)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            if (ModelState.IsValid)
                {
                    ViewBag.redirectUrl = Url.Action("index");
                    char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\n' , '-', '_', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    string[] word = words.WordDetail.WContext.Split(delimiterChars);

                    Word ipWord = words;
                    ipWord.WordDetail.Level = words.WordDetail.Level;
                    ipWord.User = currentUser;

                    for (int i = 0; i < word.Length; ++i)
                    {
                        word[i] = word[i].Trim();

                        if(word[i] == "")
                            continue;                 

                        ipWord.WordDetail.WContext = word[i];
                        ipWord.CreatedTime = DateTime.UtcNow;
                        ipWord.WordDetail.WType = 1; // word
                        ipWord.WordDetail.CreatedTime = ipWord.CreatedTime;
                        db.Words.Add(ipWord);
                        db.WordDetail.Add(ipWord.WordDetail);
                        db.SaveChanges();
                        new FlashCardsController().CreateFlashCard(ipWord, fColor);
                    }
                    
                    return PartialView("_RedirectPage");
                }

            return View();
        }

        // GET: Words/CreatePhrase
        public ActionResult CreatePhrase()
        {
            return View();
        }

        // POST: Words/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePhrase(Word word)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                word.User = currentUser;
                word.CreatedTime = DateTime.UtcNow;
                word.WordDetail.CreatedTime = word.CreatedTime;
                word.WordDetail.WType = 2;
                db.Words.Add(word);
                db.WordDetail.Add(word.WordDetail);
                db.SaveChanges();
                ViewBag.redirectUrl = Url.Action("details", "words", new { id = word.Id });
                new FlashCardsController().CreateFlashCard(word, fColor);
                    
                return PartialView("_RedirectPage");
            }

            return View();
        }

        // GET: Words/Edit/5
        public ActionResult Edit(int? id)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word word = db.Words.Find(id);
            if (word == null)
            {
                return HttpNotFound();
            }
            else if (word.User != currentUser)
            {
                // not authorized
                return HttpNotFound();
            }

            return View(word);
        }

        // POST: Words/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Word word)
        {
            if (ModelState.IsValid)
            {
                WordDetail wDetail = db.WordDetail.First(wd => wd.Id == word.WordDetail.Id);
                wDetail.CreatedTime = DateTime.Today;
                wDetail.WContext = word.WordDetail.WContext;
                wDetail.WPronounce = word.WordDetail.WPronounce;
                word.WordDetail = wDetail;
                //word.CreatedTime = word.CreatedTime.AddDays(-1);

                db.Entry(word).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.redirectUrl = Url.Action("details", "words", new { id = word.Id });
                return PartialView("_RedirectPage");
            }

            return View(word);
        }

        // GET: Words/Delete/5
        public ActionResult Delete(int? id)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word word = db.Words.Find(id);
            if (word == null)
            {
                return HttpNotFound();
            }
            else if (word.User != currentUser)
            {
                // not authorized
                return HttpNotFound();
            }

            return View(word);
        }

        // POST: Words/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Word word = db.Words.Find(id);
            db.Words.Remove(word);
            db.SaveChanges();
            return RedirectToAction("Index");
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
