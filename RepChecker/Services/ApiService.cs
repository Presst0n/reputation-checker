using RepDataCollector.Core;
using System.Configuration;

namespace RepChecker.Services
{
    public class ApiService : BattleNetApiRequestsHandler, IApiService
    {
        public ApiService()
        {
            AuthService = new AuthService(ConfigurationManager.AppSettings.Get("BlizzardApiId"), 
                ConfigurationManager.AppSettings.Get("BlizzardApiPassword"));
        }
    }
}
