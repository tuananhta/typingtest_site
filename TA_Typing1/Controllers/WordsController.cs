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
    [Authorize]
    public class WordsController : Controller
    {
        private MyDbContext db;
        private UserManager<ApplicationUser> manager;
        public WordsController()
        {
            db = new MyDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Words
        public ActionResult Index()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
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
            else if(word.User != currentUser)
            {
                // not authorized
                return HttpNotFound();
            }

            // The list of word defs are added
            word.WordDefs = db.WordDefs.ToList().Where(worddef => worddef.wordId == id);

            return View(word);
        }

        // GET: Words/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Words/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="WContext, Level")] Word words)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            string defaultColor = "flash-default";
                if (ModelState.IsValid)
                {
                    ViewBag.redirectUrl = Url.Action("index");
                    char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\n' , '-', '_', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                    string[] word = words.WContext.Split(delimiterChars);

                    Word ipWord = words;
                    ipWord.Level = words.Level;
                    ipWord.CreatedTime = DateTime.Now;
                    ipWord.User = currentUser;

                    for (int i = 0; i < word.Length; ++i)
                    {
                        word[i] = word[i].Trim();

                        if(word[i] == "")
                            continue;                 

                        ipWord.WContext = word[i];
                        ipWord.fColor = defaultColor;
                        db.Words.Add(ipWord);
                        db.SaveChanges();
                    }
                    
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
            if (word.WContext != null)
            {
                word.WContext = word.WContext.Trim();
            }
            
            if (ModelState.IsValid)
            {
                word.CreatedTime = DateTime.Now;
                db.Entry(word).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.redirectUrl = Url.Action("index");
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

        // flash Cards function
        public ActionResult flashCards()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            IEnumerable<Word> Words = db.Words.ToList().Where(w => w.User.Id == currentUser.Id);

            foreach(var word in Words)
            {
                // The list of word defs are added
                word.WordDefs = db.WordDefs.ToList().Where(worddef => worddef.wordId == word.Id);
            }

            return View(Words);
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
