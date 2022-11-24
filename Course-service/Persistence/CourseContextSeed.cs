using Course_service.Entities;
using ILogger = Serilog.ILogger;

namespace Course_service.Persistence
{
    public class CourseContextSeed
    {
        public static async Task SeedCouresAsync(CourseContext contactContext, ILogger logger)
        {
            if (!contactContext.Courses.Any())
            {
                contactContext.AddRange(getCourses());
                await contactContext.SaveChangesAsync();
                logger.Information("Seeded data for Course DB associated with context {DbContextName}",
                    nameof(CourseContext));
            }
        }

        private static IEnumerable<Course> getCourses()
        {
            return new List<Course>
        {
            new()
            {
                Code = "IIS319",
                Price = 2900000,
                Description = "TOEIC Course",
                Enrollments= new List<Enrollment>
                {
                    new() 
                    {
                        UserId = 1,
                        EnrolledDate= DateTime.Now.AddDays(-1),
                    },
                    new()
                    {
                        UserId = 2,
                        EnrolledDate= DateTime.Now
                    }
                }
            }
        };
        }
    }
}
