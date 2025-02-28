using Microsoft.AspNetCore.Mvc.Rendering;

namespace uploadBase.Web.Data
{
    public class IndexModel
    {
        public string? SelectedLanguage { get; set; } = "en";

        public bool IsSubmit { get; set; } = false;

        //view model
        public List<SelectListItem>? ListOfLanguages { get; set; }
    }
}
