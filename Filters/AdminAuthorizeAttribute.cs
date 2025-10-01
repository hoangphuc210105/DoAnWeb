using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DoAnWeb.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var adminUser = context.HttpContext.Session.GetString("ADMIN_USER");
            if (string.IsNullOrEmpty(adminUser))
            {
                // Chuyển hướng về trang login nếu chưa đăng nhập
                context.Result = new RedirectToActionResult(
                    "Login",
                    "AdminAccount",
                    new { area = "Admin" });
            }
        }
    }
}
