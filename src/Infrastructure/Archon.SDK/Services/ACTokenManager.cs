﻿using Aiursoft.Handler.Exceptions;
using Aiursoft.Handler.Models;
using Aiursoft.Scanner.Interfaces;
using Aiursoft.XelNaga.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace Aiursoft.Archon.SDK.Services
{
    public class ACToken
    {
        public string AppId { get; set; }
        public DateTime Expires { get; set; }
    }

    public class ACTokenManager : IScopedDependency
    {
        private readonly RSAService _rsa;
        public ACTokenManager(RSAService rsa)
        {
            _rsa = rsa;
        }

        public (string, DateTime) GenerateAccessToken(string appId)
        {
            var token = new ACToken
            {
                AppId = appId,
                Expires = DateTime.UtcNow + new TimeSpan(0, 20, 0)
            };
            var tokenJson = JsonConvert.SerializeObject(token, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });
            var tokenBase64 = tokenJson.StringToBase64();
            var tokenSign = _rsa.SignData(tokenJson);
            return ($"{tokenBase64}.{tokenSign}", token.Expires);
        }

        public async Task<string> ValidateAccessToken(string value)
        {
            ACToken token;
            try
            {
                var tokenparts = value.Split('.');
                string tokenBase64 = tokenparts[0], tokenSign = tokenparts[1];
                token = JsonConvert.DeserializeObject<ACToken>(tokenBase64.Base64ToString());
                if (DateTime.UtcNow > token.Expires)
                {
                    throw new AiurAPIModelException(ErrorType.Timeout, "Token was timed out!");
                }
                if (!await _rsa.VerifyData(tokenBase64.Base64ToString(), tokenSign))
                {
                    throw new AiurAPIModelException(ErrorType.Unauthorized, "Invalid signature! Token could not be authorized!");
                }
            }
            catch
            {
                throw new AiurAPIModelException(ErrorType.Unauthorized, "Token was not in a valid format and can not be verified!");
            }
            return token.AppId;
        }
    }
}
