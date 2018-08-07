﻿using System;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Aiursoft.Pylon.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Html;

namespace Aiursoft.Pylon
{
    public static class ViewExtends
    {
        private static IHtmlContentBuilder AppendJavaScript(this IHtmlContentBuilder content, string path)
        {
            return content.AppendHtmlLine($"<script src='{path}'></script>");
        }

        private static IHtmlContentBuilder AppendStyleSheet(this IHtmlContentBuilder content, string path)
        {
            return content.AppendHtmlLine($"<link href='{path}' rel='stylesheet' />");
        }

        public static IHtmlContent UseAiurLogoutter(this RazorPage page)
        {
            var template = @"<div class='modal fade' id='exampleModal' tabindex='-1' role='dialog' aria-labelledby='exampleModalLabel' aria-hidden='true'>
                                <div class='modal-dialog' role='document'>
                                    <div class='modal-content'>
                                        <div class='modal-header'>
                                            <h5 class='modal-title' id='exampleModalLabel'>Ready to Leave?</h5>
                                            <button class='close' type='button' data-dismiss='modal' aria-label='Close'>
                                                <span aria-hidden='true'>×</span>
                                            </button>
                                        </div>
                                        <div class='modal-body'>Select 'Logout' below if you are ready to end your current session.</div>
                                        <div class='modal-footer'>
                                            <button class='btn btn-secondary' type='button' data-dismiss='modal'>Cancel</button>
                                            <a class='btn btn-primary' href='javascript:$('#logoutForm').submit()'>Logout</a>
                                        </div>
                                        <form class='hidden' action='/Home/Logoff' method='post' id='logoutForm'></form>
                                    </div>
                                </div>
                            </div>";
            return new HtmlContentBuilder()
                .SetHtmlContent(template);
        }

        public static IHtmlContent UseAiurFooter()
        {
            throw new NotImplementedException();
        }

        public static IHtmlContent UseChinaRegisterInfo(this RazorPage page)
        {
            var content = new HtmlContentBuilder();
            var requestCultureFeature = page.Context.Features.Get<IRequestCultureFeature>();
            if (requestCultureFeature == null)
            {
                return content;
            }
            var requestCulture = requestCultureFeature.RequestCulture.UICulture.IetfLanguageTag;
            if (requestCulture == "zh")
            {
                content.SetHtmlContent("<a href='http://www.miitbeian.gov.cn' target='_blank'>辽ICP备17004979号-1</a>");
            }
            return content;
        }

        public static IHtmlContent UseScrollToTop(this RazorPage page)
        {
            var content = new HtmlContentBuilder();
            content.SetHtmlContent("<a class='aiur-scroll-to-top rounded' href='#page-top'><i class='fa fa-angle-up'></i></a>");
            return content;
        }

        public static IHtmlContent UseAiurDashboardCSS(this RazorPage page)
        {
            var serviceLocation = page.Context.RequestServices.GetService<ServiceLocation>();
            return new HtmlContentBuilder()
                .AppendStyleSheet($"{serviceLocation.CDN}/dist/AiurCore.min.css")
                .AppendStyleSheet($"{serviceLocation.CDN}/dist/AiurDashboard.min.css");
        }

        public static IHtmlContent UseAiurDashboardJs(this RazorPage page)
        {
            var serviceLocation = page.Context.RequestServices.GetService<ServiceLocation>();
            return new HtmlContentBuilder()
                .AppendJavaScript($"{serviceLocation.CDN}/dist/AiurCore.min.js")
                .AppendJavaScript($"{serviceLocation.CDN}/dist/AiurDashboard.min.js");
        }

        public static IHtmlContent UseAiurMarketCSS(this RazorPage page)
        {
            var serviceLocation = page.Context.RequestServices.GetService<ServiceLocation>();
            return new HtmlContentBuilder()
                .AppendStyleSheet($"{serviceLocation.CDN}/dist/AiurCore.min.css")
                .AppendStyleSheet($"{serviceLocation.CDN}/dist/AiurMarket.min.css");
        }

        public static IHtmlContent UseAiurMarketJs(this RazorPage page)
        {
            var serviceLocation = page.Context.RequestServices.GetService<ServiceLocation>();
            return new HtmlContentBuilder()
                .AppendJavaScript($"{serviceLocation.CDN}/dist/AiurCore.min.js")
                .AppendJavaScript($"{serviceLocation.CDN}/dist/AiurMarket.min.js");
        }

        public static IHtmlContent UseAiurFavicon(this RazorPage page)
        {
            var serviceLocation = page.Context.RequestServices.GetService<ServiceLocation>();
            return new HtmlContentBuilder()
                .SetHtmlContent($"<link rel='icon' type='image/x-icon' href='{serviceLocation.CDN}/favicon.ico'>");
        }
    }
}
