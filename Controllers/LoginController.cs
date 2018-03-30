using System;
using System.Linq;
using DetailPage.Models;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DetailPage.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        //使用动态常量
        private readonly DetailPageContext _detailPageContext;
        private readonly IConfiguration _configuration;

        public LoginController(DetailPageContext detailPageContext, IConfiguration configuration)
        {
            _detailPageContext = detailPageContext;
            _configuration = configuration;
        }

        [HttpPost]
//        [Consumes("application/json")] 
        public JsonResult Post([FromBody] UserModel user)
        {
            //验证用户是否正确
            if (user == null)
            {
                return Json(new {IsSuccess = false, Message = "传入的参数不对"});
            }

            var result =
                _detailPageContext.Users.FirstOrDefault(p =>
                    p.UserName == user.UserName && p.Password == user.Password);
            if (result == null)
            {
                return Json(new {IsSuccess = false, Message = "用户名或密码不正确"});
            }

            //获取token
            var secret = _configuration.GetSection("jwt")["secret"];
//            var exp = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();
            var startDate = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var timeStamp = (long)(DateTime.Now.AddHours(1) - startDate).TotalMilliseconds; // 相差毫秒
            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(secret)
                .AddClaim("exp", timeStamp)
                .AddClaim("userName", user.UserName)
                .Build();
            return Json(new {IsSuccess = true, Message = "登录成功", Token = token, Exp = timeStamp});
        }
    }
}