using System;
using System.Collections.Generic;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DetailPage.Common
{
    public class ValidationFilter : ActionFilterAttribute
    {
//        private readonly IConfiguration _configuration;
//        public ValidationFilter(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            #region 验证

            var jsonNetSerializer = new JsonNetSerializer();

            
            var token = request.Cookies["auth_token"] ?? request.Headers["HeaderAuthorization"][0];
            if (string.IsNullOrWhiteSpace(token))
            {
//                var msg = new Dictionary<string, object>
//                {
//                    {"isSuccess", false},
//                    {"message", "没有权限"}
//                };
//
//                context.HttpContext.Response.WriteAsync(jsonNetSerializer.Serialize(msg));
                context.HttpContext.Response.Redirect("/login");
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var secret = configuration.GetSection("jwt")["secret"];
            var json = new JwtBuilder().WithSecret(secret).MustVerifySignature()
                .Decode<Dictionary<string, object>>(token);
            //验证是否token是否已经过期
            var exp = long.Parse(json["exp"].ToString());
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddMilliseconds(exp);
            var now = DateTimeOffset.Now;
            if (now > dt)
            {
//                var msg = new Dictionary<string, object>
//                {
//                    {"isSuccess", false},
//                    {"message", "请重新登录"}
//                };
//
//                context.HttpContext.Response.WriteAsync(jsonNetSerializer.Serialize(msg));
                context.HttpContext.Response.Redirect("/login");
                return;
            }

            //TOOD:验证用户是否存在
            base.OnActionExecuting(context);

            #endregion
        }
    }
}