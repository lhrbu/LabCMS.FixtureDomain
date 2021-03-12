using AutoMapper;
using LabCMS.FixtureDomain.Server.Attributes;
using LabCMS.FixtureDomain.Server.Filters;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Server.Services;
using LabCMS.FixtureDomain.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LabCMS.Seedwork;
using LabCMS.Seedwork.FixtureDomain;

namespace LabCMS.FixtureDomain.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(RolePayloadReadFilter))]
    public class FixturesController:ControllerBase
    {
        private readonly ILogger<FixturesController> _logger;
        private readonly Repository _repository;
        private readonly IFixtureIndexGenerator _fixtureIndexGenerator;
        private readonly IFixtureNoGenerator _fixtureNoGenerator;
        public FixturesController(
            Repository repository,
            ILogger<FixturesController> logger,
            IFixtureIndexGenerator fixtureIndexGenerator,
            IFixtureNoGenerator fixtureNoGenerator)
        { 
            _repository = repository;
            _logger = logger;
            _fixtureIndexGenerator = fixtureIndexGenerator;
            _fixtureNoGenerator = fixtureNoGenerator;
        }
        [HttpGet]
        public IAsyncEnumerable<Fixture> GetAllAsync()=>_repository.Fixtures
            .AsNoTracking().AsAsyncEnumerable();
        [HttpGet("InFixtureRoom")]
        public IAsyncEnumerable<Fixture> GetAllInFixtureRoomAsync()=>_repository.Fixtures
            .Where(item=>item.InFixtureRoom).AsAsyncEnumerable();
       
        [HttpPost("{year}")]
        [RolePayloadRequired]
        public async ValueTask<IActionResult> PostAsync(int year,[FromBody]Fixture fixture)
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            if (rolePayload.AuthLevel < 4) { return Unauthorized(); }

            int index = await _fixtureIndexGenerator.GetThenIncreaseAsync(year);
            fixture.No = _fixtureNoGenerator.Create(index, fixture);

            await _repository.Fixtures.AddAsync(fixture);
            await _repository.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [RolePayloadRequired]
        public async ValueTask<IActionResult> PutAsync(Fixture fixture)
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            if (rolePayload.AuthLevel < 4) { return Unauthorized(); }
            _repository.Fixtures.Update(fixture);
            await _repository.SaveChangesAsync();
            _logger.LogInformation("Update: {@Fixture}",fixture);
            return Ok();
        }

        [HttpDelete("{id}")]
        [RolePayloadRequired]
        public async ValueTask<IActionResult> DeleteByIdAsync(int id)
        {
            RolePayload rolePayload = (HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
            if (rolePayload.AuthLevel < 4) { return Unauthorized(); }
            Fixture? fixture = await _repository.Fixtures.FindAsync(id);
            if(fixture is not null)
            {
                _repository.Fixtures.Remove(fixture);
                await _repository.SaveChangesAsync();
                _logger.LogWarning("Delete {@Fixture}",fixture);
                return Ok();
            }
            else { return NotFound(); }
        }
    }
}