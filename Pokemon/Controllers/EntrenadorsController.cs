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
    public class EntrenadorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Entrenadors
        public ActionResult Index()
        {
            var entrenadores = db.Entrenadores.Include(e => e.Enemy).Include(e => e.Pokemon);
            return View(entrenadores.ToList());
        }

        // GET: Entrenadors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entrenador entrenador = db.Entrenadores.Find(id);
            if (entrenador == null)
            {
                return HttpNotFound();
            }
            return View(entrenador);
        }

        // GET: Entrenadors/Create
        public ActionResult Create()
        {
            ViewBag.Id_Enemy = new SelectList(db.Pokemons, "Id", "Nombre");
            ViewBag.Id_Pokemon = new SelectList(db.Pokemons, "Id", "Nombre");
            return View();
        }

        // POST: Entrenadors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Pokemon,Id_Enemy")] Entrenador entrenador)
        {
            if (ModelState.IsValid)
            {
                db.Entrenadores.Add(entrenador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Enemy = new SelectList(db.Pokemons, "Id", "Nombre", entrenador.Id_Enemy);
            ViewBag.Id_Pokemon = new SelectList(db.Pokemons, "Id", "Nombre", entrenador.Id_Pokemon);
            return View(entrenador);
        }

        // GET: Entrenadors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entrenador entrenador = db.Entrenadores.Find(id);
            if (entrenador == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Enemy = new SelectList(db.Pokemons, "Id", "Nombre", entrenador.Id_Enemy);
            ViewBag.Id_Pokemon = new SelectList(db.Pokemons, "Id", "Nombre", entrenador.Id_Pokemon);
            return View(entrenador);
        }

        // POST: Entrenadors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Pokemon,Id_Enemy")] Entrenador entrenador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entrenador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Enemy = new SelectList(db.Pokemons, "Id", "Nombre", entrenador.Id_Enemy);
            ViewBag.Id_Pokemon = new SelectList(db.Pokemons, "Id", "Nombre", entrenador.Id_Pokemon);
            return View(entrenador);
        }

        // GET: Entrenadors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entrenador entrenador = db.Entrenadores.Find(id);
            if (entrenador == null)
            {
                return HttpNotFound();
            }
            return View(entrenador);
        }

        // POST: Entrenadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entrenador entrenador = db.Entrenadores.Find(id);
            db.Entrenadores.Remove(entrenador);
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

        public ActionResult ShowPoke()
        {
            var pokes = db.Pokemons.ToList();
            return PartialView("_Show", pokes);
        }
        public ActionResult Añadir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Random r = new Random();
            int n = r.Next(1, db.Pokemons.Count());

            Pokemones poke = db.Pokemons.Find(id);
            Pokemones enemy = db.Pokemons.Find(n);

            Entrenador campo = new Entrenador()
            {
                Id_Pokemon = poke.Id,
                Pokemon = poke,
                Id_Enemy = enemy.Id,
                Enemy = enemy,
            };

            db.Entrenadores.Add(campo);
            db.SaveChanges();
            return PartialView("_Combate", campo);
        }
        public ActionResult Atacar(int? id)
        {
            Entrenador Entre = db.Entrenadores.Find(id);
            Pokemones pokemon = db.Pokemons.Find(Entre.Id_Pokemon);
            Pokemones Enemigo = db.Pokemons.Find(Entre.Id_Enemy);
            if ((Enemigo.Ataque) < (pokemon.Defensa))
            {
                pokemon.Vida = pokemon.Vida - 10;
                Enemigo.Vida = Enemigo.Vida - (pokemon.Ataque - Enemigo.Defensa);
            }
            else if ((pokemon.Ataque) < (Enemigo.Defensa))
            {
                pokemon.Vida = pokemon.Vida - (Enemigo.Ataque - pokemon.Defensa);
                Enemigo.Vida = Enemigo.Vida - 10;
            }
            else
            {
                pokemon.Vida = pokemon.Vida - (Enemigo.Ataque - pokemon.Defensa);
                Enemigo.Vida = Enemigo.Vida - (pokemon.Ataque - Enemigo.Defensa);
            }

            if ((Enemigo.Vida < 0) || (pokemon.Vida < 0) || (Enemigo.Vida < 0) & (pokemon.Vida <0))
            {
                if ((Enemigo.Vida <= 0) & (pokemon.Vida <= 0))
                {
                    Enemigo.Vida = 100;
                    pokemon.Vida = 100;
                    ViewBag.Ganador = "Empate";
                    db.SaveChanges();
                }
                else if ((Enemigo.Vida) < 0)
                {
                    Enemigo.Vida = 100;
                    pokemon.Vida = 100;
                    ViewBag.Ganador = pokemon.Nombre ;
                    db.SaveChanges();
                }
                else if ((pokemon.Vida) < 0)
                {
                    Enemigo.Vida = 100;
                    pokemon.Vida = 100;
                    ViewBag.Ganador =  Enemigo.Nombre;
                    db.SaveChanges();
                }

            }
            db.SaveChanges();
            return PartialView("_Combate", Entre);
        }


    }
}
