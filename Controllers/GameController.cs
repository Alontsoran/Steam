using Microsoft.AspNetCore.Mvc;
using Steam.Models;

namespace Steam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {    // קריאה של כל המשחקים ואז הצגהתם
        [HttpGet]
        public ActionResult<IEnumerable<Game>> Get()
        {
            return Game.read();

        }
        // Get games by price
        [HttpGet("price/{price}")]
        public ActionResult<IEnumerable<Game>> GetByPrice(int price)
        {
            return Game.GetByPrice(price);
        }
        // Get games by rank
        [HttpGet("rank/{rank}")]
        public ActionResult<IEnumerable<Game>> GetByRankScore(int rank)
        {
            return Game.GetByRankScore(rank);
        }




        // זה ריק 
        [HttpGet("{id}")]
        public void GetGameById(int id)
        {
          
        }
        //החזרה של הודעת קונפליקט עבור משחק שקיים כבר
        [HttpPost]
        public bool  Post([FromBody] Game game)
        {
            if (game.Insert() == false)
            {
                return false;
            }
            return true;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void DeleteById(int id)
        {
            Game.deleteGame(id);
        }
    }
}