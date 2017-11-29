using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace cetpaApi.Helpers
{
    public class ExceptionFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                var status = (int)HttpStatusCode.InternalServerError;
                var message = context.Exception.Message;
                //if (message == "403")
                //{
                //    status = 403;
                //    message = "Forbidden";
                //}
                //if (message == "400")
                //{
                //    status = 400;
                //    message = "Parameter Required";
                //}
                context.HttpContext.Response.StatusCode = status;
                var data = new
                {
                    status = status,
                    message = message
                };
                context.Result = new JsonResult(data);
            }
        }
    }
}
