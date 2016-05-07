using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;
using Cadmus.ParameterEditor.Framework.Commands;
using Cadmus.Parametrizer;
using Cadmus.VisualFoundation.Framework.Commands;

namespace Cadmus.ParameterEditor.Framework
{
    public static class OperationExtensions
    {
        public static ICommand ToCommand(this Operation operation, ILogger logger, ConfigManager config)
        {
            var runOp = operation as RunOperation;
            if (runOp != null)
                return RunProcessCommand.FromOperation(runOp, logger, config);
            return new EmptyCommand();
        }
    }
}
