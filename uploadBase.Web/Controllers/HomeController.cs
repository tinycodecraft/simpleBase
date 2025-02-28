using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using uploadBase.Web.Data;
using uploadBase.Web.Resources;

namespace uploadBase.Web.Controllers
{
    [Route("[controller]")]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly RequestLocalizationOptions localoptions;

        public HomeController(ILogger<HomeController> mlogger, IStringLocalizer<SharedResource> mlocalizer,IOptions<RequestLocalizationOptions> mlocaloptions)
        {
            logger = mlogger;
            localizer = mlocalizer;
            localoptions = mlocaloptions.Value;
        }
        [Route("[action]")]
        [Route("")]
        public IActionResult Index(IndexModel model)
        {
            model.ListOfLanguages = localoptions.SupportedUICultures!.Select(c => new SelectListItem
            {
                Value = c.Name,
                Text = c.Name
            }).ToList();
            model.SelectedLanguage = model.SelectedLanguage ?? localoptions.DefaultRequestCulture.Culture.Name;

            return View(model);
        }
    }
}
