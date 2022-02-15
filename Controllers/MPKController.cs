using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PartsWarehouse.Models;

namespace PartsWarehouse.Controllers
{
    public class MPKController : Controller
    {
        private readonly MagazynDBEntities db = new MagazynDBEntities();

        // GET: MPK
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var MPK = db.MPK.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                MPK = MPK.Where(m => m.Nazwa.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Name_desc":
                    MPK = MPK.OrderByDescending(m => m.Nazwa);
                    break;
                default:
                    MPK = MPK.OrderBy(m => m.Nazwa);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(MPK.ToPagedList(pageNumber, pageSize));
        }

        // GET: MPK/Details/5
        public ActionResult Details(int? id, int? page)
        {
            page = 1;
            var wydania = db.Wydania.Include(w => w.Kartoteki).Include(w => w.MPK).Include(w => w.Osoby);
            wydania = wydania.Where(w => w.Id_MPK == id);
            wydania = wydania.OrderByDescending(w => w.Data_Wydania);
            ViewBag.MPK = db.MPK.Find(id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(wydania.ToPagedList(pageNumber, pageSize));
        }

        // GET: MPK/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MPK/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_MPK,Nazwa")] MPK mPK)
        {
            if (ModelState.IsValid)
            {
                db.MPK.Add(mPK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mPK);
        }

        // GET: MPK/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MPK mPK = db.MPK.Find(id);
            if (mPK == null)
            {
                return HttpNotFound();
            }
            return View(mPK);
        }

        // POST: MPK/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_MPK,Nazwa")] MPK mPK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mPK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mPK);
        }

        // GET: MPK/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MPK mPK = db.MPK.Find(id);
            if (mPK == null)
            {
                return HttpNotFound();
            }
            return View(mPK);
        }

        // POST: MPK/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MPK mPK = db.MPK.Find(id);
            db.MPK.Remove(mPK);
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
