using AutoMapper;
using LabCMS.FixtureDomain.Server.Attributes;
using LabCMS.FixtureDomain.Server.Filters;
using LabCMS.FixtureDomain.Server.Policies;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Server.Services;
using LabCMS.FixtureDomain.Server.Models;
using LabCMS.Seedwork;
using LabCMS.Seedwork.FixtureDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Raccoon.Devkits.JwtAuthroization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(RolePayloadReadFilter))]
    [ServiceFilter(typeof(CheckRecordFindByIdFilter))]
    [ServiceFilter(typeof(PermissionPolicyValidateFilter))]
    [ServiceFilter(typeof(CheckRecordLogFilter))]
    public class CheckoutRecordsController : ControllerBase
    {
        private readonly Repository _repository;
        private readonly IMapper _mapper;
        public CheckoutRecordsController(
            Repository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [RolePayloadRequired]
        public IAsyncEnumerable<CheckoutRecord> GetCheckoutRecordsHistory()
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            var ddd = _repository.FixtureCheckoutRecords.AsNoTracking().ToArray();
            return _repository.FixtureCheckoutRecords.Where(item => item.ApplicantUserId == rolePayload.UserId)
                .Include(item => item.Fixture)
                .AsNoTracking().AsAsyncEnumerable();
        }

        [HttpGet("TestRoomApproverTodo")]
        [RolePayloadRequired]
        public async ValueTask<IEnumerable<CheckoutRecord>> GetTestRoomApproveTodoAsync()
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            Role role = await _repository.FixtureDomainRoles.FindAsync(rolePayload.UserId);
            return _repository.FixtureCheckoutRecords
                .Where(item => item.Status == CheckRecordStatus.Initial)
                .Include(item => item.Fixture)
                .Where(item => item.Fixture!.TestFieldName == role.ResponseTestFieldName)
                .AsNoTracking();
        }

        [HttpGet("TestRoomApproverTodoCount")]
        [RolePayloadRequired]
        public async ValueTask<int> GetTestRoomApproveTodoCountAsync() =>
            (await GetTestRoomApproveTodoAsync()).Count();

        [HttpGet("FixtureRoomApproverTodo")]
        [RolePayloadRequired]
        public IAsyncEnumerable<CheckoutRecord> GetFixtureRoomApproverTodoAsync()
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            return _repository.FixtureCheckoutRecords.Where(item =>
                item.Status == CheckRecordStatus.TestRoomApproved).AsNoTracking()
                .Where(item => item.FixtureRoomApproverId == rolePayload.UserId).AsAsyncEnumerable();
        }



        [HttpPost("Init")]
        [RolePayloadRequired]
        public async ValueTask<ActionResult<CheckoutRecord>> InitApplicationAsync(CheckoutRecordPayload checkoutRecordInClient)
        {
            Fixture? fixture = await _repository.Fixtures.FindAsync(checkoutRecordInClient.FixtureNo);
            if(fixture is null)
            { return NotFound($"Fixture {checkoutRecordInClient.FixtureNo} doesn't exist!");}

            CheckoutRecord initRecord = _mapper.Map<CheckoutRecordPayload, CheckoutRecord>(checkoutRecordInClient);
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            initRecord.ApplicantUserId = rolePayload.UserId;
            await _repository.FixtureCheckoutRecords.AddAsync(initRecord);
            await _repository.SaveChangesAsync();
            return initRecord;
        }


        [HttpDelete("{id}")]
        [RolePayloadRequired]
        [PermissionPolicy(typeof(ApplicantOnlyPolicy))]
        public async ValueTask<ActionResult<CheckoutRecord>> CancelApplicationAsync(int id)
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            CheckoutRecord checkoutRecord = (HttpContext.Items[nameof(CheckoutRecord)] as CheckoutRecord)!;
            if (checkoutRecord.Status != CheckRecordStatus.End)
            {
                checkoutRecord.Status = CheckRecordStatus.Cancelled;
                _repository.Update(checkoutRecord);
                await _repository.SaveChangesAsync();
                return checkoutRecord;
            }
            else { return Conflict($"Check out record {checkoutRecord.Id} can't be cancelled!"); }

        }

        [HttpPost("TestRoomApprove/{id}")]
        [RolePayloadRequired]
        [PermissionPolicy(typeof(TestFieldResponsibleAuthorizePolicy))]
        public async ValueTask<CheckoutRecord> TestRoomApproveAsync(int id)
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            CheckoutRecord checkoutRecord = (HttpContext.Items[nameof(CheckoutRecord)] as CheckoutRecord)!;

            checkoutRecord.Status = CheckRecordStatus.TestRoomApproved;
            checkoutRecord.TestRoomApproverUserId = rolePayload.UserId;
            _repository.Update(checkoutRecord);
            await _repository.SaveChangesAsync();
            return checkoutRecord;
        }

        [HttpPost("FixtureRoomApprove/{fixtureNo}")]
        [RolePayloadRequired]
        public async ValueTask<ActionResult<CheckoutRecord>> FixtureRoomApproveAsync(int fixtureNo,
            [FromServices]IFixtureStorageRecordService storageRecordService)
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            if (rolePayload.AuthLevel < 4) { return Unauthorized($"{rolePayload.UserId} AuthLevel is not allowed to do approve here");}
            
            Fixture? fixture = await _repository.Fixtures.FindAsync(fixtureNo);
            CheckoutRecord? checkoutRecord = _repository.FixtureCheckoutRecords.Where(item => item.FixtureNo == fixtureNo)
                .FirstOrDefault(item => item.Status == CheckRecordStatus.TestRoomApproved);
            if (checkoutRecord==null || fixture == null)
            { return NotFound($"{fixtureNo} is not in valid status!");}

            checkoutRecord.Status = CheckRecordStatus.FixtureRoomApproved;
            checkoutRecord.FixtureRoomApproverId = rolePayload.UserId;
            storageRecordService.Checkout(fixture,checkoutRecord);
            await _repository.SaveChangesAsync();
            return Ok(checkoutRecord);
        }
    }
}
