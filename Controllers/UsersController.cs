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
        [HttpPost("register")]
        public ActionResult<User> Register([FromBody] User user)
        {
            DBservices db = new DBservices();
            int result = db.Register(user);

            if (result == 1)
                return Ok(user);
            return BadRequest("משתמש כבר קיים");
        }
    }
}
