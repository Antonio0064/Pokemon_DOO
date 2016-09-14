using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pokemon.Models;

namespace Pokemon.Controllers
{
    public class PokemonesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pokemones
        public ActionResult Index()
        {
            return View(db.Pokemons.ToList());
        }

        // GET: Pokemones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pokemones pokemones = db.Pokemons.Find(id);
            if (pokemones == null)
            {
                return HttpNotFound();
            }
            return View(pokemones);
        }

        // GET: Pokemones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pokemones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Url,Vida,Ataque,Defensa")] Pokemones pokemones)
        {
            if (ModelState.IsValid)
            {
                db.Pokemons.Add(pokemones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pokemones);
        }

        // GET: Pokemones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pokemones pokemones = db.Pokemons.Find(id);
            if (pokemones == null)
            {
                return HttpNotFound();
            }
            return View(pokemones);
        }

        // POST: Pokemones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Url,Vida,Ataque,Defensa")] Pokemones pokemones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pokemones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pokemones);
        }

        // GET: Pokemones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pokemones pokemones = db.Pokemons.Find(id);
            if (pokemones == null)
            {
                return HttpNotFound();
            }
            return View(pokemones);
        }

        // POST: Pokemones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pokemones pokemones = db.Pokemons.Find(id);
            db.Pokemons.Remove(pokemones);
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
