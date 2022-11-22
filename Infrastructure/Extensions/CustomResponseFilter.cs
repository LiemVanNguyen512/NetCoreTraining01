using Infrastructure.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public class CustomResponseFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context){ }

        public void OnResultExecuting(ResultExecutingContext context)
        {

            var mvcObjectResult = context.Result as ObjectResult;
            var XRequestID = context.HttpContext.Request.Headers["X-Request-ID"];

            if (mvcObjectResult != null)
            {
                var baseRespone = new BaseResponseModel();
                var dataOutput = mvcObjectResult.Value;

                //Status is type int?. Will return 200 if Status is null
                var httpStatusCode = mvcObjectResult.StatusCode.GetValueOrDefault(200);
                context.HttpContext.Response.StatusCode = httpStatusCode;
                var resultBadRequest = mvcObjectResult as BadRequestObjectResult;
                if (resultBadRequest != null)
                {

                    var problemDetails = resultBadRequest.Value as ValidationProblemDetails;
                    var errMessage = "";
                    if (problemDetails != null)
                    {
                        var dicErr = problemDetails.Errors.FirstOrDefault();
                        errMessage =  dicErr.Value.FirstOrDefault();
                    }
                    else
                    {
                        errMessage = (resultBadRequest.Value as ProblemDetails)?.Title;
                    }

                    var baseResponseModel = new BaseResponseModel
                    {
                        Status = httpStatusCode,
                        Message = errMessage,
                        XData = null,
                        XRequestId = XRequestID
                    };
                    context.Result = new ObjectResult(baseResponseModel);
                    return;
                }

                var baseResponeTmp = dataOutput as BaseResponseModel;
                if (baseResponeTmp != null)
                {
                    baseRespone = baseResponeTmp;
                    baseRespone.XRequestId = XRequestID;
                }
                else
                {
                    baseRespone = new BaseResponseModel
                    {
                        Status = httpStatusCode,
                        Message = null,
                        XData = dataOutput,
                        XRequestId = XRequestID
                    };
                }
                context.Result = new ObjectResult(baseRespone);
            }
        }
    }
}
