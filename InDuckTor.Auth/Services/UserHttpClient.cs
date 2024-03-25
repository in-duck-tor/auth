using Microsoft.AspNetCore.Mvc;

namespace InDuckTor.Auth.Services
{
    public record UserShort(string Login, string[] Roles, string[] Permissions, bool IsActive);

    public interface IUserHttpClient
    {
        Task GetUserShortInfo(string userId);
    }
    public class UserHttpClient : IUserHttpClient
    {
        private HttpClient _client;

        public UserHttpClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://89.19.214.8/api/v1");
        }

        [HttpGet]
        public async Task GetUserShortInfo(string userId)
        {
            var response = await _client.GetFromJsonAsync<UserShort>(($"/{userId}"));
        }
    }
}
