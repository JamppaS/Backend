namespace gamewebapi
{
    [Route("api/players")]
    [ApiController]
    public class ItemsController{

        private IRepository _repository;

        public PlayerController(IRepository repository) => _repository = repository;


        [HttpGet]
        [Route("{playerId}")]
        public Task<Player> Get(Guid id){
             return _repository.Get(id);
        }
        [HttpGet]
        [Route("")]
        public Task<Player[]> GetAll(){
            return _repository.GetAll();
        }
        [HttpPost]
        [Route("")]
        public async Task<Player> Create(NewPlayer player){
            //_logger.LogInformation("Creating player with name " + newPlayer.Name);
            var newplayer = new Player()
            {
                Id = Guid.NewGuid(),
                Name = player.Name
            };
            //await _repository.Create(newplayer);
            return await _repository.Create(newplayer);
        }
        [HttpPost]
        [Route("{playerId}")]
        public Task<Player> Modify(Guid id, ModifiedPlayer modifiedPlayer){
            return _repository.Modify(id, modifiedPlayer);
        }
        [HttpDelete]
        [Route("{playerId}")]
        public Task<Player> Delete(Guid id){
            return _repository.Delete(id);
        }
    }
}