using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    public interface ICommand
    {
        bool CanExecute { get; }

        void Execute();
    }
}
