using KyleTanczos.TestKyle.Sessions.Dto;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Layout
{
    public class FooterViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }

        public string GetProductNameWithEdition()
        {
            var productName = "TestKyle";

            if (LoginInformations.Tenant != null && LoginInformations.Tenant.EditionDisplayName != null)
            {
                productName += " " + LoginInformations.Tenant.EditionDisplayName;
            }

            return productName;
        }
    }
}