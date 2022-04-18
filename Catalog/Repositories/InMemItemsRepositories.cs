using System;
using System.Collections.Generic;
using Catalog.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repositories
{
    
    class InMemItemsRepositories : IInMemItemsRepositories
    {
        private readonly List<Item> items = new()
        {
           new Item{Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = System.DateTimeOffset.Now},
           new Item{Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedDate = System.DateTimeOffset.Now},
           new Item{Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreatedDate = System.DateTimeOffset.Now}
        };
        public async Task<IEnumerable<Item>>GetItemsAsync()
        {
            return await Task.FromResult(items);
        }
        public async Task<Item> GetItemAsync(Guid Id)
        {
            var item = items.Where(item => item.Id == Id).SingleOrDefault();
            return await Task.FromResult(item);
        }

        public async Task CreateItemAsync(Item item)
        {
           items.Add(item);
           await Task.CompletedTask;
        }

        public async Task UpdateItemAsync(Item item)
        {
           int index = items.FindIndex(existingItem => existingItem.Id == item.Id);
           items[index] = item;
           await Task.FromResult(item);
        }

        public async Task DeleteItemAsync(Guid guid)
        {
           int index = items.FindIndex(existingItem => existingItem.Id == guid);
           items.RemoveAt(index);
           await Task.CompletedTask;
        }
    }
}