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
using MongoDB.Driver;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace Proyecto_MongoDB.Controllers
{
    public class ListaCompradosController : Controller
    {
        MongoContext dbContext;
        public ListaCompradosController()
        {
            dbContext = new MongoContext();
        }

        // GET: USUARIOs
        public ActionResult Index()
        {
            dbContext = new MongoContext();
            var comprados = dbContext.database.GetCollection<ListaCompradosModel>("ListaCompradosModel").FindAll().ToList();

            return View(comprados);
            
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(ListaCompradosModel listacompra, String IDCarro, String NombreComprador)
        {
            try
            {
                //Crea el la coleccion en la base de datos y so esta creada crea solo la instancia
                var document = dbContext.database.GetCollection<BsonDocument>("ListaCompradosModel");

                //Se crea un query que filtre que no hayan repetidos basandose en el nombre
                var query = Query.And(Query.EQ("IDCarro", listacompra.IDCarro));

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

    }

}

