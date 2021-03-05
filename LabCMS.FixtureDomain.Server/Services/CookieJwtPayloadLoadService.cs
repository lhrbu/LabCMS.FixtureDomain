using Microsoft.AspNetCore.Http;
using Raccoon.Devkits.JwtAuthroization.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Services
{
    public class CookieJwtPayloadLoadService
    {
        private readonly JwtEncodeService _jwtEncodeService;
        public CookieJwtPayloadLoadService(
            JwtEncodeService jwtEncodeService)
        { 
            _jwtEncodeService = jwtEncodeService;
        }
        public TPayload Load<TPayload>(HttpContext httpContext,string cookieName,string secret)
        {
            var c = httpContext.Request.Cookies[cookieName];
            IDictionary<string,object?> dict = _jwtEncodeService.Decode(
                httpContext.Request.Cookies[cookieName]!, secret)!;
            byte[] jsonbytes = JsonSerializer.SerializeToUtf8Bytes(dict);
            return JsonSerializer.Deserialize<TPayload>(jsonbytes.AsSpan())!;
        }
    }
}
