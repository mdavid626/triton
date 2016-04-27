using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Parametrizer
{
    public class Parameters
    {
        public Parameters()
        {
            ParameterCollection = new List<Parameter>();
        }

        public List<Parameter> ParameterCollection { get; set; }
    }
}
