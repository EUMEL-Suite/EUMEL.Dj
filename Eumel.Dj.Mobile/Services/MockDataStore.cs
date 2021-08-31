using Eumel.Dj.Mobile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eumel.Dj.Mobile.Services
{
    public class ServiceDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public ServiceDataStore()
        {
            var cl = new HttpClientHandler();
            cl.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
            var client = new HttpClient(cl);

            var svc = new EumelDjServiceClient("https://192.168.178.37:443", client);
            items = svc.GetSongsAsync(0, int.MaxValue).Result
                //.Select(x => new Item() { Id = x, Description = x, Text = x.Substring(x.LastIndexOf("\\" + 1, StringComparison.Ordinal)).Replace(".mp3", "") }).ToList();
                .Select(x => new Item() { Id = Guid.NewGuid().ToString(), Description = $"{x.Artist} - {x.Name}\r\n{x.Album}", Text = $"{x.Artist} - {x.Name}", Location = x.Location }).ToArray().ToList();
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.FirstOrDefault(arg => arg.Id == item.Id);
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.FirstOrDefault(arg => arg.Id == id);
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}