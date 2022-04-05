using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace OlamideChangeStream
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var projectionStoreConnectionString = "";
            var mongoClient = new MongoClient(projectionStoreConnectionString);
            var database = mongoClient.GetDatabase("mongorepltest");
            var collection = database.GetCollection<BsonDocument>("test");
            var collections = await database.ListCollectionsAsync();
            await collections.ForEachAsync(collectionn =>
            {
                foreach (var val in collectionn.Names)
                {
                    Console.WriteLine(collectionn.GetValue(val));
                }
            });
            string filter = $"{{$and:[ {{\"ns.db\":\"mongorepltest\" }}, {{\"ns.coll\":\"test\" }}]}}";
            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>().Match(filter).Project("{_id:0}");
            var options = new ChangeStreamOptions { FullDocument = ChangeStreamFullDocumentOption.UpdateLookup, ResumeAfter=new BsonDocument() };
            // watch is syntatic sugar for the below since it creates an agrregation pipeline under the hood and add the stages
            //var xw =  collection.Aggregate().ChangeStream().Project("");
            using (var x = await collection.WatchAsync())
            {
                await x.ForEachAsync(change =>
                {
                    string json = ConvertChangeStreamBsonDocumentToJson(change);
                    Console.WriteLine(json);
                });
            }
        }
        private static string ConvertChangeStreamBsonDocumentToJson(ChangeStreamDocument<BsonDocument> changeStreamDocument)
        {
            string json;
            var subject = new ChangeStreamDocumentSerializer<BsonDocument>(BsonDocumentSerializer.Instance);
            using (var textWriter = new StringWriter())
            using (var writer = new MongoDB.Bson.IO.JsonWriter(textWriter))
            {
                {
                    var context = BsonSerializationContext.CreateRoot(writer);
                    subject.Serialize(context, changeStreamDocument);
                    json = textWriter.ToString();
                }
            }
            return json;
        }
        private ChangeStreamDocument<BsonDocument> ConvertJsonToChangeStreamBsonDocument(string json)
        {
            ChangeStreamDocument<BsonDocument> changeStreamDocument;
            var subject = new ChangeStreamDocumentSerializer<BsonDocument>(BsonDocumentSerializer.Instance);

            using (var reader = new MongoDB.Bson.IO.JsonReader(json))
            {
                var context = BsonDeserializationContext.CreateRoot(reader);
                changeStreamDocument = subject.Deserialize(context);
            }
            return changeStreamDocument;
        }
        public class ConfigurationAppsettingKeyNameConstants
        {
            public const string ProjectionStoreBaseUrl = "ProjectionStore:BaseUrl";
            public const string ProjectionStoreUserName = "ProjectionStore:UserName";
            public const string ProjectionStoreUserPassword = "ProjectionStore:UserPassword";
            public const string ProjectionStoreClusterName = "ProjectionStore:ClusterName";
            public const string ProjectionStoreDatabaseName = "ProjectionStore:DatabaseName";
            public const string ProjectionStoreWriteRetries = "ProjectionStore:WriteRetries";
            public const string ProjectionStoreWriteConcern = "ProjectionStore:WriteConcern";
            public const string ProjectionStoreCollectionName = "ProjectionStore:CollectionName";
        }
    }
}
