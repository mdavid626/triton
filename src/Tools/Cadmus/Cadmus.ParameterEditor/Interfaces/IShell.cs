using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;

namespace Cadmus.ParameterEditor.Interfaces
{
    public interface IShell
    {
        void Load();

        ILogger Logger { get; }
    }
}
