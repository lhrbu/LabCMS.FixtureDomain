using LabCMS.FixtureDomain.Shared.Events;
namespace LabCMS.FixtureDomain.Server.EventHandles;
public interface IEventHandler
{
    ValueTask HandleAsync(EventBase eventBase);
}
