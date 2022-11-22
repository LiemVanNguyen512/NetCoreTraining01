using Infrastructure.Domains;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Extensions
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var xRequestID = context.Request.Headers["X-Request-ID"];
                if (string.IsNullOrEmpty(xRequestID))
                {
                    xRequestID = Guid.NewGuid().ToString().Replace("-", "");
                    context.Request.Headers["X-Request-ID"] = xRequestID;
                }
                _logger.LogInformation(xRequestID);
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }           
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";

            var code = 99;
            var resultException = new BaseResponseModel
            {
                Status = code,
                Message = e.Message,
                Source = e.Source,
                XRequestId = context.Request.Headers["X-Request-ID"]
            };
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            _logger.LogInformation(message: e.Message);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(resultException));
        }
    }
}
