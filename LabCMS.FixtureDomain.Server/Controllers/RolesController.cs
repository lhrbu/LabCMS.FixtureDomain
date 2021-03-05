using AutoMapper;
using LabCMS.FixtureDomain.Server.Attributes;
using LabCMS.FixtureDomain.Server.Filters;
using LabCMS.FixtureDomain.Server.Policies;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Server.Services;
using LabCMS.FixtureDomain.Shared.ClientSideModels;
using LabCMS.Seedwork;
using LabCMS.Seedwork.FixtureDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raccoon.Devkits.JwtAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(RolePayloadLoadFilter))]
    [ServiceFilter(typeof(PermissionPolicyValidateFilter))]
    public class RolesController : ControllerBase
    {
        private readonly Repository _repository;
        private readonly IMapper _mapper;
        private readonly RandomPasswordGenerator _passwordGenerator;
        public RolesController(Repository repository,
            IMapper mapper,
            RandomPasswordGenerator passwordGenerator)
        { 
            _repository = repository; 
            _mapper = mapper;
            _passwordGenerator = passwordGenerator;
        }
        
        [HttpGet]
        public IAsyncEnumerable<Role> GetAllAsync() => _repository.FixtureDomainRoles.AsAsyncEnumerable();

        [HttpPost("ResetPassword")]
        [RolePayloadRequired]
        [PermissionPolicy(typeof(ApplicantOnlyPolicy))]

        public async ValueTask<IActionResult> ResetPasswordAsync()
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            Role? role = await _repository.FixtureDomainRoles.FindAsync(rolePayload.UserId);
            if(role is null) { return NotFound(); }
            await _repository.Entry(role).Reference(item => item.Person).LoadAsync();
            string password = _passwordGenerator.Generate();
            role.Person!.PasswordMD5MD5 = password;
            return Ok();
            //throw new NotImplementedException("Send reset email here");
        }
        private string MD5Encode(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] passwordMD5MD5bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder stringBuilder = new();
            foreach (byte bt in passwordMD5MD5bytes)
            { stringBuilder.Append(bt.ToString("x2"));}
            return stringBuilder.ToString();
        }

        [HttpPost]
        public async ValueTask<IActionResult> SignInCheckAsync([FromQuery]string userId,[FromQuery]string passwordMD5)
        {
            Role? role = await _repository.FixtureDomainRoles.FindAsync(userId);
            if(role is null) { return Unauthorized(); }
            await _repository.Entry(role).Reference(item => item.Person).LoadAsync();
            string passwordMD5MD5 = MD5Encode(passwordMD5);
            if (role.Person!.PasswordMD5MD5 == passwordMD5MD5)
            {
                RolePayload rolePayload = _mapper.Map<Role,RolePayload>(role);
                HttpContext.WriteJwtPayload(nameof(RolePayload),rolePayload);
                return Ok();
            }else{return Unauthorized();}
        }

        [HttpDelete]
        public IActionResult SignOutDelete()
        {
            HttpContext.Response.Cookies.Delete(nameof(RolePayload));
            return Ok();
        }
        
    }
}
