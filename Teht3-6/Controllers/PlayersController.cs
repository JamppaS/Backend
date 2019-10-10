using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gamewebapi
{
    [Route("api/players/")]
    [ApiController]
    public class PlayerController :ControllerBase{

        private IRepository _repository;

        public PlayerController(IRepository repository) => _repository = repository;


        [HttpGet]
        [Route("{playerId:int}")]
        public async Task<Player> Get(Guid id){
            return await _repository.Get(id);
        }
         
        

        public async Task<IActionResult> GetPlayerWithName(string name){
            
            return Ok(await _repository.GetPlayerWithName(name));
        }
        // [HttpGet]
        // public async Task<IActionResult> GetPlayerWithName(string name){
        //     name = HttpContext.Request.Query["name"][0];
        //     return Ok(await _repository.GetPlayerWithName(name));
        // }
        
        
        public async Task<Player[]> GetPlayersWithItemType(ItemType itemType){
            return await _repository.GetPlayersWithItemType(itemType);
        }
        
        public async Task<Player[]> GetPlayersWithScore(int score){
            return await _repository.GetPlayersWithScore(score);
        }

        
        public async Task<Player> IncrementPlayerScore(Guid id, int increment){
            return await _repository.IncrementPlayerScore(id,increment);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(){
            if(!String.IsNullOrEmpty(HttpContext.Request.Query["name"])){
                string name = HttpContext.Request.Query["name"];
                return Ok(await GetPlayerWithName(name));
            }
            if(!String.IsNullOrEmpty(HttpContext.Request.Query["score"])){
                int score = int.Parse(HttpContext.Request.Query["score"]);
                return Ok(await GetPlayersWithScore(score));
            }
            /*if(!String.IsNullOrEmpty(HttpContext.Request.Query["itemtype"])){
                ItemType itemType = (ItemType)int.Parse(HttpContext.Request.Query["itemtype"]);
            }*/
            return Ok(await _repository.GetAll());
        }
        [HttpPost]
        [Route("")]
        public async Task<Player> Create(NewPlayer player){
            //_logger.LogInformation("Creating player with name " + newPlayer.Name);
            var newplayer = new Player()
            {
                Id = Guid.NewGuid(),
                Name = player.Name,
                CreationTime = DateTime.Now
            };
            //await _repository.Create(newplayer);
            return await _repository.Create(newplayer);
        }
        [HttpPost]
        [Route("{playerId}")]
        public Task<Player> Modify(Guid id, Player modifiedPlayer){
            return _repository.Modify(id, modifiedPlayer);
        }
        [HttpDelete]
        [Route("{playerId}")]
        public Task<Player> Delete(Guid id){
            return _repository.Delete(id);
        }
        
            
    }
}
