using CRUD.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly CrudDbContext _context;
        private const int AdminId = 87; // Hardcoded admin ID

        public ChatController(CrudDbContext context)
        {
            _context = context;
        }

        // Fetch conversation between specified user and admin
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetConversationWithAdmin(int userId)
        {
            // Fetch all messages where sender/recipient is either the userId or AdminId
            var messages = await _context.ChatMessages
                .Where(m =>
                    (m.SenderId == userId && m.RecipientId == AdminId) ||
                    (m.SenderId == AdminId && m.RecipientId == userId)
                )
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            return Ok(new { messages });
        }
    }
}
