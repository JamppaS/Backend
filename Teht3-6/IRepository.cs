using System.Threading.Tasks;
using System;
namespace gamewebapi{

    public interface IRepository{
        Task<Player> Get(Guid id);
        Task<Player[]> GetAll();
        Task<Player> Create(Player player);
        Task<Player> Modify(Guid id, Player player);
        Task<Player> Delete(Guid id);

        
        Task<Item> CreateItem(Guid playerId, Item item);
        Task<Item> GetItem(Guid playerId, Guid itemId);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> UpdateItem(Guid playerId,Guid itemId, Item item);
        Task<Item> DeleteItem(Guid playerId, Guid itemId);

        Task<Player[]> GetPlayersWithScore(int score);
        Task<Player> GetPlayerWithName(string name);
        Task<Player[]> GetPlayersWithItemType(ItemType itemType);
        Task<Player> IncrementPlayerScore(Guid id, int incement);
    
    }
}