namespace LoggingService.Interceptors;
using Castle.DynamicProxy;
using Serilog;

public class LoggingInterceptor: IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        var method = invocation.Method.Name;
        Log.Information("Calling {Method} with args {@Args}", method, invocation.Arguments);
        
        invocation.Proceed();
        Log.Information("Completed {Method} -> {@Return}", method, invocation.ReturnValue);
    }
}