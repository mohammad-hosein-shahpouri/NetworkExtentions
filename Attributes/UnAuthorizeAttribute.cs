namespace NetworkExtentions.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UnAuthorizeAttribute : ActionFilterAttribute
{
    /// <summary>
    /// This Properties Should Be Filled:
    /// Url Property
    /// Or
    /// Action and Controller Properties (Area is not Required)
    /// </summary>

    private int? statusCode { get; set; }
    private string? url { get; set; }
    private string? area { get; set; }
    private string? controller { get; set; }
    private string? action { get; set; }

    /// <summary>
    /// You Can Use Attribute by Filling Properties
    /// Or You Can Use Constructors .
    /// To Use Constructor to Set a Url to Redirect:
    /// [UnAuthorize("https://github.com/mohammad-hosein-shahpouri")] ,
    /// Or You Can Use Constructor to Set a Route to Redirect:
    /// [UnAuthorize("{Controller Name},{Action Name},{Area Name}")]
    /// Area Name is Not Required and default value is null .
    /// If you fill none of Properties above, Action or Controller Redirects to /home/index .
    /// </summary>
    public UnAuthorizeAttribute()
    {
    }

    public UnAuthorizeAttribute(int statusCode) => this.statusCode = statusCode;

    public UnAuthorizeAttribute(string url) => this.url = url;

    public UnAuthorizeAttribute(string controller, string action, string? area = null)
    {
        this.area = area ?? "";
        this.controller = controller;
        this.action = action;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!filterContext.HttpContext.User.Identity!.IsAuthenticated) base.OnActionExecuting(filterContext);
        else if (statusCode is not null) filterContext.Result = new StatusCodeResult(statusCode.Value);
        else if (url is not null) filterContext.Result = new RedirectResult(url);
        else if (controller != null && action != null) filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary(
                new { area = area ?? string.Empty, controller = controller, action = action }));
        else filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary(
                new { controller = "Home", action = "Index" }));
    }
}