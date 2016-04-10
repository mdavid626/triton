using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation.Metadata;

namespace Cadmus.Foundation
{
    public class AppVersionInfo
    {
        public string GetCurrentVersion()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetCustomAttribute<AppVersionAssembly>() != null);
            var attr = assembly?.GetCustomAttributes<AssemblyInformationalVersionAttribute>().FirstOrDefault();
            return attr?.InformationalVersion;
        }
    }
}
