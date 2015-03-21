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

namespace TA_Typing1.Controllers
{
    public class RecordsController : Controller
    {
        private MyDbContext db;
        private UserManager<ApplicationUser> manager;
        public RecordsController()
        {
            db = new MyDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Records
        public ActionResult publicRecords()
        {
            return View(db.Records.ToList().Where(record => record.pri_rec == false).OrderByDescending(record => record.wpm));
        }
        [Authorize]
        public async Task<ActionResult> privateRecords()
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            return View(db.Records.ToList().Where(record => record.User.Id == currentUser.Id).OrderByDescending(record => record.CreatedTime));
        }
        // POST: Records/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind(Include = "wpm, pri_rec")] Record record)
        {
            
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());

                if (ModelState.IsValid)
                {
                    record.CreatedTime = DateTime.Now;
                    record.User = currentUser;

                    db.Records.Add(record);
                    db.SaveChanges();

                    return RedirectToAction("publicRecords");
                }

            return View();
        }

        // GET: Records/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Record record = db.Records.Find(id);
            db.Records.Remove(record);
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
