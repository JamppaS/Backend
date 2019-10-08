// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using gameapi.Mongodb;
// using MongoDB.Driver;
// using gameapi.Models;

// namespace gameapi.Repositories
// {
//     public class MongoDbRepository : IPlayersRepository
//     {
//         private IMongoCollection<Player> _collection;

//         public MongoDbRepository(MongoDBClient client)
//         {
//             IMongoDatabase database = client.GetDatabase("game");
//             _collection = database.GetCollection<Player>("players");
//         }

//         public async Task<Player> Create(Player player)
//         {
//             player.Id = Guid.NewGuid();

//             await _collection.InsertOneAsync(player);
//             return player;
//         }

//         public async Task<Item> CreateNewItem(Guid id, Item newItem)
//         {
//             newItem.Id = Guid.NewGuid();

//             var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
//             var playerCursor = await _collection.FindAsync(filter);
//             Player player = await playerCursor.FirstAsync();

//             player.items.Add(newItem);

//             return player.items.Last();
//         }

//         public async Task<Player> Delete(Guid id)
//         {
//             var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
//             var playerCursor = await _collection.FindAsync(filter);
//             Player player = await playerCursor.FirstAsync();

//             await _collection.DeleteOneAsync(filter);

//             return player;
//         }

//         public async Task<Item> DeleteItem(Guid id, Guid itemId)
//         {
//             var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
//             var playerCursor = await _collection.FindAsync(filter);
//             Player player = await playerCursor.FirstAsync();

//             for (int i = 0; i < player.items.Count; i++)
//             {
//                 if (player.items[i].Id == itemId)
//                 {
//                     Item deletedItem = player.items[i];
//                     player.items.RemoveAt(i);
//                     return deletedItem;
//                 }
//             }

//             return null;
//         }

//         public async Task<Player> Get(Guid id)
//         {
//             var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
//             var playerCursor = await _collection.FindAsync(filter);
//             Player player = await playerCursor.FirstAsync();
//             return player;
//         }

//         public async Task<Item> GetItem(Guid id, Guid itemId)
//         {
//             var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
//             var playerCursor = await _collection.FindAsync(filter);
//             Player player = await playerCursor.FirstAsync();

//             for (int i = 0; i < player.items.Count; i++)
//             {
//                 if (player.items[i].Id == itemId)
//                 {
//                     return player.items[i];
//                 }
//             }

//             return null;
//         }

//         public async Task<Player[]> GetAll()
//         {
//             var list = _collection.Find(p => true).ToList();
//             Player[] myArray = list.ToArray();
//             return myArray;
//         }

//         public async Task<List<Item>> GetAllItems(Guid id)
//         {
//             var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
//             var playerCursor = await _collection.FindAsync(filter);
//             Player player = await playerCursor.FirstAsync();

//             return player.items;
//         }

//         public async Task<Player> Modify(Guid id, Player player)
//         {
//             var filter = Builders<Player>.Filter.Eq(p => p.Id, player.Id);
//             await _collection.ReplaceOneAsync(filter,player);
//             return player;
//         }

//         public async Task<Item> ModifyItem(Guid id, Guid itemId, Item modifiedItem)
//         {

//             var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
//             var playerCursor = await _collection.FindAsync(filter);
//             Player player = await playerCursor.FirstAsync();

//             for (int i = 0; i < player.items.Count; i++)
//             {
//                 if (player.items[i].Id == itemId)
//                 {
//                     Item oldItem = player.items[i];
//                     player.items[i] = modifiedItem;
//                     return oldItem;
//                 }
//             }

//             return null;
//         }
//     }
// }