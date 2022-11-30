using AutoMapper;
using EvenBus.Message.IntegrationEvents.Events;
using MassTransit;
using Shared.DTOs.UserDTOs;
using User_service.Services.Interfaces;
using ILogger = Serilog.ILogger;

namespace User_service.IntergrationEvents.EventsHandler
{
    public class EnrolledConsumer : IConsumer<EnrolledEvent>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IMemberService _memberService;

        public EnrolledConsumer(IMapper mapper, ILogger logger, IMemberService memberService)
        {
            _mapper = mapper;
            _logger = logger;
            _memberService = memberService;
        }

        public async Task Consume(ConsumeContext<EnrolledEvent> context)
        {
            var userUpdateDto = _mapper.Map<UpdateUserDto>(context.Message);
            var result = await _memberService.UpdateUserAsync(context.Message.Id, userUpdateDto);
            if (result != null)
            {
                _logger.Information("EnrolledEvent consumed successfully. " +
                "Balance account of {firstName} is updated", result.FirstName);
            }
        }
    }
}
