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
    public class KartotekiController : Controller
    {
        private readonly MagazynDBEntities db = new MagazynDBEntities();

        // GET: Kartoteki
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";

            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var kartoteki = db.Kartoteki.Include(k => k.Dostawcy).Include(k => k.JM);

            if (!String.IsNullOrEmpty(searchString))
            {
                kartoteki = kartoteki.Where(k => k.Nazwa.ToUpper().Contains(searchString.ToUpper()) || k.Kod.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Name_desc":
                    kartoteki = kartoteki.OrderByDescending(k => k.Nazwa);
                    break;
                default:
                    kartoteki = kartoteki.OrderBy(k => k.Nazwa);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(kartoteki.ToPagedList(pageNumber, pageSize));
        }

        // GET: Kartoteki/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kartoteki kartoteki = db.Kartoteki.Find(id);
            if (kartoteki == null)
            {
                return HttpNotFound();
            }
            ViewBag.LastWydania = kartoteki.Wydania.OrderByDescending(w => w.Data_Wydania).Take(10).ToList();
            return View(kartoteki);
        }

        // GET: Kartoteki/Create
        public ActionResult Create()
        {
            ViewBag.Id_Dostawcy = new SelectList(db.Dostawcy, "Id_Dostawcy", "Nazwa");
            ViewBag.Id_JM = new SelectList(db.JM, "Id_JM", "Nazwa");
            return View();
        }

        // POST: Kartoteki/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Kartoteki,Nazwa,Stan,Niski_Stan,Miejsce,Id_JM,Id_Dostawcy,Kod")] Kartoteki kartoteki)
        {

            if (ModelState.IsValid)
            {
                db.Kartoteki.Add(kartoteki);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Dostawcy = new SelectList(db.Dostawcy, "Id_Dostawcy", "Nazwa", kartoteki.Id_Dostawcy);
            ViewBag.Id_JM = new SelectList(db.JM, "Id_JM", "Nazwa", kartoteki.Id_JM);
            return View(kartoteki);

        }

        // GET: Kartoteki/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kartoteki kartoteki = db.Kartoteki.Find(id);
            if (kartoteki == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Dostawcy = new SelectList(db.Dostawcy, "Id_Dostawcy", "Nazwa", kartoteki.Id_Dostawcy);
            ViewBag.Id_JM = new SelectList(db.JM, "Id_JM", "Nazwa", kartoteki.Id_JM);
            return View(kartoteki);
        }

        // POST: Kartoteki/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Kartoteki,Nazwa,Stan,Niski_Stan,Miejsce,Id_JM,Id_Dostawcy,Kod")] Kartoteki kartoteki)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kartoteki).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Dostawcy = new SelectList(db.Dostawcy, "Id_Dostawcy", "Nazwa", kartoteki.Id_Dostawcy);
            ViewBag.Id_JM = new SelectList(db.JM, "Id_JM", "Nazwa", kartoteki.Id_JM);
            return View(kartoteki);
        }

        // GET: Kartoteki/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kartoteki kartoteki = db.Kartoteki.Find(id);
            if (kartoteki == null)
            {
                return HttpNotFound();
            }
            return View(kartoteki);
        }

        // POST: Kartoteki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kartoteki kartoteki = db.Kartoteki.Find(id);
            db.Kartoteki.Remove(kartoteki);
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
        [HttpPost]
        public JsonResult nameExist(string Nazwa, int? Id_Kartoteki)
        {

            return Json(IsNameUnique(Nazwa, Id_Kartoteki));

        }
        [HttpPost]
        public JsonResult codeExist(string Kod, int? Id_Kartoteki)
        {
            return Json(IsCodeUnique(Kod, Id_Kartoteki));
        }

        private bool IsNameUnique(string Name, int? id)
        {
            if (id == 0) return !db.Kartoteki.Any(x => x.Nazwa == Name);
            else return !db.Kartoteki.Any(x => x.Nazwa == Name && x.Id_Kartoteki != id);
        }

        private bool IsCodeUnique ( string Code, int? id)
        {
            if (id == 0) return !db.Kartoteki.Any(x => x.Kod == Code);
            else return !db.Kartoteki.Any(x => x.Kod == Code && x.Id_Kartoteki != id);
        }

        public ActionResult NewWydanie(int id)
        {
            return RedirectToAction("Create", "Wydania", new { id = id });
        }

        public ActionResult NewPrzyjecie(int id)
        {
            return RedirectToAction("Create", "Przyjecia", new { id = id });
        }

    }
}
