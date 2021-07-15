using RepDataCollector.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

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
