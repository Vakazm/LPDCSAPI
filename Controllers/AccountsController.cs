using LPDCSAPI.Classes;
using Microsoft.AspNetCore.Mvc;

namespace LPDCSAPI.Controllers
{
    // API controller, responsible for most of the program's logic
    [ApiController]
    [Route ("api/[controller]")]
    public class AccountsController : Controller
    {
        // Register dealer
        [HttpPost ("Register")]
        public IActionResult Register ([FromBody] Dealer.Reginfo reginfo)
        {
            // Check if registration info is correct
            if (!ModelState.IsValid) { return BadRequest (ModelState); }
            if (string.IsNullOrEmpty (reginfo.Username))
            {
                return BadRequest (new { Message = "Login failed, username was not provided" });
            }
            if (string.IsNullOrEmpty (reginfo.Email))
            {
                return BadRequest (new { Message = "Login failed, email was not provided" });
            }

            // Check if username or email provided is already in database
            if (Context.IsDealer (reginfo.Username, reginfo.Email))
            {
                return BadRequest (new { Message = "Registration failed, user already exists" });
            }

            // All good, create new dealer, add it to database and return their token
            return Ok (new { Message = $"Registration successful, access token = {Context.AddDealer (reginfo)}" });
        }

        // Login dealer
        [HttpPost ("Login")]
        public IActionResult Login ([FromBody] Dealer.Reginfo reginfo)
        {
            // Check if all login information provided
            string? username = reginfo.Username, email = reginfo.Email, password = reginfo.Password;
            if (string.IsNullOrEmpty (username) && string.IsNullOrEmpty (email))
            {
                return BadRequest (new { Message = "Login failed, neither username nor email was provided" });
            }

            // Try to login
            string? token = Context.LoginDealer (username, email, password);
            if (token == null) { return BadRequest (new { Message = "Login failed, wrong login or password" }); }
            else { return Ok (new { Message = $"Login successful, access token = {token}" }); }
        }

    }
}
