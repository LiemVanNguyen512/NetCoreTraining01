using Infrastructure.Domains;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Course_service.Entities
{
    public class Course : EntityBase<int>
    {
        [Required]
        [Column(TypeName = "nvarchar(45)")]
        public string Code { get; set; }
        [Required]
        public float Price { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? Description { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
