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
    public class WydaniaController : Controller
    {
        private MagazynDBEntities db = new MagazynDBEntities();

        // GET: Wydania
        public ActionResult Index()
        {
            var wydania = db.Wydania.Include(w => w.Kartoteki).Include(w => w.MPK).Include(w => w.Osoby);
            return View(wydania.ToList());
        }

        // GET: Wydania/Details/5
        public ActionResult Details(int? id)
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

        // GET: Wydania/Create
        public ActionResult Create()
        {
            ViewBag.Id_Kartoteki = new SelectList(db.Kartoteki, "Id_Kartoteki", "Nazwa");
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
                db.SaveChanges();
                return RedirectToAction("Index");
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

        public JsonResult stanKartoteki(int Ilosc, int? Id_Kartoteki)
        {
            var kartoteka = db.Kartoteki.FirstOrDefault(x => x.Id_Kartoteki == Id_Kartoteki);
            if (kartoteka.Stan - Ilosc > 0) return Json(true, JsonRequestBehavior.AllowGet);
            else return Json(false,JsonRequestBehavior.DenyGet);
        }
    }
}
