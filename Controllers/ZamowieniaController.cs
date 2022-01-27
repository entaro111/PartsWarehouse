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
    public class ZamowieniaController : Controller
    {
        private readonly MagazynDBEntities db = new MagazynDBEntities();
        // GET: Zamowienia
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


            var zamowienia = db.Zamowienia.Include(z => z.Kartoteki);

            if (!String.IsNullOrEmpty(searchString))
            {
                zamowienia = zamowienia.Where(z => z.Kartoteki.Nazwa.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Name_desc":
                    zamowienia = zamowienia.OrderByDescending(z => z.Kartoteki.Nazwa);
                    break;
                case "Date":
                    zamowienia = zamowienia.OrderBy(z => z.Data_zamowienia);
                    break;
                case "Name":
                    zamowienia = zamowienia.OrderBy(z => z.Kartoteki.Nazwa);
                    break;
                default:
                    zamowienia = zamowienia.OrderByDescending(z => z.Data_zamowienia);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(zamowienia.ToPagedList(pageNumber, pageSize));
        }

        // GET: Zamowienia/Create
        public ActionResult Create()
        {
            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa");
            return View();
        }

        // POST: Zamowienia/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Zamowienia,Data_zamowienia,Realizacja,Id_Kartoteki,Ilosc")] Zamowienia zamowienia)
        {
            if (ModelState.IsValid)
            {
                db.Zamowienia.Add(zamowienia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa", zamowienia.Id_Kartoteki);
            return View(zamowienia);
        }

        // GET: Zamowienia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zamowienia zamowienia = db.Zamowienia.Find(id);
            if (zamowienia == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa", zamowienia.Id_Kartoteki);
            return View(zamowienia);
        }

        // POST: Zamowienia/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Zamowienia,Data_zamowienia,Realizacja,Id_Kartoteki,Ilosc")] Zamowienia zamowienia)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(zamowienia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa", zamowienia.Id_Kartoteki);
            return View(zamowienia);
        }

        // GET: Zamowienia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zamowienia zamowienia = db.Zamowienia.Find(id);
            if (zamowienia == null)
            {
                return HttpNotFound();
            }
            return View(zamowienia);
        }

        // POST: Zamowienia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Zamowienia zamowienia = db.Zamowienia.Find(id);
            db.Zamowienia.Remove(zamowienia);
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
        private void UpdateQuantityAfterNewOrder(Zamowienia zamowienie)
        {
            var kartoteka = db.Kartoteki.FirstOrDefault(x => x.Id_Kartoteki == zamowienie.Id_Kartoteki);
            if (zamowienie.Realizacja) kartoteka.Stan += (int)zamowienie.Ilosc;

        }

        private int CalculateQuantityAfterUpdate(Zamowienia zamowienie, Zamowienia zamowieniePrzed)
        {

            
            if (zamowienie.Ilosc != zamowieniePrzed.Ilosc)
            {
                if (zamowienie.Realizacja)
                {
                    if (zamowieniePrzed.Realizacja)
                    {
                        return (int)(zamowienie.Ilosc - zamowieniePrzed.Ilosc);
                    }

                    return (int)zamowienie.Ilosc;
                }
                else
                {
                    if (zamowieniePrzed.Realizacja)
                    {
                        return (int)zamowieniePrzed.Ilosc*(-1);
                    }
                    
                }
            }
            return 0;
           
         }
        private void UpdateQuantityAfterUpdate(Zamowienia zamowienie, Zamowienia zamowieniePrzed)
        {
            var kartoteka = db.Kartoteki.FirstOrDefault(x => x.Id_Kartoteki == zamowienie.Id_Kartoteki);
            int quantityCalculated = CalculateQuantityAfterUpdate(zamowienie, zamowieniePrzed);

            kartoteka.Stan += quantityCalculated;
        }
    }

}
