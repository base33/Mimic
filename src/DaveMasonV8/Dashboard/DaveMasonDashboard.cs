using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Dashboards;

namespace DaveMasonV8.Dashboard
{
    [Weight(-10)]
    public class DaveMasonDashboard : IDashboard
    {
        public string Alias => "daveMason";

        public string[] Sections => new[]
        {
            Umbraco.Core.Constants.Applications.Settings
        };

        public string View => "/App_Plugins/DaveMason/index.html";

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();
    }
}
