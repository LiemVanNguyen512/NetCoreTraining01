using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Message.IntegrationEvents
{
    public interface IIntergrationEvent
    {
        Guid EventId { get; }
        DateTime CreationDate { get; }
    }
}
