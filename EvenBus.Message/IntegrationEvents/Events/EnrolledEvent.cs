using EvenBus.Message.IntegrationEvents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Message.IntegrationEvents.Events
{
    public record EnrolledEvent : IntergrationBaseEvent, IEnrolledEvent
    {
        public int Id { get; set; }
        public string FirstName { get ; set ; }
        public string? LastName { get ; set ; }
        public string Email { get ; set ; }
        public string? Phone { get ; set ; }
        public string UserName { get ; set ; }
        public float BalanceAccount { get; set; }
    }
}
