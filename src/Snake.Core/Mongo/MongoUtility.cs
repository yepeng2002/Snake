using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Snake.Core.Mongo
{
    internal static class MongoUtility
    {
        private static readonly string AuthorizedDB = "admin";
        public static readonly string MongConnectionString = ConfigurationManager.AppSettings.Get("MongoConnectionString");

        private static IMongoDatabase GetDatabase(string connectString)
        {
            var mongoUrl = new MongoUrl(connectString);
            var logSetting = MongoClientSettings.FromUrl(mongoUrl);
            if (mongoUrl.HasAuthenticationSettings)
            {
                MongoCredential mongoCredential = MongoCredential.CreateCredential(AuthorizedDB, mongoUrl.Username, mongoUrl.Password);
                List<MongoCredential> mongoCredentialList = new List<MongoCredential>();
                mongoCredentialList.Add(mongoCredential);
                logSetting.Credentials = mongoCredentialList;
            }
            var client = new MongoClient(logSetting);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }

        public static string GetDefaultConnectionString()
        {
            return MongConnectionString;
        }

        public static IMongoCollection<T> GetCollectionFromConnectionString<T>(string connectionstring)
            where T : IEntity
        {
            return GetDatabase(GetDefaultConnectionString()).GetCollection<T>(GetCollectionName<T>());
        }

        private static string GetCollectionName<T>() where T : IEntity
        {
            string collectionName;
            if (typeof(T).BaseType.Equals(typeof(object)))
            {
                collectionName = GetCollectioNameFromInterface<T>();
            }
            else
            {
                collectionName = GetCollectionNameFromType(typeof(T));
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("这个实体的集合名称不能为空");
            }
            return collectionName;
        }

        private static string GetCollectioNameFromInterface<T>()
        {
            string collectionname;
            Attribute att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute));
            if (att != null)
            {
                collectionname = ((CollectionNameAttribute)att).Name;
            }
            else
            {
                collectionname = typeof(T).Name;
            }

            return collectionname;
        }

        private static string GetCollectionNameFromType(Type entitytype)
        {
            string collectionname;


            Attribute att = Attribute.GetCustomAttribute(entitytype, typeof(CollectionNameAttribute));
            if (att != null)
            {
                collectionname = ((CollectionNameAttribute)att).Name;
            }
            else
            {

                while (!entitytype.BaseType.Equals(typeof(Entity)))
                {
                    entitytype = entitytype.BaseType;
                }

                collectionname = entitytype.Name;
            }

            return collectionname;
        }
    }
}
