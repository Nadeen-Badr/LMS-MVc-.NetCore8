using Microsoft.AspNetCore.Mvc;

public class LanguageController : Controller
{
    public IActionResult ChangeLanguage(string lang)
    {
        // Set the language cookie
        Response.Cookies.Append("lang", lang, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

        // Redirect to the previous page or the home page
        return Redirect(Request.Headers["Referer"].ToString());
    }
}
