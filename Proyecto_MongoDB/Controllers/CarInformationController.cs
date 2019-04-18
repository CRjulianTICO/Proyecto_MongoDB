using Proyecto_MongoDB.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Proyecto_MongoDB.Models;

namespace Proyecto_MongoDB.Controllers
{
    public class CarInformationController : Controller
    {


        MongoContext dbContext;
        public CarInformationController()
        {
            dbContext = new MongoContext();
        }

        
        [System.Web.Http.HttpPost]
        public ActionResult Create(CarModel carmodel) {
            try
            {
                //Crea el la coleccion en la base de datos y so esta creada crea solo la instancia
                var document = dbContext.database.GetCollection<BsonDocument>("CarModel");

                //Se crea un query que filtre que no hayan repetidos basandose en el nombre y el color
                var query = Query.And(Query.EQ("Nombre", carmodel.Nombre), Query.EQ("Placa", carmodel.Placa));

                //Cuenta los resultados del Query
                var count = document.FindAs<CarModel>(query).Count();


                if (count == 0)
                {
                    var result = document.Insert(carmodel);
                }
                else
                {
                    TempData["Message"] = "Nombre y Placa del carro ya existe";
                    return View("Create", carmodel);
                }


                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }



        public ActionResult Index()
        {
            var carDetails = dbContext.database.GetCollection<CarModel>("CarModel").FindAll().ToList();
            return View(carDetails);
        }


        /*
                // GET: api/CarInformation
                public IEnumerable<string> Get()
                {
                    return new string[] { "value1", "value2" };
                }

                // GET: api/CarInformation/5
                public string Get(int id)
                {
                    return "value";
                }

                // POST: api/CarInformation
                public void Post([FromBody]string value)
                {
                }

                // PUT: api/CarInformation/5
                public void Put(int id, [FromBody]string value)
                {
                }

                // DELETE: api/CarInformation/5
                public void Delete(int id)
                {
                }

                */

    }
}
