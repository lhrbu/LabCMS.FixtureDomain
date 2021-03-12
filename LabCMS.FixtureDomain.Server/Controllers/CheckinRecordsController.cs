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
    public class CheckinRecordsController : ControllerBase
    {
        private readonly Repository _repository;
        private readonly IMapper _mapper;
        public CheckinRecordsController(
            Repository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [RolePayloadRequired]
        public IAsyncEnumerable<CheckinRecord> GetCheckinRecordsHistory()
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            return _repository.FixtureCheckinRecords.Where(item=>item.ApplicantUserId==rolePayload.UserId)
                .AsNoTracking().AsAsyncEnumerable();
        }

        [HttpGet("FixtureRoomApproverTodo")]
        [RolePayloadRequired]
        public IAsyncEnumerable<CheckinRecord> GetFixtureRoomApproverTodoAsync()
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            return _repository.FixtureCheckinRecords.Where(item =>
                item.Status == CheckRecordStatus.Initial).AsNoTracking()
                .Where(item => item.FixtureRoomApproverId == rolePayload.UserId).AsAsyncEnumerable();
        }

        [HttpPost("FixtureRoomApprove/{fixtureNo}")]
        [RolePayloadRequired]
        public async ValueTask<ActionResult<CheckinRecord>> FixtureRoomApproveAsync(int fixtureNo,
            [FromServices]IFixtureStorageRecordService fixtureStorageRecordService)
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            if (rolePayload.AuthLevel < 4) { return Unauthorized($"{rolePayload.UserId} AuthLevel is not allowed to do approve here"); }

            Fixture? fixture = await _repository.Fixtures.FindAsync(fixtureNo);
            CheckoutRecord? checkoutRecord = _repository.FixtureCheckoutRecords
                .Where(item => item.FixtureNo == fixtureNo)
                .FirstOrDefault(item => item.Status == CheckRecordStatus.FixtureRoomApproved);
            if (checkoutRecord == null || fixture == null)
            { return NotFound($"{fixtureNo} is not in valid status!"); }
            
            CheckinRecord checkinRecord = _mapper.Map<CheckinRecordPayload, CheckinRecord>(
                new CheckinRecordPayload { FixtureNo = fixtureNo, CheckoutRecordId = checkoutRecord.Id });
            checkinRecord.ApplicantUserId = checkoutRecord.ApplicantUserId;
            checkinRecord.Status = CheckRecordStatus.FixtureRoomApproved;
            checkinRecord.FixtureRoomApproverId = rolePayload.UserId;

            await _repository.AddAsync(checkinRecord);
            checkoutRecord.Status = CheckRecordStatus.End;
            fixtureStorageRecordService.Checkin(fixture,checkinRecord);
            await _repository.SaveChangesAsync();
            return Ok(checkinRecord);
        }

    }
}
