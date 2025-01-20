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
        // Get games for table display
        [HttpGet("table")]
        public ActionResult<IEnumerable<object>> GetGamesForTable()
        {
            try
            {
                DBservices db = new DBservices();
                List<Game> games = db.GetAllGames();

                var tableData = games.Select(game => new
                {
                    AppId = game.AppId1,
                    GameName = game.Name1,
                    ReleaseDate = game.Releasedate1.ToShortDateString(),
                    Price = $"${game.Price1:N2}",
                    Score = game.ScoreRank1,
                    Platforms = GetPlatformString(game),
                    Publisher = game.Publisher1,
                    Actions = game.NumberOfPurchases,
                    TOTAL=game.NumberOfPurchases*game.Price1// לשימוש בכפתורי פעולות
                });

                return Ok(tableData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בטעינת נתוני משחקים: {ex.Message}");
            }
        }

        // Get user games for table
        [HttpGet("table/user/{userId}")]
        public ActionResult<IEnumerable<object>> GetUserGamesForTable(string userId)
        {
            try
            {
                DBservices db = new DBservices();
                List<Game> games = db.ShowUserGames(userId);

                var tableData = games.Select(game => new
                {
                    AppId = game.AppId1,
                    GameName = game.Name1,
                    ReleaseDate = game.Releasedate1.ToShortDateString(),
                    Price = $"${game.Price1:N2}",
                    Score = game.ScoreRank1,
                    Platforms = GetPlatformString(game),
                    Publisher = game.Publisher1,
                    Actions = game.NumberOfPurchases
                });

                return Ok(tableData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בטעינת משחקי המשתמש: {ex.Message}");
            }
        }

        // Get filtered games for table
        [HttpGet("table/filter")]
        public ActionResult<IEnumerable<object>> GetFilteredGamesForTable(
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int? minScore = null,
            [FromQuery] string platform = null,
            [FromQuery] string searchTerm = "")
        {
            try
            {
                DBservices db = new DBservices();
                List<Game> games = db.GetFilteredGames(maxPrice, minScore, platform, searchTerm);

                var tableData = games.Select(game => new
                {
                    AppId = game.AppId1,
                    GameName = game.Name1,
                    ReleaseDate = game.Releasedate1.ToShortDateString(),
                    Price = $"${game.Price1:N2}",
                    Score = game.ScoreRank1,
                    Platforms = GetPlatformString(game),
                    Publisher = game.Publisher1,
                    Actions = game.NumberOfPurchases
                });

                return Ok(tableData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בסינון משחקים: {ex.Message}");
            }
        }

        private string GetPlatformString(Game game)
        {
            List<string> platforms = new List<string>();
            if (game.Windows1) platforms.Add("Windows");
            if (game.Mac1) platforms.Add("Mac");
            if (game.Linux1) platforms.Add("Linux");
            return string.Join(", ", platforms);
        }
    }



}