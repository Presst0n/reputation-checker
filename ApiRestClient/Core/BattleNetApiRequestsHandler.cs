using Newtonsoft.Json;
using RepDataCollector.Models;
using RepDataCollector.Models.Responses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepDataCollector.Core
{
    public class BattleNetApiRequestsHandler
    {
        const string AllUserCharactersEndpoint = "https://eu.api.blizzard.com/profile/user/wow";
        const string userInfoEndpoint = "https://eu.battle.net/oauth/userinfo";


        private readonly RestClient _client;
        private readonly AuthService _authService;

        private string Access_Token { get; set; }

        public BattleNetApiRequestsHandler(string clientId, string clientSecret)
        {
            _authService = new AuthService(clientId, clientSecret);
            _client = new RestClient();
        }

        public async Task<bool> AuthorizeAsync()
        {
            bool isSuccess = await _authService.AuthorizeAsync();

            if (!isSuccess)
                return isSuccess;

            var response = await _authService.GetAccessTokenAsync();
            Access_Token = response.Access_Token;

            return isSuccess;
        }

        public async Task<UserInfoResponse> GetUserInfoAsync()
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

        public async Task ValidateAccessTokenAsync()
        {
            var result = await _authService.ValidateTokenAsync(Access_Token);

            // if token is still valid, do nothing, otherwise ask to log-in again.
        }

        public async Task<List<Character>> GetAllUserWowCharactersAsync()
        {
            if (string.IsNullOrEmpty(Access_Token))
                return null;

            _client.BaseUrl = new Uri(AllUserCharactersEndpoint);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", $"Bearer {Access_Token}");
            request.AddParameter("namespace", $"profile-eu");
            request.AddParameter("locale", $"eu");
            IRestResponse response = await _client.ExecuteAsync(request);


            var deserializedResponse = JsonConvert.DeserializeObject<UserCharactersResponse>(response?.Content);

            var characters = new List<Character>();
            deserializedResponse.WowAccounts.ToList()
                .ForEach(w => w.Characters.ToList()
                .ForEach(c =>  characters.Add(c)));

            return characters;
        }

        public async Task<List<ReputationResponse>> GetReputationsFromAllCharactersAsync(List<Character> characters)
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
                request.AddParameter("locale", "eu_EU");

                IRestResponse response = await _client.ExecuteAsync(request);

                var deserializedResponse = JsonConvert.DeserializeObject<ReputationResponse>(response?.Content);
                reputations.Add(deserializedResponse);
            }

            return reputations;
        }

        public async Task<FactionResponse> GetFactionAsync(string factionEndpoint)
        {
            if (string.IsNullOrEmpty(factionEndpoint))
                return null;

            string uri = factionEndpoint;
            string newNamespace = "?namespace=static-9.0.5_37760-eu";

            // that's the issue here "9.0.5_37760-eu"
            if (!factionEndpoint.EndsWith(newNamespace))
            {
                var index = factionEndpoint.IndexOf('?');

                var baseUri = factionEndpoint.Remove(index);

                uri = baseUri + newNamespace;

            }

            _client.BaseUrl = new Uri(uri);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", $"Bearer {Access_Token}");
            IRestResponse response = await _client.ExecuteAsync(request);

            return JsonConvert.DeserializeObject<FactionResponse>(response?.Content);
        }
    }
}

