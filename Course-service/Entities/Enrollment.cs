using Infrastructure.Domains;
using System.ComponentModel.DataAnnotations;

namespace Course_service.Entities
{
    public class Enrollment : EntityBase<int>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public DateTime EnrolledDate { get; set; }
        public Course Course { get; set; }
    }
}
