using Shared.DTOs.CourseDTOs;
using Shared.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.EnrollmentDTOs
{
    public class EnrollmentDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public DateTime EnrolledDate { get; set; }
        public CourseDto Course { get; set; }
        public UserDto Member { get; set; }
    }
}
