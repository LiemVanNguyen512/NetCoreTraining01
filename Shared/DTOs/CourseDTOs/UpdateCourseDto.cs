using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CourseDTOs
{
    public class UpdateCourseDto
    {
        [Required]
        [Column(TypeName = "nvarchar(45)")]
        public string Code { get; set; }
        [Required]
        public float Price { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? Description { get; set; }
    }
}
