using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using MikroCopUsers.Data;
using MikroCopUsers.Models;
using MikroCopUsers.Services;

namespace MikroCopUsers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController :
        ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;
        private readonly PasswordHasher _passwordHasher;

        public UsersController(ApplicationDbContext context, ILogger<UsersController> logger,
            PasswordHasher passwordHasher)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            user.PasswordHash = _passwordHasher.Hash(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User updated)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.UserName = updated.UserName;
            user.FullName = updated.FullName;
            user.Email = updated.Email;
            user.MobileNumber = updated.MobileNumber;
            user.Language = updated.Language;
            user.Culture = updated.Culture;

            if (!string.IsNullOrWhiteSpace(updated.PasswordHash))
            {
                user.PasswordHash = _passwordHasher.Hash(updated.PasswordHash);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/users/validate-password
        [HttpPost("validate-password")]
        public async Task<IActionResult> ValidatePassword([FromBody] LoginRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == req.UserName);
            if (user == null) return Unauthorized("User not found");

            var hashed = _passwordHasher.Hash(req.Password);
            if (hashed == user.PasswordHash)
            {
                return Ok("Password is valid");
            }

            return Unauthorized("Invalid password");
        }

        public class LoginRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}