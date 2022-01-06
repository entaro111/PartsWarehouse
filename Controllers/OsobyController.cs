using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PartsWarehouse.Models;

namespace PartsWarehouse.Controllers
{
    public class OsobyController : Controller
    {
        private MagazynDBEntities db = new MagazynDBEntities();

        // GET: Osoby
        public ActionResult Index()
        {
            var osoby = db.Osoby.Include(o => o.Dzialy);
            return View(osoby.ToList());
        }

        // GET: Osoby/Details/5
        public ActionResult Details(int? id)
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
    }
}
