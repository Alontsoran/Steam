using Microsoft.AspNetCore.Mvc;
using Steam.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Steam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return Ok(Steam.Models.User.read());
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {

            // בדיקת ID
            if (user.Id <= 0)
            {
                return BadRequest("ID חייב להיות מספר חיובי");
            }

            // בדיקת שם
            if (string.IsNullOrEmpty(user.Name))
            {
                return BadRequest("חובה להזין שם");
            }

            // בדיקת אימייל
            if (string.IsNullOrEmpty(user.Email1))
            {
                return BadRequest("חובה להזין אימייל");
            }
            if (!user.Email1.Contains("@"))
            {
                return BadRequest("אימייל לא תקין");
            }

            // בדיקת סיסמה
            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("חובה להזין סיסמה");
            }
            if (user.Password.Length < 6)
            {
                return BadRequest("סיסמה חייבת להכיל לפחות 6 תווים");
            }

            // בדיקה אם משתמש קיים
            if (user.Insert())
            {
                return Ok(user);
            }
            return BadRequest("משתמש כבר קיים");
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
