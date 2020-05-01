using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Dashboards;

namespace Mimic.Dashboard
{
    [Weight(-10)]
    public class MimicDashboard : IDashboard
    {
        public string Alias => "Mimic";

        public string[] Sections => new[]
        {
            Umbraco.Core.Constants.Applications.Settings
        };

        public string View => "/App_Plugins/Mimic/index.html";

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();
    }
}
