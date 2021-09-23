using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class RestChatService : IChatService
    {
        private EumelDjServiceClient Service => DependencyService.Get<IEumelRestServiceFactory>().Build();

        public async Task<IEnumerable<ChatEntry>> GetChatHistory()
        {
            var items = await Service.GetChatHistoryAsync();
            return items.ToArray();
        }
    }
}