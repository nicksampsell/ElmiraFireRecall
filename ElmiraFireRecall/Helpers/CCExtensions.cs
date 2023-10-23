using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ElmiraFireRecall.Helpers
{
    public static class CCExtensions
    {
        public static string? ActiveClass(this IHtmlHelper htmlHelper, string? controllers = null, string? actions = null, string? cssClass = "active")
        {
            var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;
            var currentAction = htmlHelper?.ViewContext.RouteData.Values["action"] as string;

            var acceptedControllers = (controllers ?? currentController ?? "").Split(',');
            var acceptedActions = (actions ?? currentAction ?? "").Split(',');

            return acceptedControllers.Contains(currentController) && acceptedActions.Contains(currentAction) ? cssClass : "";
        }

        public static string? LastUsedTab(this IHtmlHelper htmlHelper, string? tabName, string? cssClass = "active")
        {
            return ((string)htmlHelper.ViewContext.TempData["TabName"] == tabName) ? cssClass : "";

        }
    }
}
