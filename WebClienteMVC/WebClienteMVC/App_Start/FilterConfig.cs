using System.Web;
using System.Web.Mvc;

namespace WebClienteMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //SEGURIDAD GLOBAL
            filters.Add(new AuthorizeAttribute());
        }
    }
}
