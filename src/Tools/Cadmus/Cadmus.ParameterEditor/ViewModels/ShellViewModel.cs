using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.ParameterEditor.Framework;
using Cadmus.ParameterEditor.Interfaces;

namespace Cadmus.ParameterEditor.ViewModels
{
    public class ShellViewModel : ViewModelBase, IShell
    {
        public string Test
        {
            get { return "hellooo"; }
        }
    }
}
