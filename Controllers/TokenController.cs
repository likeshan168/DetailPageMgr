﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using DetailPage.Common;
using DetailPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Zeeko.UtilsPack;

namespace DetailPage.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly JWTTokenOptions _tokenOptions;
        private readonly DetailPageContext _dbContext;

        public TokenController(JWTTokenOptions tokenOptions, DetailPageContext dbContext)
        {
            _tokenOptions = tokenOptions;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 生成一个新的 Token
        /// </summary>
        /// <param name="user">用户信息实体</param>
        /// <param name="expire">token 过期时间</param>
        /// <param name="audience">Token 接收者</param>
        /// <returns></returns>
        private string CreateToken(UserModel user, DateTime expire, string audience)
        {
            var handler = new JwtSecurityTokenHandler();
            string jti = audience + user.UserName + expire.GetMilliseconds();
            jti = jti.GetMd5();
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString(), ClaimValueTypes.Integer32),
                new Claim("jti",jti,ClaimValueTypes.String)
            };
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user.UserName, "TokenAuth"), claims);
            var token = handler.CreateEncodedJwt(new SecurityTokenDescriptor
            {
                Issuer = "TestIssuer",
                Audience = audience,
                SigningCredentials = _tokenOptions.Credentials,
                Subject = identity,
                Expires = expire
            });
            return token;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user">用户登录信息</param>
        /// <param name="audience">要访问的网站</param>
        /// <returns></returns>
        [HttpPost("{audience}")]
        public IActionResult Post([FromBody]UserModel user, string audience)
        {
            DateTime expire = DateTime.Now.AddDays(7);

            var result =
                _dbContext.Users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
            if (result == null)
            {
                return Json(new { Error = "用户名或密码错误" });
            }
            return Json(new { Token = CreateToken(result, expire, audience) });
        }
    }
}