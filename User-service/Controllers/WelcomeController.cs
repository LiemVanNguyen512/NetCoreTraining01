using Infrastructure.ScheduledJobs.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace User_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WelcomeController : ControllerBase
    {
        private readonly IScheduledJobService _jobService;
        private readonly ILogger _logger;

        public WelcomeController(IScheduledJobService jobService, ILogger logger)
        {
            _jobService = jobService;
            _logger = logger;
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult Welcome()
        {
            var jobId = _jobService.Enqueue(() => ResponseWelcome("Welcome to Hangfire API - Enqueue"));
            return Ok($"Job ID: {jobId} - Enqueue Job");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DelayedWelcome()
        {
            var seconds = 5;
            var jobId = _jobService.Schedule(() => ResponseWelcome($"Welcome to Hangfire API - Schedule delay {seconds}s"),
               TimeSpan.FromSeconds(seconds));
            return Ok($"Job ID: {jobId} - Delayed Job {seconds}s");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult WelcomeAt()
        {
            var enqueueAt = DateTimeOffset.Now.AddSeconds(10);
            var jobId = _jobService.Schedule(() => ResponseWelcome($"Welcome to Hangfire API - Schedule at {enqueueAt}"),
               enqueueAt);
            return Ok($"Job ID: {jobId} - Schedule Job at {enqueueAt}");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult ConfirmedWelcome()
        {
            const int timeInSeconds = 5;
            var parentJobId =
               _jobService.Schedule(() => ResponseWelcome($"Welcome to Hangfire API - Schedule delay {timeInSeconds}s"), TimeSpan.FromSeconds(5));

            var jobId = _jobService.ContinueQueueWith(parentJobId,
               () => ResponseWelcome("Welcome message is sent"));

            return Ok($"Job ID: {jobId} - Confirmed Welcome will be sent in {timeInSeconds} seconds");
        }

        [NonAction]
        public void ResponseWelcome(string text)
        {
            _logger.Information(text);
        }
    }
}
