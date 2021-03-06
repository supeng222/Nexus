﻿using Aiursoft.Archon.SDK.Models;
using Aiursoft.Handler.Exceptions;
using Aiursoft.Handler.Models;
using Aiursoft.Scanner.Interfaces;
using Aiursoft.XelNaga.Models;
using Aiursoft.XelNaga.Services;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Aiursoft.Archon.SDK.Services.ToArchonServer
{
    public class ArchonApiService : IScopedDependency
    {
        private readonly ArchonLocator _archonLocator;
        private readonly HTTPService _http;

        public ArchonApiService(
            ArchonLocator serviceLocation,
            HTTPService http)
        {
            _archonLocator = serviceLocation;
            _http = http;
        }

        public async Task<IndexViewModel> GetKey()
        {
            var url = new AiurUrl(_archonLocator.Endpoint, "Home", "Index", new { });
            var result = await _http.Get(url, true);
            var JResult = JsonConvert.DeserializeObject<IndexViewModel>(result);

            if (JResult.Code != ErrorType.Success)
                throw new AiurUnexceptedResponse(JResult);
            return JResult;
        }

        public async Task<AccessTokenViewModel> AccessTokenAsync(string appId, string appSecret)
        {
            var url = new AiurUrl(_archonLocator.Endpoint, "API", "AccessToken", new AccessTokenAddressModel
            {
                AppId = appId,
                AppSecret = appSecret
            });
            var result = await _http.Get(url, true);
            var JResult = JsonConvert.DeserializeObject<AccessTokenViewModel>(result);

            if (JResult.Code != ErrorType.Success)
                throw new AiurUnexceptedResponse(JResult);
            return JResult;
        }
    }
}
