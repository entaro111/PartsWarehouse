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
    public class KartotekiController : Controller
    {
        private MagazynDBEntities db = new MagazynDBEntities();

        // GET: Kartoteki
        public ActionResult Index()
        {
            var kartoteki = db.Kartoteki.Include(k => k.Dostawcy).Include(k => k.JM);
            return View(kartoteki.ToList());
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
        public ActionResult Create([Bind(Include = "Id_Kartoteki,Nazwa,Stan,Miejsce,Id_JM,Id_Dostawcy,Kod")] Kartoteki kartoteki)
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
        public ActionResult Edit([Bind(Include = "Id_Kartoteki,Nazwa,Stan,Miejsce,Id_JM,Id_Dostawcy,Kod")] Kartoteki kartoteki)
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

        public JsonResult nameExist([Bind(Prefix = "Nazwa")] string Name)
        {
            return Json(!db.Kartoteki.Any(x => x.Nazwa.ToLower() == Name.ToLower()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult codeExist([Bind(Prefix = "Kod")] string Code)
        {
            return Json(!db.Kartoteki.Any(x => x.Kod.ToLower() == Code.ToLower()), JsonRequestBehavior.AllowGet);
        }
    }
}
