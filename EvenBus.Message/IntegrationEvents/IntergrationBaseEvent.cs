using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Message.IntegrationEvents
{
    public record IntergrationBaseEvent : IIntergrationEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();

        public DateTime CreationDate { get; } = DateTime.Now;
    }
}
