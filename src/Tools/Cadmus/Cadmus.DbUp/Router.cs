using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.DbUp
{
    public class Router
    {
        private ApplicationArguments _arguments;

        public Router(ApplicationArguments arguments)
        {
            _arguments = arguments;
        }

        public bool Route()
        {
            if (_arguments.Help)
            {
                ApplicationArgumentsParser.ShowHelp();
            }
            else if (_arguments.Version)
            {
                
            }
            else if (_arguments.Create)
            {
                
            }
            else if (_arguments.Drop)
            {
                
            }
            else if (_arguments.Upgrade)
            {
                
            }
            else
            {
                ApplicationArgumentsParser.ShowHelp();
            }

            return true;
        }
    }
}
