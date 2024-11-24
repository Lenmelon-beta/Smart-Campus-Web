using Microsoft.EntityFrameworkCore;
using CustomAuth.Data;
using CustomAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAuth.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _chatContext;
        private readonly AuthDbContext _authContext;
        private readonly ILogger<ChatService> _logger;

        public ChatService(ChatDbContext chatContext, AuthDbContext authContext, ILogger<ChatService> logger)
        {
            _chatContext = chatContext;
            _authContext = authContext;
            _logger = logger;
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesAsync(string from, string to)
        {
            try
            {
                var messages = await _chatContext.Messages
                    .Where(m => (m.From == from && m.To == to) || (m.From == to && m.To == from))
                    .OrderBy(m => m.ID)
                    .ToListAsync();

                _logger.LogInformation($"Retrieved {messages.Count} messages between {from} and {to}");
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetMessagesAsync. From: {from}, To: {to}");
                throw;
            }
        }

        public async Task<ChatMessage> SaveMessageAsync(string from, string to, string message)
        {
            try
            {
                var chatMessage = new ChatMessage
                {
                    From = from,
                    To = to,
                    Message = message
                };

                _chatContext.Messages.Add(chatMessage);
                _logger.LogInformation($"Attempting to save message: From={from}, To={to}, Message={message}");
                await _chatContext.SaveChangesAsync();

                _logger.LogInformation($"Successfully saved message with ID: {chatMessage.ID}");
                return chatMessage;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"DbUpdateException in SaveMessageAsync. From: {from}, To: {to}");
                _logger.LogError($"Inner exception: {ex.InnerException?.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in SaveMessageAsync. From: {from}, To: {to}");
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetAllUsersAsync()
        {
            try
            {
                var users = await _authContext.UserAccounts.Select(u => u.Username).ToListAsync();
                _logger.LogInformation($"Retrieved {users.Count} users");
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllUsersAsync");
                throw;
            }
        }
    }
}