using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using Catalog.DTOs;
using Catalog.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Catalog.Controllers
{
    // Get /items
    [ApiController]
    [Route("items")]
    public class ItemsController: ControllerBase
    {
        private readonly IInMemItemsRepositories repository;
        private readonly ILogger<ItemsController> logger;
        public ItemsController(IInMemItemsRepositories repository, ILogger<ItemsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
        
        [HttpGet]
        public async Task<IEnumerable<ItemDTO>> GetItemsAsync()
        {
            var item = (await repository.GetItemsAsync())
                      .Select(item => item.AsDTO());
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:yy")}: Retrieved {item.Count()}");
            return item;
        }
        // GET /items/{id}
       [HttpGet("{id}")]
       public async Task<ActionResult<ItemDTO>> GetItemAsync(Guid id) // allows to return more than one type
       {
           var item = await repository.GetItemAsync(id);
           if(item == null)
           {
               return NotFound();
           }
           return Ok(item.AsDTO());
       }
        [HttpPost]
        public async Task<ActionResult<ItemDTO>> CreateItemAsync(CreateItemDTO itemDTO)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDTO.Name,
                Price = itemDTO.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await repository.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new {id = item.Id}, item.AsDTO());
        }
        // Put /items/
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDTO itemDTO)
        {
            var existingItem = await repository.GetItemAsync(id);
            if(existingItem is null) return NotFound();
            Item updatedItem = existingItem with{
                Name = itemDTO.Name,
                Price = itemDTO.Price
            };
            await repository.UpdateItemAsync(updatedItem);
            return NoContent();
        }
        // DELETE /items/.
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = repository.GetItemAsync(id);
            if(existingItem is null)
            {
                return NotFound();
            }
            await repository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}