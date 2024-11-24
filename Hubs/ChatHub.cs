using Microsoft.AspNetCore.SignalR;
using CustomAuth.Models;
using CustomAuth.Services;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CustomAuth.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private static readonly ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>();
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IChatService chatService, ILogger<ChatHub> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            OnlineUsers.TryAdd(connectionId, "Anonymous");
            await UpdateOnlineUsersCount();
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string to, string message)
        {
            try
            {
                var from = Context.Items["Username"] as string ?? "Anonymous";
                _logger.LogInformation($"Attempting to send message from {from} to {to}: {message}");
                var chatMessage = await _chatService.SaveMessageAsync(from, to, message);
                _logger.LogInformation($"Message saved successfully with ID: {chatMessage.ID}");
                await Clients.All.SendAsync("ReceiveMessage", new { From = from, To = to, Message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in SendMessage. From: {Context.Items["Username"]}, To: {to}");
                throw;
            }
        }

        public async Task SetUsername(string username)
        {
            string connectionId = Context.ConnectionId;
            Context.Items["Username"] = username;
            OnlineUsers[connectionId] = username;
            await UpdateOnlineUsersCount();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            OnlineUsers.TryRemove(connectionId, out _);
            await UpdateOnlineUsersCount();
            await base.OnDisconnectedAsync(exception);
        }

        private async Task UpdateOnlineUsersCount()
        {
            int count = OnlineUsers.Count;
            _logger.LogInformation($"Updating online users count: {count}");
            await Clients.All.SendAsync("UpdateOnlineUsers", count);
        }
    }
}