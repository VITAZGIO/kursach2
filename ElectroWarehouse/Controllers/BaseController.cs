using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ElectroWarehouse.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();
            var userLogin = HttpContext.Session.GetString("UserLogin");
            var userRole = HttpContext.Session.GetString("UserRole") ?? "User";

            ViewBag.IsAdmin = userRole == "Admin";
            ViewBag.UserRole = userRole;

            if (controller != "Account" && userLogin == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                base.OnActionExecuting(context);
                return;
            }

            if (controller != "Account" && userRole != "Admin" && IsChangingAction(action))
            {
                TempData["Error"] = "У вас нет прав на изменение данных.";
                context.Result = new RedirectToActionResult("Index", controller, null);
                base.OnActionExecuting(context);
                return;
            }

            base.OnActionExecuting(context);
        }

        private static bool IsChangingAction(string? action)
        {
            return string.Equals(action, "Create", StringComparison.OrdinalIgnoreCase)
                || string.Equals(action, "Edit", StringComparison.OrdinalIgnoreCase)
                || string.Equals(action, "Delete", StringComparison.OrdinalIgnoreCase)
                || string.Equals(action, "DeleteConfirmed", StringComparison.OrdinalIgnoreCase);
        }
    }
}
