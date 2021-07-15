using RepDataCollector.Models;
using RepDataCollector.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepDataCollector.Core
{
    public interface IBattleNetApiRequestsHandler
    {
        Task<bool> AuthorizeAsync();
        Task<List<Character>> GetAllUserWowCharactersAsync();
        Task<List<ReputationResponse>> GetReputationsByCharactersAsync(List<Character> characters);
        Task<UserInfoResponse> GetUserInfoAsync();
        Task ValidateAccessTokenAsync();
    }
}