using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenBus.Message.IntegrationEvents.Interfaces
{
    public interface IEnrolledEvent : IIntergrationEvent
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string? LastName { get; set; }
        string Email { get; set; }
        string? Phone { get; set; }
        string UserName { get; set; }
        float BalanceAccount { get; set; }
    }
}
