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
    public class PrzyjeciaController : Controller
    {
        private MagazynDBEntities db = new MagazynDBEntities();
        private static Przyjecia przyjeciePrzedEdycja;
        // GET: Przyjecia
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

            var przyjecia = db.Przyjecia.Include(p => p.Kartoteki);

            if (!String.IsNullOrEmpty(searchString))
            {
                przyjecia = przyjecia.Where(w => w.Kartoteki.Nazwa.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Name_desc":
                    przyjecia = przyjecia.OrderByDescending(p => p.Kartoteki.Nazwa);
                    break;
                case "Date":
                    przyjecia = przyjecia.OrderBy(p => p.Data_Przyjecia);
                    break;
                case "Name":
                    przyjecia = przyjecia.OrderBy(p => p.Kartoteki.Nazwa);
                    break;
                default:
                    przyjecia = przyjecia.OrderByDescending(p => p.Data_Przyjecia);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(przyjecia.ToPagedList(pageNumber, pageSize));
        }

        // GET: Przyjecia/Create
        public ActionResult Create()
        {
            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa");
            return View();
        }

        // POST: Przyjecia/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Przyjecia,Ilosc,Data_Przyjecia,Id_Kartoteki")] Przyjecia przyjecia)
        {
            if (ModelState.IsValid)
            {
                db.Przyjecia.Add(przyjecia);
                UpdateQuantity(przyjecia, przyjeciePrzedEdycja);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa", przyjecia.Id_Kartoteki);
            return View(przyjecia);
        }

        // GET: Przyjecia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Przyjecia przyjecia = db.Przyjecia.Find(id);
            if (przyjecia == null)
            {
                return HttpNotFound();
            }
            przyjeciePrzedEdycja = przyjecia;
            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa", przyjecia.Id_Kartoteki);
            return View(przyjecia);
        }

        // POST: Przyjecia/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Przyjecia,Ilosc,Data_Przyjecia,Id_Kartoteki")] Przyjecia przyjecia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(przyjecia).State = EntityState.Modified;
                UpdateQuantity(przyjecia, przyjeciePrzedEdycja);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa", przyjecia.Id_Kartoteki);
            return View(przyjecia);
        }

        // GET: Przyjecia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Przyjecia przyjecia = db.Przyjecia.Find(id);
            if (przyjecia == null)
            {
                return HttpNotFound();
            }
            return View(przyjecia);
        }

        // POST: Przyjecia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Przyjecia przyjecia = db.Przyjecia.Find(id);
            UpdateQuantityAfterDelete(przyjecia);
            db.Przyjecia.Remove(przyjecia);
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

        private void UpdateQuantity(Przyjecia przyjecie, Przyjecia przyjeciePrzedEdycja)
        {
            var kartoteka = FindKartoteka(przyjecie);
            if (przyjecie.Id_Przyjecia != 0)
            {
                int newAmount = CalculateNewAmount(przyjecie, przyjeciePrzedEdycja);
                kartoteka.Stan += newAmount;
            }
            else
            {
                kartoteka.Stan += przyjecie.Ilosc;
            }

        }

        private int CalculateNewAmount(Przyjecia przyjecie, Przyjecia przyjeciePrzedEdycja)
        {
            return przyjecie.Ilosc - przyjeciePrzedEdycja.Ilosc;
        }

        private void UpdateQuantityAfterDelete(Przyjecia przyjecie)
        {
            var kartoteka = FindKartoteka(przyjecie);
            kartoteka.Stan -= przyjecie.Ilosc; ;
        }

        private Kartoteki FindKartoteka(Przyjecia przyjecie)
        {
            return db.Kartoteki.FirstOrDefault(x => x.Id_Kartoteki == przyjecie.Id_Kartoteki);
        }
    }
}
