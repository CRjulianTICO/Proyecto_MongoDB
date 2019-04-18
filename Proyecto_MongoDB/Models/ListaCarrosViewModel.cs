using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Proyecto_MongoDB.Models
{

    public class ListaCarrosViewModel
        {
            public ListaCarrosViewModel()
            {
            }
            public List<CarModel> ListaCarros { get; set; }

    }
}

