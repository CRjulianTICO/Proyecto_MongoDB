using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Proyecto_MongoDB.App_Start;
using Proyecto_MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_MongoDB.Controllers
{
    public class InformacionCarController : Controller
    {
        MongoContext dbContext;
        public InformacionCarController()
        {
            dbContext = new MongoContext();
        }

        // GET: InformacionCar
        public ActionResult Index()
        {
            var carDetails = dbContext.database.GetCollection<CarModel>("CarModel").FindAll().ToList();
            return View(carDetails);
        }

        // GET: InformacionCar/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InformacionCar/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InformacionCar/Create
        [HttpPost]
        public ActionResult Create(CarModel collection)
        {
            try
            {
                //Crea el la coleccion en la base de datos y so esta creada crea solo la instancia
                var document = dbContext.database.GetCollection<BsonDocument>("CarModel");

                //Se crea un query que filtre que no hayan repetidos basandose en el nombre y el color
                var query = Query.And(Query.EQ("Nombre", collection.Nombre), Query.EQ("Placa", collection.Placa));

                //Cuenta los resultados del Query
                var count = document.FindAs<CarModel>(query).Count();


                if (count == 0)
                {
                    var result = document.Insert(collection);
                }
                else
                {
                    TempData["Message"] = "Nombre y Placa del carro ya existe";
                    return View("Create", collection);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: InformacionCar/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InformacionCar/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: InformacionCar/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InformacionCar/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
