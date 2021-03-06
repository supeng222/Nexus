﻿using Aiursoft.Gateway.Controllers;
using Aiursoft.Gateway.Data;
using Aiursoft.Gateway.Models;
using Aiursoft.Gateway.SDK.Models;
using Aiursoft.Gateway.SDK.Models.ForApps.AddressModels;
using Aiursoft.Scanner.Interfaces;
using Aiursoft.XelNaga.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Aiursoft.Gateway.Services
{
    public class UserAppAuthManager : IScopedDependency
    {
        private readonly GatewayDbContext _dbContext;

        public UserAppAuthManager(GatewayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> FinishAuth(GatewayUser user, FinishAuthInfo model, bool forceGrant, bool trusted)
        {
            var authorized = await HasAuthorizedApp(user, model.AppId);
            if (!authorized && trusted)
            {
                // Unauthorized. But viewing a trusted app. Just auto auth him.
                await GrantTargetApp(user, model.AppId);
                authorized = true;
            }
            if (authorized && forceGrant != true)
            {
                // Dont need to auth, and the user don't force to auth.
                var pack = await GeneratePack(user, model.AppId);
                var url = new AiurUrl(GetRegexRedirectUri(model.RedirectUri), new AuthResultAddressModel
                {
                    Code = pack.Code,
                    State = model.State
                });
                return new RedirectResult(url.ToString());
            }
            else
            {
                // Need to do the auth logic.
                var url = new AiurUrl(string.Empty, "OAuth", nameof(OAuthController.AuthorizeConfirm), new FinishAuthInfo
                {
                    AppId = model.AppId,
                    RedirectUri = model.RedirectUri,
                    State = model.State
                });
                return new RedirectResult(url.ToString());
            }
        }

        public async Task GrantTargetApp(GatewayUser user, string appId)
        {
            if (!await HasAuthorizedApp(user, appId))
            {
                var appGrant = new AppGrant
                {
                    AppID = appId,
                    GatewayUserId = user.Id
                };
                _dbContext.LocalAppGrant.Add(appGrant);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<OAuthPack> GeneratePack(GatewayUser user, string appId)
        {
            var pack = new OAuthPack
            {
                Code = Math.Abs(Guid.NewGuid().GetHashCode()),
                UserId = user.Id,
                ApplyAppId = appId
            };
            _dbContext.OAuthPack.Add(pack);
            await _dbContext.SaveChangesAsync();
            return pack;
        }

        public Task<bool> HasAuthorizedApp(GatewayUser user, string appId)
        {
            return _dbContext
                .LocalAppGrant
                .Where(t => t.AppID == appId)
                .AnyAsync(t => t.GatewayUserId == user.Id);
        }

        private string GetRegexRedirectUri(string sourceUrl)
        {
            var url = new Uri(sourceUrl);
            return $@"{url.Scheme}://{url.Host}:{url.Port}{url.AbsolutePath}";
        }
    }
}
