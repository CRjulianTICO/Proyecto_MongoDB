using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Proyecto_MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using Proyecto_MongoDB.App_Start;


namespace Proyecto_MongoDB.Controllers
{
    public class ListaCompradosController : Controller
    {


        MongoContext dbContext;
        public ListaCompradosController()
        {
            dbContext = new MongoContext();
        }

        // GET: ListaComprados
        public ActionResult Index()
        {
            var comprados = dbContext.database.GetCollection<ListaCompradosModel>("ListaCompradosModel").FindAll().ToList();

            //ViewBag.DeafaultNombreComprador = listacompra.NombreComprador;

            //foreach (var item in comprados)
            //{
            //    ViewBag.DefaultNombreComprador = item.NombreComprador;
            //    ViewBag.DefaultIDCarro = item.IDCarro;
            //}

            return View(comprados);
        }

        // GET: ListaComprados/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ListaComprados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ListaComprados/Create
        [HttpPost]
        public ActionResult Create(ListaCompradosModel listacompra, String IDCarro, String NombreComprador)
        {
            try
            {
                //ViewBag.DeafaultNombreComprador = listacompra.NombreComprador;


                //Crea el la coleccion en la base de datos y so esta creada crea solo la instancia
                var document = dbContext.database.GetCollection<BsonDocument>("ListaCompradosModel");

                //Se crea un query que filtre que no hayan repetidos basandose en el nombre y el id del carro
                var query = Query.And(Query.EQ("NombreComprador", listacompra.NombreComprador), Query.EQ("IDCarro", listacompra.IDCarro));

                //Cuenta los resultados del Query (la consulta)
                var count = document.FindAs<ListaCarrosViewModel>(query).Count();


                if (count == 0)
                {
                    var result = document.Insert(listacompra);
                }
                else
                {
                    ViewBag.Message = "Carro ya esta comprado";
                    return View("Create", listacompra);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ListaComprados/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ListaComprados/Edit/5
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

        // GET: ListaComprados/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ListaComprados/Delete/5
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














        public String nombreCarro(String id)
        {
            String nombre;
            if (id == null)
            {
                ViewBag.Message = "El carro no se encontro!";

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



                return "";
            }


            return "No se encontro";





        }








    }
}
