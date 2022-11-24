using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.UserDTOs
{
    public class UpdateUserDto
    {
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? LastName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? Phone { get; set; }
        public string UserName { get; set; }
        public float BalanceAccount { get; set; }
    }
}
