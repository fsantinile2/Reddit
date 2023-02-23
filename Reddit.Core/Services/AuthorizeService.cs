using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Reddit.Core.Interfaces;
using System.Net.Http.Headers;

namespace Reddit.Core.Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly RedditConfig _redditConfig;
        private readonly IHttpClientFactory _httpClientFactory;
        private IMemoryCache _memoryCache;
        private string _token = String.Empty;

        public AuthorizeService(IOptions<RedditConfig> options, IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _redditConfig = options.Value;
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
        }
        public async Task<string> Authorize()
        {
            _memoryCache.TryGetValue(_token, out string token);

            if (String.IsNullOrEmpty(token))
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("https://www.reddit.com/api/v1/access_token");

                var authenticationString = $"{_redditConfig.ApplicationID}:{_redditConfig.SecretKey}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                var dict = new Dictionary<string, string>();
                dict.Add("grant_type", "password");
                dict.Add("username", $"{_redditConfig.Username}");
                dict.Add("password", $"{_redditConfig.Password}");
                dict.Add("scope", "*");
                var req = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress) { Content = new FormUrlEncodedContent(dict) };
                var res = await client.SendAsync(req);

                if (res.IsSuccessStatusCode)
                {
                    var response = JsonConvert.DeserializeObject<RedditToken>(await res.Content.ReadAsStringAsync());

                    var memoryCacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(86400),
                    };

                    _memoryCache.Set(_token, response?.Access_token, memoryCacheEntryOptions);

                    return response.Access_token;
                }
                return _token;
            }
            return token;

        }
    }
}
