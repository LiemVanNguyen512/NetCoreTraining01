using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.UserDTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string UserName { get; set; }
        public float BalanceAccount { get; set; }
    }
}
