using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fclp;

namespace Cadmus.DbUp.Interfaces
{
    public interface IApplicationArgumentsParser
    {
        ICommandLineParserResult Parse(string[] args);

        IApplicationArguments Arguments { get; }

        void ShowHelp();

        void RegisterOperation(string name, IOperation instance);

        IOperation CreateOperation();
    }
}
