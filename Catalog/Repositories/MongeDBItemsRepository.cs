using System;
using System.Collections.Generic;
using Catalog.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Catalog.Repositories{
    public class MongeDBItemsRepository : IInMemItemsRepositories
    {
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;

        private readonly FilterDefinitionBuilder<Item> filerBuilder = Builders<Item>.Filter;
        public MongeDBItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);
        }
        public async Task CreateItemAsync(Item item)
        {
            await itemsCollection.InsertOneAsync(item);
        }

        public  async Task DeleteItemAsync(Guid id)
        {
          var filter = filerBuilder.Eq(item => item.Id,id );
         await itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item> GetItemAsync(Guid Id)
        {
           var filter = filerBuilder.Eq(item => item.Id,Id );
           return await itemsCollection.Find(filter).SingleAsync(); 
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
             return await itemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
           var filter = filerBuilder.Eq(existingitem => existingitem.Id, item.Id);
           await itemsCollection.ReplaceOneAsync(filter, item);
        }
    }
}