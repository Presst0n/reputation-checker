using Newtonsoft.Json;
using RepDataCollector.Models;
using RepDataCollector.Models.Responses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepDataCollector.Core
{
    public abstract class BattleNetApiRequestsHandler/* : IBattleNetApiRequestsHandler*/
    {
        private const string allUserCharactersEndpoint = "https://eu.api.blizzard.com/profile/user/wow";
        private const string userInfoEndpoint = "https://eu.battle.net/oauth/userinfo";

        private readonly RestClient _client;
        protected AuthService AuthService { get; set; }
        private string Access_Token { get; set; }

        public BattleNetApiRequestsHandler() 
        {
            _client = new RestClient();
        }

        public BattleNetApiRequestsHandler(string clientId, string clientSecret) : this()
        {
            AuthService = new AuthService(clientId, clientSecret);
        }

        public virtual async Task<bool> AuthorizeAsync()
        {
            bool isSuccess = await AuthService.AuthorizeAsync();

            if (!isSuccess)
                return isSuccess;

            var response = await AuthService.GetAccessTokenAsync();
            Access_Token = response.Access_Token;

            return isSuccess;
        }

        public virtual async Task<UserInfoResponse> GetUserInfoAsync()
        {
            if (string.IsNullOrEmpty(Access_Token))
                return null;

            _client.BaseUrl = new Uri(userInfoEndpoint);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", $"Bearer {Access_Token}");
            IRestResponse response = await (_client.ExecuteAsync(request));

            return JsonConvert.DeserializeObject<UserInfoResponse>(response?.Content);
        }

        public virtual async Task ValidateAccessTokenAsync()
        {
            var result = await AuthService.ValidateTokenAsync(Access_Token);

            // if token is still valid, do nothing, otherwise ask to log-in again.
        }

        public virtual async Task<List<Character>> GetAllUserWowCharactersAsync()
        {
            if (string.IsNullOrEmpty(Access_Token))
                return null;

            _client.BaseUrl = new Uri(allUserCharactersEndpoint);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", $"Bearer {Access_Token}");
            request.AddParameter("namespace", $"profile-eu");
            request.AddParameter("locale", $"eu");
            IRestResponse response = await _client.ExecuteAsync(request);

            var deserializedResponse = JsonConvert.DeserializeObject<UserCharactersResponse>(response?.Content);

            var characters = deserializedResponse?.WowAccounts?.ToList()
                .SelectMany(w => w.Characters).ToList();

            return characters;
        }

        public virtual async Task<List<ReputationResponse>> GetReputationsByCharactersAsync(List<Character> characters)
        {
            if (string.IsNullOrEmpty(Access_Token))
                return null;

            if (characters is null || characters?.Count == 0)
                return null;

            var reputations = new List<ReputationResponse>();

            foreach (var character in characters)
            {
                var endpoint = $"https://eu.api.blizzard.com/profile/wow/character/{character.Realm.Slug}/{character.Name.ToLowerInvariant()}/reputations";
                _client.BaseUrl = new Uri(endpoint);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", $"Bearer {Access_Token}");
                request.AddParameter("namespace", "profile-eu");
                request.AddParameter("locale", "en_US");

                IRestResponse response = await _client.ExecuteAsync(request);

                var deserializedResponse = JsonConvert.DeserializeObject<ReputationResponse>(response?.Content);
                reputations.Add(deserializedResponse);
            }

            return reputations;
        }
    }
}

