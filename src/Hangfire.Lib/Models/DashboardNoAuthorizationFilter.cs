using Hangfire.Dashboard;

namespace Hangfire.Extensions
{
    public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext dashboardContext) => true;
    }

}
