using MongoDB.Bson;
using MongoDB.Driver;
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
        public ActionResult Details(String id)
        {
            if (id == null)
            {
                ViewBag.Message = "El carro no se encontro!";
                return RedirectToAction("Index");
            }

            //Se trae la coleccion 
            var documento = dbContext.database.GetCollection<CarModel>("CarModel");


            //Hace un Query que en caso que dentro del documento haya un objeto con el mismo id, significa que hay regsitros
            var carDetailscount = documento.FindAs<CarModel>(Query.EQ("_id", new ObjectId(id))).Count();

            if (carDetailscount > 0)
            {
                //Busca el Id del objeto que se le dio en Details
                var carObjectid = Query<CarModel>.EQ(p => p.Id, new ObjectId(id));

                //Se trae unicamente el objeto que tiene el id que se encontro
                var carDetail = dbContext.database.GetCollection<CarModel>("CarModel").FindOne(carObjectid);

                return View(carDetail);
            }


            //En caso de que no se cumple el if vuele al index
            return RedirectToAction("Index");

          
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

                //Se crea un query que filtre que no hayan repetidos basandose en el nombre y la pl
                var query = Query.And(Query.EQ("Marca", collection.Marca), Query.EQ("Placa", collection.Placa));

                //Cuenta los resultados del Query (la consulta)
                var count = document.FindAs<CarModel>(query).Count();


                if (count == 0)
                {
                    var result = document.Insert(collection);
                }
                else
                {
                    ViewBag.Message = "Nombre y Placa del carro ya existe";
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
        public ActionResult Edit(String id)
        {

            if (id == null)
            {
                ViewBag.Message = "El carro no se encontro!";
                return RedirectToAction("Index");
            }

            //Se trae la coleccion 
            var documento = dbContext.database.GetCollection<CarModel>("CarModel");


            //Hace un Query que en caso que dentro del documento haya un objeto con el mismo id, significa que hay regsitros
            var carDetailscount = documento.FindAs<CarModel>(Query.EQ("_id", new ObjectId(id))).Count();

            if (carDetailscount > 0)
            {
                //Busca el Id del objeto que se le dio en Details
                var carObjectid = Query<CarModel>.EQ(p => p.Id, new ObjectId(id));

                //Se trae unicamente el objeto que tiene el id que se encontro
                var carDetail = dbContext.database.GetCollection<CarModel>("CarModel").FindOne(carObjectid);

                return View(carDetail);
            }


            //En caso de que no se cumple el if vuele al index
            return RedirectToAction("Index");
        }

        // POST: InformacionCar/Edit/5
        [HttpPost]
        public ActionResult Edit(String id, CarModel carmodel)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.Message = "El carro no se encontro!";
                    return RedirectToAction("Index");
                }


                carmodel.Id = new ObjectId(id);

                //Query de Mongo para traer el id del objeto que se esta pasando por parametro 
                var CarObjectId = Query<CarModel>.EQ(p => p.Id, new ObjectId(id));

                // Se trae el documento de la coleccion 
                var colleccion = dbContext.database.GetCollection<CarModel>("CarModel");
                 
                //Se actualiza el documento que tenga el id y el objeto que se esta actualizando
                var resultado = colleccion.Update(CarObjectId, Update.Replace(carmodel), UpdateFlags.None);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: InformacionCar/Delete/5
        public ActionResult Delete(String id)
        {
            if (id == null)
            {
                ViewBag.Message = "El carro no se encontro!";
                return RedirectToAction("Index");
            }

            //Se trae la coleccion 
            var documento = dbContext.database.GetCollection<CarModel>("CarModel");


            //Hace un Query que en caso que dentro del documento haya un objeto con el mismo id, significa que hay regsitros
            var carDetailscount = documento.FindAs<CarModel>(Query.EQ("_id", new ObjectId(id))).Count();

            if (carDetailscount > 0)
            {
                //Busca el Id del objeto que se le dio en Details
                var carObjectid = Query<CarModel>.EQ(p => p.Id, new ObjectId(id));

                //Se trae unicamente el objeto que tiene el id que se encontro
                var carDetail = dbContext.database.GetCollection<CarModel>("CarModel").FindOne(carObjectid);

                return View(carDetail);
            }


            //En caso de que no se cumple el if vuele al index
            return RedirectToAction("Index");
        }

        // POST: InformacionCar/Delete/5
        [HttpPost]
        public ActionResult Delete(String id, CarModel carmodel)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.Message = "El carro no se encontro!";
                    return RedirectToAction("Index");
                }

                //Query para buscar el objeto con el id que se paso por parametro
                var carObjectid = Query<CarModel>.EQ(p => p.Id, new ObjectId(id));

                // Consigue la coleccion del objeto
                var colleccion = dbContext.database.GetCollection<CarModel>("CarModel");


                // Borra el documento que tenga el id que se busco anteriormente
                var resultado = colleccion.Remove(carObjectid, RemoveFlags.Single);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
