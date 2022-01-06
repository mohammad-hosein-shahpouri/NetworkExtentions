namespace NetworkExtentions.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AjaxOnlyAttribute : ActionFilterAttribute
{
    private readonly string ajaxHeaderKey = "x-requested-with";
    private readonly string ajaxHeaderValue = "XMLHttpRequest";

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        ///x-requested-with: XMLHttpRequest
        var headerValue = filterContext.HttpContext.Request.Headers[ajaxHeaderKey].ToString().ToLower();
        if (headerValue == ajaxHeaderValue.ToLower()) base.OnActionExecuting(filterContext);
        else filterContext.Result = new ForbidResult();
    }
}