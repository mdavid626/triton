using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadmus.Parametrizer
{
    public class Configuration
    {
        public Configuration()
        {
            Parameters = new List<Parameter>();
            Steps = new List<Step>();
        }

        [XmlArray]
        [XmlArrayItem("Parameter", typeof(Parameter))]
        public List<Parameter> Parameters { get; set; }

        [XmlIgnore]
        public bool ParametersSpecified => Parameters.Any();

        [XmlArray]
        [XmlArrayItem("Step", typeof(Step))]
        public List<Step> Steps { get; set; }

        [XmlIgnore]
        public bool StepsSpecified => Steps.Any();
    }
}
