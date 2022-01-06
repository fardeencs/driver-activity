using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace DriverActivityWeb.Helper
{
    /*public class ValidateFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
            //model valid not pass  
            if (!context.ModelState.IsValid)
            {
                var entry = context.ModelState.Values.FirstOrDefault();

                var message = entry.Errors.FirstOrDefault().ErrorMessage;

                //modify the result  
                context.Result = new OkObjectResult(new
                {
                    code = -1,
                    data = new JObject(),
                    message = message,
                });
            }
        }
    }*/

    public class ValidationFilterAttribute : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            /*var param = context.ActionArguments.SingleOrDefault(p => p.Value is IEntity);
            if (param.Value == null)
            {
                context.Result = new BadRequestObjectResult("Object is null");
                return;
            }*/

            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
                //context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
