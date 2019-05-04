using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using Proyecto_MongoDB.App_Start;
using Proyecto_MongoDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_MongoDB.Controllers
{

    [Authorize]
    public class InformacionCarController : Controller
    {
        MongoContext dbContext;
        
        public InformacionCarController()
        {
            dbContext = new MongoContext();
        }

        /*

                [HttpPost]
                public ActionResult Upload()
                {
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                            file.SaveAs(path);
                        }
                    }

                    return RedirectToAction("UploadDocument");
                }

                [HttpPost]
                public HttpPostedFileBase Upload()
                {
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                            file.SaveAs(path);
                        }
                    }

                    return;
                }
        */


        /*
                CarModel carViewModel = new CarModel()
                {
                     Id = carDomain.Id
            ,
                    DailyRentalFee = carDomain.DailyRentalFee
            ,
                    Make = carDomain.Make
            ,
                    NumberOfDoors = carDomain.NumberOfDoors
            ,
                    ImageId = carDomain.ImageId
                };*/



        private void EliminarImagenCarro(CarModel car)
        {
            dbContext.database.GridFS.DeleteById(car.ImageId);
            car.ImageId = string.Empty;
            //var document = dbContext.database.GetCollection<BsonDocument>("CarModel");
           // var result = document.Insert(car);
        }


        private void AgregarImagenCarro(HttpPostedFileBase file, CarModel car)
        {
            ObjectId imageId = ObjectId.GenerateNewId();
            car.ImageId = imageId.ToString();


            var document = dbContext.database.GetCollection<BsonDocument>("CarModel");
            var result = document.Insert(car);

            MongoGridFSCreateOptions createOptions = new MongoGridFSCreateOptions()
            {
                Id = imageId
                ,
                ContentType = file.ContentType
            };
            dbContext.database.GridFS.Upload(file.InputStream, file.FileName, createOptions);
        }





        /*
                public void prueba()
                {//Forma default
                    MongoGridFS gridFsDefault = dbContext.database.GridFS;

                    //Forma custom para sobreecribir el gridfs deafult
                    MongoGridFSSettings gridFSSettings = new MongoGridFSSettings();
                    gridFSSettings.ChunkSize = 1024;

                    MongoGridFS gridFsCustom = dbContext.database.GetGridFS(gridFSSettings);



                    MongoCollection<BsonDocument> chunks = gridFsDefault.Chunks;




                }
                */



#region Index

        // GET: InformacionCar
        public ActionResult Index()
        {

            var carDetails = dbContext.database.GetCollection<CarModel>("CarModel").FindAll().ToList();
            foreach (var item in carDetails)
            {
                ViewBag.ImageId += item.ImageId;
            }
            
            return View(carDetails);
        }

        #endregion


        #region Detalles
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
        #endregion
        // GET: InformacionCar/Create
        public ActionResult Create()
        {
            return View();
        }


        #region Create
        // POST: InformacionCar/Create
        [HttpPost]
        public ActionResult Create(CarModel collection, HttpPostedFileBase file)
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
                    //var result = document.Insert(collection);
                    AgregarImagenCarro(file, collection);
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
        #endregion




        #region EDitar
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


        #endregion



        private void ActualizarImagenCarro(HttpPostedFileBase file, CarModel car, String id)
        {
            ObjectId imageId = ObjectId.GenerateNewId();
            car.ImageId = imageId.ToString();

            // Se trae el documento de la coleccion 
            var colleccion = dbContext.database.GetCollection<CarModel>("CarModel");

            var CarObjectId = Query<CarModel>.EQ(p => p.Id, new ObjectId(id));
            //Se actualiza el documento que tenga el id y el objeto que se esta actualizando
            var resultado = colleccion.Update(CarObjectId, Update.Replace(car), UpdateFlags.None);


            MongoGridFSCreateOptions createOptions = new MongoGridFSCreateOptions()
            {
                Id = imageId
                ,
                ContentType = file.ContentType
            };
            dbContext.database.GridFS.Upload(file.InputStream, file.FileName, createOptions);
        }

        // POST: InformacionCar/Edit/5
        [HttpPost]
        public ActionResult Edit(String id, CarModel car, HttpPostedFileBase file)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.Message = "El carro no se encontro!";
                    return RedirectToAction("Index");
                }



                /*
                    Car car = CarRentalContext.Cars.FindOneById(new ObjectId(id));
                    if (!string.IsNullOrEmpty(car.ImageId))
                    {
                        DeleteCarImage(car);
                    }
                    AttachImageToCar(file, car);
                 
                 * */

                car.Id = new ObjectId(id);

                //Query de Mongo para traer el id del objeto que se esta pasando por parametro 
                var CarObjectId = Query<CarModel>.EQ(p => p.Id, new ObjectId(id));

                var carDetail = dbContext.database.GetCollection<CarModel>("CarModel").FindOne(CarObjectId);

                

                //String carId = CarObjectId.ToJson();

                if (!string.IsNullOrEmpty(carDetail.ImageId))
                {
                    EliminarImagenCarro(carDetail);
                }
                ActualizarImagenCarro(file, car, id);


              

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
