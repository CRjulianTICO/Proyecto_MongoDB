using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using Proyecto_MongoDB.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_MongoDB.Controllers
{
    public class ImagenesController : Controller
    {


        MongoContext dbContext;


        public ImagenesController()
        {
            dbContext = new MongoContext();
        }


        public ActionResult Imagen(string imageId)
        {
            MongoGridFSFileInfo imageFileInfo = dbContext.database.GridFS.FindOneById(new ObjectId(imageId));
            return File(imageFileInfo.OpenRead(), imageFileInfo.ContentType);
        }


    
    // GET: Imagenes
    public ActionResult Index()
        {
            return View();
        }

        // GET: Imagenes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Imagenes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Imagenes/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Imagenes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Imagenes/Edit/5
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

        // GET: Imagenes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Imagenes/Delete/5
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
