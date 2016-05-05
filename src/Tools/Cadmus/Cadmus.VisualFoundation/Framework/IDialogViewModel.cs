using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.VisualFoundation.Framework
{
    public interface IDialogViewModel
    {
        bool? DialogResult { get; }

        Action Close { get; set; }
    }
}
