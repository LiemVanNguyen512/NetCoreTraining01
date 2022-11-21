using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contact.API.Entities
{
    public class CatalogContact
    {
        [Key]
        public int Id { get; set; }
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
