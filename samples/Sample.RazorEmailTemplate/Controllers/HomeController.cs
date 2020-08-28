using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.RazorEmailTemplate.Emails;
using Sharp.RazorToString;

namespace Sample.RazorEmailTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly RazorToStringRenderer _razor;

        public HomeController(RazorToStringRenderer razor) => _razor = razor;

        [HttpGet("/")]
        public async Task<ActionResult> Get()
        {
            var result = await _razor.RenderAsync(new EmailTemplate
            {
                Content = "Hi Shaheen, to confirm your email please use this link.",
                Url = "https://github.com/xshaheen/RazorToString",
                ViewData =
                {
                    ["Message"] = "This is message"
                }
            });

            return Content($"The email is\n\n{result}");
        }
    }
}