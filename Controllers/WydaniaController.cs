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
    public class WydaniaController : Controller
    {
        private MagazynDBEntities db = new MagazynDBEntities();
        private static Wydania wydaniePrzedEdycja;
        // GET: Wydania
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "Date" : "";
            ViewBag.NameSortParam = sortOrder == "Name" ? "Name_desc" : "Name";
            

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var wydania = db.Wydania.Include(w => w.Kartoteki).Include(w => w.MPK).Include(w => w.Osoby);

            if (!String.IsNullOrEmpty(searchString))
            {
                wydania = wydania.Where(w => w.Kartoteki.Nazwa.ToUpper().Contains(searchString.ToUpper()) || w.Osoby.Nazwisko.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Name_desc":
                    wydania = wydania.OrderByDescending(w => w.Kartoteki.Nazwa);
                    break;
                case "Date":
                    wydania = wydania.OrderBy(w => w.Data_Wydania);
                    break;
                case "Name":
                    wydania = wydania.OrderBy(w => w.Kartoteki.Nazwa);
                    break;
                default:
                    wydania = wydania.OrderByDescending(w => w.Data_Wydania);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(wydania.ToPagedList(pageNumber, pageSize));
        }

        // GET: Wydania/Create/id
        public ActionResult Create(int id)
        {
            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa", id);
            ViewBag.Id_MPK = new SelectList(db.MPK, "Id_MPK", "Nazwa");
            ViewBag.Id_Osoby = new SelectList(db.Osoby, "Id_Osoby", "Imie");
            return View();
        }

        // POST: Wydania/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Wydania,Ilosc,Data_Wydania,Id_MPK,Id_Osoby,Id_Kartoteki")] Wydania wydania)
        {

            if (ModelState.IsValid)
            {
                db.Wydania.Add(wydania);
                UpdateQuantity(wydania, wydaniePrzedEdycja);
                db.SaveChanges();
                return RedirectToAction("Index","Kartoteki");
            }

            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa", wydania.Id_Kartoteki);
            ViewBag.Id_MPK = new SelectList(db.MPK, "Id_MPK", "Nazwa", wydania.Id_MPK);
            ViewBag.Id_Osoby = new SelectList(db.Osoby, "Id_Osoby", "Imie", wydania.Id_Osoby);
            return View(wydania);
        }

        // GET: Wydania/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wydania wydania = db.Wydania.Find(id);
            if (wydania == null)
            {
                return HttpNotFound();
            }
            wydaniePrzedEdycja = wydania;
            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa", wydania.Id_Kartoteki);
            ViewBag.Id_MPK = new SelectList(db.MPK, "Id_MPK", "Nazwa", wydania.Id_MPK);
            ViewBag.Id_Osoby = new SelectList(db.Osoby, "Id_Osoby", "Imie", wydania.Id_Osoby);
            return View(wydania);
        }

        // POST: Wydania/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Wydania,Ilosc,Data_Wydania,Id_MPK,Id_Osoby,Id_Kartoteki")] Wydania wydania)
        {

            if (ModelState.IsValid)
            {
                db.Entry(wydania).State = EntityState.Modified;
                UpdateQuantity(wydania, wydaniePrzedEdycja);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa", wydania.Id_Kartoteki);
            ViewBag.Id_MPK = new SelectList(db.MPK, "Id_MPK", "Nazwa", wydania.Id_MPK);
            ViewBag.Id_Osoby = new SelectList(db.Osoby, "Id_Osoby", "Imie", wydania.Id_Osoby);
            return View(wydania);
        }

        // GET: Wydania/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wydania wydania = db.Wydania.Find(id);
            if (wydania == null)
            {
                return HttpNotFound();
            }
            return View(wydania);
        }

        // POST: Wydania/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wydania wydania = db.Wydania.Find(id);
            UpdateQuantityAfterDelete(wydania);
            db.Wydania.Remove(wydania);
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
        public JsonResult stanKartoteki(int Ilosc, int? Id_Kartoteki)
        {
            var kartoteka = db.Kartoteki.FirstOrDefault(x => x.Id_Kartoteki == Id_Kartoteki);
            if (kartoteka.Stan - Ilosc > 0) return Json(true, JsonRequestBehavior.AllowGet);
            else return Json(false, JsonRequestBehavior.DenyGet);
        }

        private void UpdateQuantity(Wydania wydanie, Wydania wydaniePrzedEdycja)
        {
            var kartoteka = FindKartoteka(wydanie);
            if (wydanie.Id_Wydania != 0)
            {
                int newAmount = CalculateNewAmount(wydanie, wydaniePrzedEdycja);
                kartoteka.Stan -= newAmount;
            }
            else
            {
                kartoteka.Stan -= wydanie.Ilosc;
            }

        }

        private int CalculateNewAmount(Wydania wydanie, Wydania wydaniePrzedEdycja)
        {
            return wydanie.Ilosc - wydaniePrzedEdycja.Ilosc;
        }

        private void UpdateQuantityAfterDelete(Wydania wydanie)
        {
            var kartoteka = FindKartoteka(wydanie);
            kartoteka.Stan += wydanie.Ilosc; ;
        }

        private Kartoteki FindKartoteka(Wydania wydanie)
        {
            return db.Kartoteki.FirstOrDefault(x => x.Id_Kartoteki == wydanie.Id_Kartoteki);
        }


    }
}
