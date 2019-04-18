using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Proyecto_MongoDB.App_Start
{

    public class MongoContext
    {

        MongoClient client;
        MongoServer server;
        public MongoDatabase database;

        //Se crea el constructor

        public MongoContext()
        {
            try
            {
                //Los valores se toman del archivo web.config
                var mongoDatabaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];
                var mongoUserName = ConfigurationManager.AppSettings["MongoUsername"];
                var mongoPassword = ConfigurationManager.AppSettings["MongoPassword"];
                var mongoPort = ConfigurationManager.AppSettings["MongoPort"];
                var mongoHost = ConfigurationManager.AppSettings["MongoHost"];

                string username = "admin";
                string password = "123";
                string mongoDbAuthMechanism = "SCRAM-SHA-256";
                MongoInternalIdentity internalIdentity = new MongoInternalIdentity("admin", username);

                PasswordEvidence passwordEvidence = new PasswordEvidence(password);


               

                //crear las credenciales
                MongoCredential credencial = MongoCredential.CreateCredential(mongoDatabaseName, mongoUserName, mongoPassword);


              //  MongoCredential mongoCredential = new MongoCredential(mongoDbAuthMechanism, internalIdentity, passwordEvidence);
              //  List<MongoCredential> credentials =  new List<MongoCredential>() { mongoCredential,credencial };



                //crear la configuracion del MongoClient para conectar
                var settings = new MongoClientSettings
                {
                    //Se agrega la credencial creada 
                    Credential = credencial,

                    //sele agrega los datos del servidor
                    Server = new MongoServerAddress(mongoHost, Convert.ToInt32(mongoPort))
                };


                client = new MongoClient(settings);
                server = client.GetServer();
                
                database = server.GetDatabase(mongoDatabaseName);

                /* var cmd = new CommandDocument("isMaster", "1");
                 var result = database.RunCommand(cmd);
                 var servers = result.Response.FirstOrDefault(
                 response => response.ToString().Contains("primary")).Value.ToString();
                 */
                //client.GetServer().Ping();
                
            }
            catch (Exception)
            {

                throw;
            }

           


        }

    }
}