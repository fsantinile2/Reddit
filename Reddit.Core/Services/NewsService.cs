using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Reddit.Core.Interfaces;
using Reddit.Core.Models;
using System.Net.Http.Headers;

namespace Reddit.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private string _news = String.Empty;

        public NewsService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
        }
        public async Task<int> Get(string token)
        {
            _memoryCache.TryGetValue(_news, out NewsModel quantityBefore);

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("User-Agent", "MockClient/0.1 by Me");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            client.BaseAddress = new Uri("https://oauth.reddit.com");

            var result = await client.GetAsync($"{client.BaseAddress}/new");

            var response = JsonConvert.DeserializeObject<NewsModel>(await result.Content.ReadAsStringAsync());

            var quantityActual = response.Data.Children.DistinctBy(x => x.Data.Title);

            if (quantityBefore != null)
            {
                return quantityBefore.Data.Children.Except(quantityActual).Count();
            }

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1000),
            };

            _memoryCache.Set(_news, response, memoryCacheEntryOptions);

            return quantityActual.Count();
        }
    }
}
