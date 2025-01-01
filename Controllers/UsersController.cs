using Microsoft.AspNetCore.Mvc;
using Steam.DAL;
using Steam.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Steam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
      

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] User loginUser)
        {
            DBservices db = new DBservices();
            User user = db.Login(loginUser.Email, loginUser.Password);

            if (user != null)
                return Ok(user);

            return BadRequest("שם משתמש או סיסמה שגויים");
        }

        // POST api/<ValuesController>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            DBservices db = new DBservices();
            int result = db.Register(user);

            if (result == 1)
                return Ok(user);
            return BadRequest("משתמש כבר קיים");
        }

       

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpPost]
        [Route("register")]  // או [Route("Register")]
        public ActionResult<User> Register([FromBody] User user)
        {
            DBservices db = new DBservices();
            Console.WriteLine($"Received user: {user}"); // הדפסת המשתמש שהתקבל
            int result = db.Register(user);
            Console.WriteLine($"Register result: {result}"); // הדפסת התוצאה
            if (result == 1)
                return Ok(user);
            return BadRequest("משתמש כבר קיים");
        }
        [HttpPut("update")]
        public IActionResult UpdateUser([FromBody] User user, [FromQuery] string newpassword)
        {
            try
            {
                // אם לא התקבל אובייקט משתמש בגוף הבקשה
                if (user == null)
                    return BadRequest("User object was null");

                // יצירת אובייקט DBservices
                DBservices db = new DBservices();

                // קריאה לפונקציה שמעדכנת את המשתמש במסד הנתונים
                int result = db.UpdateUser(user, newpassword ?? string.Empty);

                // אם התוצאה > 0, פירושו שה-Update הצליח; אחרת נכשל
                return result > 0 ? Ok() : BadRequest("Failed to update user");
            }
            catch (Exception ex)
            {
                // שגיאה בלתי צפויה, מחזירים 500
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("id")]
       public ActionResult GetUserId(string email)
        {
            DBservices u = new DBservices();
            try
            {
                string userId = u.GetUserId(email);
                if (userId != "-1")
                    return Ok(userId);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

