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
            return Ok(Game.read());
        }
        // זה ריק 
        [HttpGet("{id}")]
        public void GetGameById(int id)
        {
          
        }
        //החזרה של הודעת קונפליקט עבור משחק שקיים כבר
        [HttpPost]
        public ActionResult<Game> Post([FromBody] Game game)
        {
            if (game.Insert() == false)
            {
                return BadRequest("משחק כבר קיים");
            }
            Game.Gamelist.Add(game);
            return Ok(game);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}