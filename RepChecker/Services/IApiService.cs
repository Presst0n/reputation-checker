using RepDataCollector.Core;
using RepDataCollector.Models;
using RepDataCollector.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepChecker.Services
{
    public interface IApiService
    {
        Task<bool> AuthorizeAsync();
        Task<UserInfoResponse> GetUserInfoAsync();
        Task ValidateAccessTokenAsync();
        Task<List<Character>> GetAllUserWowCharactersAsync();
        Task<List<ReputationResponse>> GetReputationsByCharactersAsync(List<Character> characters);
    }
}