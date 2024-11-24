using Microsoft.AspNetCore.Mvc;
using CustomAuth.Models;
using CustomAuth.Services;

namespace CustomAuthControllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var currentUser = User.Identity?.Name ?? throw new InvalidOperationException("User not authenticated");
                var viewModel = new ChatViewModel
                {
                    CurrentUser = currentUser,
                    Contacts = await _chatService.GetAllUsersAsync()
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Index action");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string to)
        {
            try
            {
                var currentUser = User.Identity?.Name ?? throw new InvalidOperationException("User not authenticated");
                var messages = await _chatService.GetMessagesAsync(currentUser, to); // Replace "Bob" with the actual current user
                return Json(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetMessages");
                return StatusCode(500, "An error occurred while fetching messages.");
            }
        }
    }
}