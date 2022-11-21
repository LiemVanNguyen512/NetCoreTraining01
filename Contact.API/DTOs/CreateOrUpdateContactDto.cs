using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Contact.API.DTOs
{
    public class CreateOrUpdateContactDto
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
    }
}
