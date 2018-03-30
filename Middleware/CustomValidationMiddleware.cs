using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Schema;

namespace DetailPage.Middleware
{
    public class CustomValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 通过依赖注入
        /// </summary>
        /// <param name="next"></param>
        /// <param name="configuration"></param>
        public CustomValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task Invoke(HttpContext context)
        {
            #region 验证

            var jsonNetSerializer = new JsonNetSerializer();
            
            if (!context.Request.Path.StartsWithSegments("/login"))
            {
                var token = context.Request.Cookies["auth_token"];
                if (string.IsNullOrWhiteSpace(token))
                {
                    var msg = new Dictionary<string, object>
                    {
                        {"isSuccess", false},
                        {"message", "没有权限"}
                    };

                    await context.Response.WriteAsync(jsonNetSerializer.Serialize(msg));
                }

                var secret = _configuration.GetSection("jwt")["secret"];
                var json = new JwtBuilder().WithSecret(secret).MustVerifySignature()
                    .Decode<Dictionary<string, object>>(token);
                //验证是否token是否已经过期
                var exp = long.Parse(json["exp"].ToString());
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
                DateTime dt = startTime.AddMilliseconds(exp);
                var now = DateTimeOffset.Now;
                if (now > dt)
                {
                    var msg = new Dictionary<string, object>
                    {
                        {"isSuccess", false},
                        {"message", "请重新登录"}
                    };

                    await context.Response.WriteAsync(jsonNetSerializer.Serialize(msg));
                }
                //TOOD:验证用户是否存在
            }


            #endregion

            await _next(context);
        }
    }
}