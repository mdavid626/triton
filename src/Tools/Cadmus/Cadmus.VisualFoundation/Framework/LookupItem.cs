using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.VisualFoundation.Framework
{
    public class LookupItem
    {
        public string Text { get; set; }

        public string Description { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
