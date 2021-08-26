
using LabCMS.FixtureDomain.Shared.Events;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class AcceptanceCheckEventHandler : IEventHandler
{
    public async ValueTask HandleAsync(EventBase eventBase)
    {
        if(eventBase is AcceptanceCheckEvent acceptanceCheckEvent)
        {

        }
        else { throw new ArgumentException("Wrong type of event given.",nameof(eventBase)); }
    }
}
