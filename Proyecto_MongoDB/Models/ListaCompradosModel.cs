using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Proyecto_MongoDB.Models
{

    public class ListaCompradosModel
        {
   

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("NombreComprador")]
        public String NombreComprador { get; set; }

        [BsonElement("IDCarro")]
        public String IDCarro { get; set; }
    }
}

