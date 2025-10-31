namespace LoggingService.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

public class LoggingActionFilter: IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        Log.Information("➡️ {Action} | Args: {@Args}",
            context.ActionDescriptor.DisplayName,
            context.ActionArguments);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception == null)
        {
            Log.Information("✅ Completed {Action}", context.ActionDescriptor.DisplayName);
        }
        
    }
}