using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elearn.Pages.Course
{
    public class logoutModel : PageModel
    {
        public IActionResult OnGet()
        {

            HttpContext.Session.Clear();


            TempData["SuccessMessage"] = "You have been logged out successfully.";


            return RedirectToPage("/login");
        }
    }
}