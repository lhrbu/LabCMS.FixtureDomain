using System.Reflection;
using System.Text.RegularExpressions;
using LabCMS.FixtureDomain.Server.EventHandles;
using LabCMS.FixtureDomain.Server.Extensions;
using LabCMS.FixtureDomain.Server.Filters;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace LabCMS.FixtureDomain.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FixturesController : ControllerBase
{
    private readonly FixtureDomainRepository _repository;
    private readonly FixtureEventHandlersFactory _handlersFactory;
    private readonly IConfiguration _configuration;
    private readonly static Assembly _fixtureEventAssembly = typeof(FixtureEventHandler).Assembly;
    public FixturesController(
        FixtureDomainRepository repository,
        FixtureEventHandlersFactory handlersFactory,
        IConfiguration configuration)
    {
        _repository = repository;
        _handlersFactory = handlersFactory;
        _configuration = configuration;
    }

    [HttpPost(nameof(Register))]
    public async ValueTask<ActionResult<int>> Register(FixtureRegisterPayload registerPayload)
    {
        int year2Suffix = DateTime.Now.ToLocalTime().Year - 2000;
        IEnumerable<int> sameTestFieldFixtureNos = _repository.Fixtures.Select(item => item.No).AsEnumerable()
            .Select(item => item.ToString())
            .Where(item => Regex.IsMatch(item, "^[0-9]{8}$"))
            .Where(item => int.Parse(item.Substring(2, 2)) == year2Suffix)
            .Select(item => int.Parse(item.Substring(4, 4)));

        int fixtureIndex = sameTestFieldFixtureNos.Any() ?
            sameTestFieldFixtureNos.Max() + 1 :
            1;
        string fixturePrefix = registerPayload.TestField switch
        {
            TestField.Vibration => "10",
            TestField.Environment => "20",
            TestField.Photometric => "30",
            TestField.WaterSpray => "40",
            _ => "50"
        };
        int fixtureNo = int.Parse($"{fixturePrefix}{year2Suffix}{fixtureIndex.ToString("D4")}");
        Fixture fixture = new(fixtureNo, registerPayload.ProjectShortName,
            registerPayload.TestField, registerPayload.SetIndex);
        fixture.Status = FixtureStatus.Registered;
        await _repository.AddAsync(fixture);
        await _repository.SaveChangesAsync();
        return Ok(fixtureNo);
    }

    private async ValueTask<IActionResult> HandleAsync(FixtureEvent fixtureEvent)
    {
        string eventHandlerTypeName = $"{fixtureEvent.GetType().Name}Handler";
        string eventHandlerTypeFullName = $"LabCMS.FixtureDomain.Server.EventHandles.{eventHandlerTypeName}";
        Type eventHandlerType = _fixtureEventAssembly.GetType(eventHandlerTypeFullName)!;
        FixtureEventHandler eventHandler = _handlersFactory.Create(eventHandlerType, _repository);
        await eventHandler.HandleAsync(fixtureEvent);
        return Ok();
    }

    [HttpPost(nameof(AcceptanceCheck))]
    public async ValueTask<IActionResult> AcceptanceCheck(FixtureAcceptanceCheckEvent fixtureEvent)
        => await HandleAsync(fixtureEvent);

    [HttpPost(nameof(AssignLocationNo))]
    public async ValueTask<IActionResult> AssignLocationNo(FixtureAssignLocationNoEvent fixtureEvent)
        => await HandleAsync(fixtureEvent);

    [HttpPost(nameof(MoveInOutOfFixtureRoom))]
    public async ValueTask<IActionResult> MoveInOutOfFixtureRoom(FixtureMoveInOutOfFixtureRoomEvent fixtureEvent)
        => await HandleAsync(fixtureEvent);

    [HttpPost(nameof(InternalCheckoutApply))]
    [ServiceFilter(typeof(DecodeRoleFromCookieFilter))]
    public async ValueTask<IActionResult> InternalCheckoutApply(FixtureInternalCheckoutApplyEvent fixtureEvent)
    {
        fixtureEvent.ApplicantUserId = HttpContext.GetRole().UserId;
        return await HandleAsync(fixtureEvent);
    }

    [HttpPost(nameof(ExternalCheckoutApply))]
    [ServiceFilter(typeof(DecodeRoleFromCookieFilter))]
    public async ValueTask<IActionResult> ExternalCheckoutApply(FixtureExternalCheckoutApplyEvent fixtureEvent)
    {
        fixtureEvent.ApplicantUserId = HttpContext.GetRole().UserId;
        return await HandleAsync(fixtureEvent);
    }

    [HttpPost(nameof(ExternalCheckoutApprove))]
    [ServiceFilter(typeof(DecodeRoleFromCookieFilter))]
    public async ValueTask<IActionResult> ExternalCheckoutApprove(FixtureExternalCheckoutApproveEvent fixtureEvent)
    {
        string[] adminUsersId = _configuration.GetValue<string[]>("AdminUsersId");
        string userId = HttpContext.GetRole().UserId;
        if (adminUsersId.Contains(userId))
        {
            fixtureEvent.ApproverUserId = HttpContext.GetRole().UserId;
            return await HandleAsync(fixtureEvent);
        }
        else { return Unauthorized(); }
    }

    [HttpPost(nameof(CheckoutScan))]
    [ServiceFilter(typeof(DecodeRoleFromCookieFilter))]
    public async ValueTask<IActionResult> CheckoutScan(FixtureCheckoutScanEvent fixtureEvent)
        => HttpContext.GetRole().UserId == "scanner" ?
            await HandleAsync(fixtureEvent) :
            Unauthorized();

    [HttpPost(nameof(CheckinScan))]
    [ServiceFilter(typeof(DecodeRoleFromCookieFilter))]
    public async ValueTask<IActionResult> CheckinScan(FixtureCheckinScanEvent fixtureEvent)
        => HttpContext.GetRole().UserId == "scanner" ?
            await HandleAsync(fixtureEvent) :
            Unauthorized();

}
