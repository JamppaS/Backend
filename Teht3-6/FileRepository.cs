using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace gamewebapi{
    public class FileRepository: IRepository{
        
        
        public Task<Player> Get(Guid id){
            
            string[] _repository = File.ReadAllLines("game_dev.txt", Encoding.Default);
            foreach(var player in _repository){
                var p =JsonConvert.DeserializeObject<Player>(player);
                if(p.Id == id){
                    return Task.FromResult(p);
                }
            }
            throw new KeyNotFoundException();
        }
        public Task<Player[]> GetAll(){
            string[] _repository = File.ReadAllLines("game_dev.txt", Encoding.Default);
            List<Player> plist = new List<Player>();
            foreach(var player in _repository){
                plist.Add(JsonConvert.DeserializeObject<Player>(player));
            }
            
            return Task.FromResult(plist.ToArray());
        }
        public Task<Player> Create(Player player){
            string[] _repository = File.ReadAllLines("game_dev.txt", Encoding.Default);
           // PlayerList players = JsonConvert.DeserializeObject<PlayerList>(_repository);
            var newplayer = new Player()
            {
                Id = Guid.NewGuid(),
                Name = player.Name
            };
            List<string> newlist = _repository.ToList();
            newlist.Add(JsonConvert.SerializeObject(newplayer));
           // players.allPlayers.Add(newplayer); // heittää nullreferencen atm
            //File.AppendAllText("game_dev.txt", JsonConvert.SerializeObject(newplayer));
            File.WriteAllLines("game_dev.txt", newlist.ToArray());
            return Task.FromResult(newplayer);
        }
        public Task<Player> Modify(Guid id, Player player){

            string[] _repository = File.ReadAllLines("game_dev.txt", Encoding.Default);
            for(int i = 0; i<_repository.Length; i++){
                var p =JsonConvert.DeserializeObject<Player>(_repository[i]);
                if(p.Id == id){
                    p.Score = player.Score;
                    _repository[i] = JsonConvert.SerializeObject(p);
                    File.WriteAllLines("game_dev.txt", _repository);
                    return Task.FromResult(p);
                }
            }
            throw new NotFoundException();
        }
        public Task<Player> Delete(Guid id){
            Player deletedPlayer = new Player();
            List<Player> newlist = new List<Player>();
            var _repository = File.ReadAllLines("game_dev.txt", Encoding.Default);
            for(int i = 0; i<_repository.Length; i++){
                var p = JsonConvert.DeserializeObject<Player>(_repository[i]);
                if(p.Id != id)
                    newlist.Add(p);
                else{
                    deletedPlayer = p;
                }
            }
            string[] newarray = new string[newlist.Count];
            for(int i=0; i<newlist.Count; i++){
                newarray[i] = JsonConvert.SerializeObject(newlist[i]);
            }
            File.WriteAllLines("game_dev.txt", newarray);
            return Task.FromResult(deletedPlayer);
            
        }

        public Task<Item> CreateItem(Guid playerId, Item item)
        {
            Task<Player> player = Get(playerId);
            if(player.Result.Level < 3 && item.itemType.Equals(ItemType.Sword))
            {
                throw new RequirementException();
            }
            else
            {
                string[] _repository = File.ReadAllLines("game_dev.txt", Encoding.Default);
                for(int i = 0; i<_repository.Length; i++){
                    var p =JsonConvert.DeserializeObject<Player>(_repository[i]);
                    if(p.Id == playerId){
                        p.Items.Add(item);
                        
                    }
                    _repository[i] = JsonConvert.SerializeObject(p);
                    
                }
                File.WriteAllLines("game_dev.txt", _repository);
                return Task.FromResult(item);
            }



        }

        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            string[] _repository = File.ReadAllLines("game_dev.txt", Encoding.Default);
            foreach(var player in _repository){
                var p =JsonConvert.DeserializeObject<Player>(player);
                if(p.Id == playerId){
                    foreach(var item in p.Items){
                        if(item.Id == itemId){
                            return Task.FromResult(item);
                        }
                    }     
                }
            }
            throw new NotFoundException();
        }

        public Task<Item[]> GetAllItems(Guid playerId)
        {
            string[] _repository = File.ReadAllLines("game_dev.txt", Encoding.Default);
            foreach(var player in _repository){
                var p =JsonConvert.DeserializeObject<Player>(player);
                if(p.Id == playerId){
                    return Task.FromResult(p.Items.ToArray());
                }
            }
            throw new NotFoundException();
        }

        public Task<Item> UpdateItem(Guid playerId,Guid itemId, Item item)
        {
            string[] _repository = File.ReadAllLines("game_dev.txt", Encoding.Default);
            for(int i = 0; i<_repository.Length; i++){
                var p =JsonConvert.DeserializeObject<Player>(_repository[i]); 
                if(p.Id == playerId){
                    foreach(var it in p.Items){
                        if(it.Id == itemId){
                            it.Price = item.Price;
                            _repository[i] = JsonConvert.SerializeObject(p);
                            File.WriteAllLines("game_dev.txt", _repository);
                            return Task.FromResult(it);
                        }
                        
                    }
                }
                _repository[i] = JsonConvert.SerializeObject(p);
                File.WriteAllLines("game_dev.txt", _repository);
                
            }
            
            
            throw new NotFoundException();
        }

        public Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            string[] _repository = File.ReadAllLines("game_dev.txt", Encoding.Default);
            List<Player> newlist = new List<Player>();
            foreach(var player in _repository){
                var p =JsonConvert.DeserializeObject<Player>(player);
                newlist.Add(p);
            }
            Item _item = new Item();
            foreach(var p in newlist){
                if(p.Id == playerId){
                    foreach(var it in p.Items){
                        if(it.Id == itemId){
                            _item = it;
                            p.Items.Remove(it);
                            
                        }
                        

                    }
                    savefile();
                    return Task.FromResult(_item);   
                }
                
            }
            void savefile(){
                string[] newarray = new string[newlist.Count];
                for(int i=0; i<newlist.Count; i++){
                    newarray[i] = JsonConvert.SerializeObject(newlist[i]);
                }
                File.WriteAllLines("game_dev.txt", newarray);
            }
            throw new NotFoundException();
        }

        public Task<Player[]> GetPlayersWithScore(int score){
            throw new NotImplementedException();
        }

        public Task<Player> GetPlayerWithName(string name){
            throw new NotImplementedException();
        }

        public Task<Player[]> GetPlayersWithItemType(ItemType itemType){
            throw new NotImplementedException();
        }

        public Task<Player> IncrementPlayerScore(Guid id, int increment){
            throw new NotImplementedException();
        }

    }
}