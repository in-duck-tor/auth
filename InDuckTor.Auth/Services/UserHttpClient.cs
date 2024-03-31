using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace InDuckTor.Auth.Services
{
    public record UserShort(string Login, string[] Roles, string[] Permissions, bool IsActive);

    public interface IUserHttpClient
    {
        Task<UserShort?> GetUserShortInfo(string userId);
    }
    public class UserHttpClient : IUserHttpClient
    {
        private HttpClient _client;

        public UserHttpClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://89.19.214.8");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE3MTIzNTk4NTQsImV4cCI6MTc0ODM1OTg1NCwiaXNzIjoiaW4tZHVjay10b3IiLCJjbGllbnRfaWQiOiJhbmd1bGFyX3NwYSIsInN1YiI6IjIiLCJhdXRoX3RpbWUiOjE3MTIzNTk4NTQsImlkcCI6ImxvY2FsIiwiYWNjb3VudF90eXBlIjoiY2xpZW50Iiwicm9sZXMiOlsiY2xpZW50IiwiZW1wbG95ZWUiXSwianRpIjoiRTEzRDE0MUZCN0FBMzY2OTgxQkIyQzJCMzdGRDM1RDQiLCJzaWQiOiJEMzJFMzI1ODY1Q0E3MURGMTcyQzNEMjczMUJBNEYzQSIsImlhdCI6MTcxMjM1OTg1NCwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsImVtYWlsIl0sImFtciI6WyJwd2QiXX0.EBw9IZEszWHDt_wrmXL4Ag3DScS8LKnbIK9Gk7oztBPSqxAG8ai56zEPK-JW2-yClmQ9xkhPjBdnMgkEtGxvJGX0YEmQRQEXfK2svrMUeswH0YixG2TxB-WGIHqYOs3OMf6Zx-16OvCWy1G18sOg_tz9WrGZnUfda-uDoKI-woqq-MvmeQGN9QgGdgAT6SYRRmnS1m8nbnYHSbUyfjYoPJB2FOfMP2g6AdWwu8ssIUmF3xAq8fGV0aFNAhU6oCKkFPOM4WH7uTHKwugijJqiZUGYAJLXTwb7qEEa2PXJ2R6UCWR9HR3Gk9sj6szonvJ9EMpjA7BOV5hwpqfTC8CFQQ");
        }

        [HttpGet]
        public async Task<UserShort?> GetUserShortInfo(string userId)
        {
            return await _client.GetFromJsonAsync<UserShort?>(($"/api/v1/user/{userId}/claims"));
        }
    }
}
