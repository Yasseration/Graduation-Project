using System.Web;
using System.Web.Mvc;

namespace GraduationProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new OutputCacheAttribute() { Duration = 0, VaryByParam = "*", NoStore = true });
            filters.Add(new HandleErrorAttribute());
        }
    }
}
