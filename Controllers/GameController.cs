using Microsoft.AspNetCore.Mvc;
using Steam.DAL;
using Steam.Models;

namespace Steam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {    // קריאה של כל המשחקים ואז הצגהתם
        [HttpPost("user/{userId}/game/{gameId}")]
        public ActionResult AddGame(string userId, string gameId)
        {
            DBservices db = new DBservices();
            int result = db.AddGame(userId, gameId);

            if (result == 1)
                return Ok();

            return BadRequest("Game already exists for user");
        }

        // קבלת המשחקים של משתמש ספציפי
        [HttpGet("user/{userId}/games")]
        public ActionResult<List<Game>> GetUserGames(string userId)  // קבלת userId כפרמטר מה-route
        {
            try
            {
                DBservices db = new DBservices();
                return db.ShowUserGames(userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("user/{userId}/games/{Price}")]
        public ActionResult<List<Game>> GetUserPriceGames(float Price,string userId)  // קבלת userId כפרמטר מה-route
        {
            try
            {
                DBservices db = new DBservices();
                return db.ShowUserWithPriceGames(userId, Price);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("user/{userId}/games/Rank/{Rank}")]
        public ActionResult<List<Game>> GetUserRankGames(float Rank, string userId)  // קבלת userId כפרמטר מה-route
        {
            try
            {
                DBservices db = new DBservices();
                return db.showGamesWIthRank(userId, Rank);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // קבלת כל המשחקים
        // קבלת כל המשחקים
        [HttpGet]
        public ActionResult<List<Game>> GetAllGames()
        {
            DBservices db = new DBservices();
            List<Game> games = db.GetAllGames();
            if (games != null && games.Any())
                return Ok(games);
            return NotFound("No games found");
        }
        // קריאה של כל המשחקים ואז הצגהתם
        [HttpDelete("user/{userId}/game/{gameId}")]
        public ActionResult DeleteGame(string userId, string gameId)
        {
            DBservices db = new DBservices();
            int result = db.DeleteGame(userId, gameId);

            if (result == 1)
                return Ok();

            return BadRequest("Game already exists for user");
        }

    }


}