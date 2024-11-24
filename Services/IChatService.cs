using CustomAuth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomAuth.Services
{
    public interface IChatService
    {
        Task<IEnumerable<ChatMessage>> GetMessagesAsync(string from, string to);
        Task<ChatMessage> SaveMessageAsync(string from, string to, string message);
        Task<IEnumerable<string>> GetAllUsersAsync();
    }
}