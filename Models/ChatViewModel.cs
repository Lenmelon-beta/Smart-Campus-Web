using System.Collections.Generic;

namespace CustomAuth.Models
{
    public class ChatViewModel
    {
        public string CurrentUser { get; set; }
        public IEnumerable<string> Contacts { get; set; }
    }
}