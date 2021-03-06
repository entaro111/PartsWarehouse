using PagedList;
using PartsWarehouse.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PartsWarehouse.Controllers
{
    public class OsobyController : Controller
    {
        private readonly MagazynDBEntities db = new MagazynDBEntities();

        // GET: Osoby
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.DzialSortParm = sortOrder == "Dzial" ? "Dzial_desc" : "Dzial";

            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var osoby = db.Osoby.Include(o => o.Dzialy);
            if (!String.IsNullOrEmpty(searchString))
            {
                osoby = osoby.Where(o => o.Nazwisko.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Name_desc":
                    osoby = osoby.OrderByDescending(o => o.Nazwisko);
                    break;
                case "Dzial":
                    osoby = osoby.OrderBy(o => o.Dzialy.Nazwa);
                    break;
                case "Dzial_desc":
                    osoby = osoby.OrderByDescending(o => o.Dzialy.Nazwa);
                    break;
                default:
                    osoby = osoby.OrderBy(o => o.Nazwisko);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(osoby.ToPagedList(pageNumber, pageSize));
        }

        // GET: Osoby/Details/5
        public ActionResult Details(int? id, int? page)
        {
            page = 1;
            var wydania = db.Wydania.Include(w => w.Kartoteki).Include(w => w.MPK).Include(w => w.Osoby);
            wydania = wydania.Where(w => w.Id_Osoby == id);
            wydania = wydania.OrderByDescending(w => w.Data_Wydania);
            ViewBag.Osoba = db.Osoby.Find(id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(wydania.ToPagedList(pageNumber, pageSize));
        }

        // GET: Osoby/Create
        public ActionResult Create()
        {
            ViewBag.Id_Dzial = new SelectList(db.Dzialy, "Id_Dzialy", "Nazwa");
            return View();
        }

        // POST: Osoby/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Osoby,Imie,Nazwisko,Id_Dzial")] Osoby osoby)
        {
            if (ModelState.IsValid)
            {
                db.Osoby.Add(osoby);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Dzial = new SelectList(db.Dzialy, "Id_Dzialy", "Nazwa", osoby.Id_Dzial);
            return View(osoby);
        }

        // GET: Osoby/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Osoby osoby = db.Osoby.Find(id);
            if (osoby == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Dzial = new SelectList(db.Dzialy, "Id_Dzialy", "Nazwa", osoby.Id_Dzial);
            return View(osoby);
        }

        // POST: Osoby/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Osoby,Imie,Nazwisko,Id_Dzial")] Osoby osoby)
        {
            if (ModelState.IsValid)
            {
                db.Entry(osoby).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Dzial = new SelectList(db.Dzialy, "Id_Dzialy", "Nazwa", osoby.Id_Dzial);
            return View(osoby);
        }

        // GET: Osoby/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Osoby osoby = db.Osoby.Find(id);
            if (osoby == null)
            {
                return HttpNotFound();
            }
            return View(osoby);
        }

        // POST: Osoby/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Osoby osoby = db.Osoby.Find(id);
            db.Osoby.Remove(osoby);
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

        public JsonResult fullNameExist(string Nazwisko, string Imie, int? Id_Osoby)
        {
            return Json(IsFullNameUnique(Nazwisko, Imie, Id_Osoby));
        }

        private bool IsFullNameUnique(string Surname, string Name, int? id)
        {
            if (id == 0) return !db.Osoby.Any(x => x.Imie == Name && x.Nazwisko == Surname);
            else return !db.Osoby.Any(x => x.Imie == Name && x.Nazwisko == Surname && x.Id_Osoby != id);
        }
    }
}
