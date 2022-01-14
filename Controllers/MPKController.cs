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
    public class MPKController : Controller
    {
        private MagazynDBEntities db = new MagazynDBEntities();

        // GET: MPK
        public ActionResult Index()
        {
            return View(db.MPK.ToList());
        }

        // GET: MPK/Details/5
        public ActionResult Details(int? id)
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
