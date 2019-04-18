using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Proyecto_MongoDB.Models
{
    public class CarModel
    {
        
        
        [Key]
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Nombre")]
        public String Nombre { get; set; }



        [BsonElement("Color")]
        public String Color { get; set; }



        [BsonElement("Precio")]
        public Int32 Precio { get; set; }



        [BsonElement("Cilindraje")]
        public Int32 Cilindraje { get; set; }


        [BsonElement("NoRegistracion")]
        public String NoRegistracion { get; set; }

        

        
        [BsonElement("DiaRegistracion")]
        public String DiaRegistracion { get; set; }



        [BsonElement("Modelo")]
        public String Modelo { get; set; }



        [BsonElement("Placa")]
        public String Placa { get; set; }


    }
}