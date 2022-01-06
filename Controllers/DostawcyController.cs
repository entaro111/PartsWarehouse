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
    public class DostawcyController : Controller
    {
        private MagazynDBEntities db = new MagazynDBEntities();

        // GET: Dostawcy
        public ActionResult Index()
        {
            return View(db.Dostawcy.ToList());
        }

        // GET: Dostawcy/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dostawcy dostawcy = db.Dostawcy.Find(id);
            if (dostawcy == null)
            {
                return HttpNotFound();
            }
            return View(dostawcy);
        }

        // GET: Dostawcy/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dostawcy/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Dostawcy,Nazwa,Miejscowosc,Kod")] Dostawcy dostawcy)
        {
            if (ModelState.IsValid)
            {
                db.Dostawcy.Add(dostawcy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dostawcy);
        }

        // GET: Dostawcy/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dostawcy dostawcy = db.Dostawcy.Find(id);
            if (dostawcy == null)
            {
                return HttpNotFound();
            }
            return View(dostawcy);
        }

        // POST: Dostawcy/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Dostawcy,Nazwa,Miejscowosc,Kod")] Dostawcy dostawcy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dostawcy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dostawcy);
        }

        // GET: Dostawcy/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dostawcy dostawcy = db.Dostawcy.Find(id);
            if (dostawcy == null)
            {
                return HttpNotFound();
            }
            return View(dostawcy);
        }

        // POST: Dostawcy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dostawcy dostawcy = db.Dostawcy.Find(id);
            db.Dostawcy.Remove(dostawcy);
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
