﻿using Proyecto_MongoDB.App_Start;
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

namespace Proyecto_MongoDB.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ListaController : Controller
    {
        MongoContext dbContext;
        public ListaController()
        {
            dbContext = new MongoContext();
        }
        // GET: USUARIOs
        public ActionResult Index()
        {
            dbContext = new MongoContext();
            var carros = dbContext.database.GetCollection<CarModel>("CarModel").FindAll().ToList();
            var model = new ListaCarrosViewModel()
            {
                ListaCarros = carros
            };

            return View(model);
            
        }

    }

}

