using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eumel.Dj.Mobile.Services
{
    public interface IChatService
    {
        Task<IEnumerable<ChatEntry>> GetChatHistory();
    }
}