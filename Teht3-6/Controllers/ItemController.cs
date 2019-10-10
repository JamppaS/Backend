using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace gamewebapi
{
    [Route("api/players/{playerId}/items/")]
    [ApiController]
    public class ItemController : ControllerBase{

        private IRepository _itemrepository;

        public ItemController(IRepository repository) => _itemrepository = repository;

        [HttpPost]
        [Route("")]
        [ExceptionFilter]
        public Task<Item> CreateItem(Guid playerId, NewItem item){
            var newitem = new Item(){
                Id = Guid.NewGuid(),
                Level = item.Level,
                itemType = item.ItemType,
                Price = item.Price,
                CreationTime = DateTime.Now
            };
            if(this.TryValidateModel(newitem)){
                return _itemrepository.CreateItem(playerId, newitem);
            }
            throw new Exception("Item range invalid");
        }
        [HttpGet]
        [Route("{itemId}")]
        public Task<Item> GetItem(Guid playerId, Guid itemId){
            return _itemrepository.GetItem(playerId,itemId);
        }
        [HttpGet]
        [Route("")]
        public Task<Item[]> GetAllItems(Guid playerId){
            return _itemrepository.GetAllItems(playerId);
        }
        [HttpPost]
        [Route("{itemId}")]
        public Task<Item> UpdateItem(Guid playerId, Guid itemId, Item item){
            return _itemrepository.UpdateItem(playerId, itemId, item);
        }
        [HttpDelete]
        [Route("{itemId}")]
        public Task<Item> DeleteItem(Guid playerId, Guid item){
            return _itemrepository.DeleteItem(playerId, item);
        }


    }

}