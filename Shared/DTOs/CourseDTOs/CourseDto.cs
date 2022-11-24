using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CourseDTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }
    }
}
