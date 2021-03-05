using AutoMapper;
using LabCMS.FixtureDomain.Server.Attributes;
using LabCMS.FixtureDomain.Server.Filters;
using LabCMS.FixtureDomain.Server.Policies;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Server.Services;
using LabCMS.FixtureDomain.Shared;
using LabCMS.FixtureDomain.Shared.ClientSideModels;
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
    [ServiceFilter(typeof(RolePayloadLoadFilter))]
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
        public async ValueTask<ActionResult<CheckinRecord>> FixtureRoomApproveAsync(int fixtureNo)
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            if (rolePayload.AuthLevel < 4) { return Unauthorized($"{rolePayload.UserId} AuthLevel is not allowed to do approve here"); }

            CheckoutRecord? checkoutRecord = _repository.FixtureCheckoutRecords.Where(item => item.FixtureNo == fixtureNo)
                 .FirstOrDefault(item => item.Status == CheckRecordStatus.FixtureRoomApproved);
            if (checkoutRecord is null)
            { return NotFound($"{fixtureNo} is not in valid status!"); }


            CheckinRecord checkinRecord = _mapper.Map<CheckinRecordPayload, CheckinRecord>(
                new CheckinRecordPayload { FixtureNo = fixtureNo, CheckoutRecordId = checkoutRecord.Id });
            checkinRecord.ApplicantUserId = checkoutRecord.ApplicantUserId;
            checkinRecord.Status = CheckRecordStatus.FixtureRoomApproved;
            checkinRecord.FixtureRoomApproverId = rolePayload.UserId;

            await _repository.AddAsync(checkinRecord);
            checkoutRecord.Status = CheckRecordStatus.End;
            await _repository.SaveChangesAsync();
            return Ok(checkinRecord);
        }
        #region cancelled web api   

        //[HttpPost("Init")]
        //[RolePayloadRequired]
        //public async ValueTask<ActionResult<CheckinRecord>> InitApplicationAsync(CheckinRecordPayload checkinRecordInClient)
        //{
        //    CheckoutRecord? checkoutRecord = await _repository.FixtureCheckoutRecords.FindAsync(checkinRecordInClient.CheckoutRecordId);
        //    if (checkoutRecord?.Status is CheckRecordStatus.End)
        //    {
        //        CheckinRecord initRecord = _mapper.Map<CheckinRecordPayload, CheckinRecord>(checkinRecordInClient);
        //        RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
        //        initRecord.ApplicantUserId = rolePayload.UserId;
        //        await _repository.FixtureCheckinRecords.AddAsync(initRecord);
        //        await _repository.SaveChangesAsync();
        //        return initRecord;
        //    }
        //    else
        //    {
        //        return BadRequest($"Check out record {checkinRecordInClient.CheckoutRecordId} is not finished.");
        //    }
        //}

        //[HttpDelete("{id}")]
        //[RolePayloadRequired]
        //[PermissionPolicy(typeof(ApplicantOnlyPolicy))]
        //public async ValueTask<ActionResult<CheckinRecord>> CancelApplicationAsync(int id)
        //{
        //    RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
        //    CheckinRecord checkinRecord = (HttpContext.Items[id] as CheckinRecord)!;
        //    if (checkinRecord.Status != CheckRecordStatus.End)
        //    {
        //        checkinRecord.Status = CheckRecordStatus.Cancelled;
        //        _repository.Update(checkinRecord);
        //        await _repository.SaveChangesAsync();
        //        return checkinRecord;
        //    }
        //    else
        //    {
        //        return Conflict($"Check in record {checkinRecord.Id} is in end status, so that it can't be cancelled!");
        //    }
        //}

        //[HttpPost("FixtureRoomApprove/{id}")]
        //[RolePayloadRequired]
        //[PermissionPolicy(typeof(FixtureRoomAuthorizePolicy))]
        //public async ValueTask<CheckinRecord> FixtureRoomApproveAsync(int id)
        //{
        //    CheckinRecord checkinRecord = (HttpContext.Items[id] as CheckinRecord)!;
        //    RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;

        //    checkinRecord.Status = CheckRecordStatus.FixtureRoomApproved;
        //    checkinRecord.FixtureRoomApproverId = rolePayload.UserId;

        //    _repository.Update(checkinRecord);
        //    await _repository.SaveChangesAsync();
        //    return checkinRecord;
        //}


        //[HttpPost("End/{id}")]
        //[RolePayloadRequired]
        //[PermissionPolicy(typeof(ScannerOnlyPolicy))]

        //public async ValueTask<CheckinRecord> EndAsync(int id)
        //{
        //    CheckinRecord checkinRecord = (HttpContext.Items[id] as CheckinRecord)!;
        //    checkinRecord.Status = CheckRecordStatus.End;
        //    checkinRecord.CheckinDate = DateTimeOffset.Now;
        //    _repository.Update(checkinRecord);
        //    await _repository.Entry(checkinRecord).Reference(item=>item.Fixture).LoadAsync();
        //    checkinRecord.Fixture!.StorageInformation = "Fixture Room";
        //    await _repository.SaveChangesAsync();
        //    return checkinRecord;
        //}
        #endregion



    }
}
