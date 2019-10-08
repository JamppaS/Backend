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
            List<Player> players = new List<Player>();


            using (StreamReader r = new StreamReader("game_dev.txt"))
            {
                string json = r.ReadToEnd();
                if (json != null || json != "")
                {
                     players = JsonConvert.DeserializeObject<List<Player>>(json);
                    
                }
            }

            foreach (Player player in players){
                Console.WriteLine(player);
                if(player.Id == id){
                    return Task.FromResult(player);
                }
            }
            return null;
        }
        public Task<Player[]> GetAll(){
            List<Player> players = new List<Player>();

            using (StreamReader r = new StreamReader("game_dev.txt"))
            {
                string json = r.ReadToEnd();
                if (json != null || json != "")
                {
                     players = JsonConvert.DeserializeObject<List<Player>>(json);
                    
                }
            }
            
            return Task.FromResult(players.ToArray());
        }
        public Task<Player> Create(Player player){
            List<Player> players = new List<Player>();

            var newplayer = new Player()
             {
                 Id = Guid.NewGuid(),
                 Name = player.Name
             };
            using (StreamReader r = new StreamReader("game_dev.txt"))
            {
                string json = r.ReadToEnd();
                if (json != null || json != "")
                {
                     players = JsonConvert.DeserializeObject<List<Player>>(json);
                    
                }
            }

            players.Add(newplayer);
            File.WriteAllText("game_dev.txt", JsonConvert.SerializeObject(players));
            return Task.FromResult(newplayer);
        }
        public Task<Player> Modify(Guid id, ModifiedPlayer player){

            List<Player> players = new List<Player>();

            using (StreamReader r = new StreamReader("game_dev.txt"))
            {
                string json = r.ReadToEnd();
                if (json != null || json != "")
                {
                     players = JsonConvert.DeserializeObject<List<Player>>(json);
                    
                }
            }

            foreach(var oldplayer in players){
                if(oldplayer.Id == id){
                    oldplayer.Score = player.Score;
                    File.WriteAllText("game_dev.txt", JsonConvert.SerializeObject(players));
                    return Task.FromResult(oldplayer);
                }
            }
            return null;
        }
        public Task<Player> Delete(Guid id){

            List<Player> players = new List<Player>();

            using (StreamReader r = new StreamReader("game_dev.txt"))
            {
                string json = r.ReadToEnd();
                if (json != null || json != "")
                {
                     players = JsonConvert.DeserializeObject<List<Player>>(json);
                    
                }
            }

            for(int i = players.Count()-1; i >= 0; i--){
                if(players[i].Id == id){
                    Player deletedPlayer = players[i];
                    players.RemoveAt(i);
                    File.WriteAllText("game_dev.txt", JsonConvert.SerializeObject(players));
                    return Task.FromResult(deletedPlayer);
                }
            }
            return null;
        }
    }
}