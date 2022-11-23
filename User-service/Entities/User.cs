using Infrastructure.Domains;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User_service.Entities
{
    public class User : EntityBase<int>
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
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string UserName { get; set; }
        [DefaultValue(0)]
        public float BalanceAccount { get; set; }
    }
}
