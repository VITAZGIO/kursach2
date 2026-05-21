using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ElectroWarehouse.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();

            if (controller != "Account" &&
                HttpContext.Session.GetString("UserLogin") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }

            base.OnActionExecuting(context);
        }
    }
}