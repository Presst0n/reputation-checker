using RepDataCollector.Core;
using RepDataCollector.Models.Responses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace RepChecker.Services
{
    public class ApiService : BattleNetApiRequestsHandler, IApiService
    {
        public ApiService() : base()
        {
            AuthService = new AuthService(ConfigurationManager.AppSettings.Get("BlizzardApiId"), 
                ConfigurationManager.AppSettings.Get("BlizzardApiPassword"));
        }

    }
}
