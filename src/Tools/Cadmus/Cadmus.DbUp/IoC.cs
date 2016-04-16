using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.DbUp
{
    public class IoC
    {
        public static Func<Assembly> GetDbRunAssembly = () => Assembly.GetExecutingAssembly();
        public static Func<string> GetConnectionName = () => "DefaultConnection";
    }
}
