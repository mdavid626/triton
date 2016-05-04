using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;

namespace Cadmus.VisualFoundation.Framework
{
    public interface IGuiCommand : ICommand
    {
        string Title { get; }
    }
}
