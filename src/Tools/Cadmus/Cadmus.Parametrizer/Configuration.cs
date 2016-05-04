using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadmus.Parametrizer
{
    [Serializable]
    [XmlRoot("Configuration")]
    public class Configuration
    {
        public Configuration()
        {
            Parameters = new List<Parameter>();
            Steps = new List<Step>();
            ChildConfigurations = new List<Configuration>();
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

        [XmlIgnore]
        public string FilePath { get; set; }

        [XmlElement("Parent")]
        public string Parent { get; set; }

        [XmlIgnore]
        public List<Configuration> ChildConfigurations { get; set; }

        [XmlIgnore]
        public bool ForceSave { get; set; }

        public void MergeValues(Configuration configuration)
        {
            ChildConfigurations.Add(configuration);

            foreach (var argParam in configuration.Parameters.Where(p => p.Value != null))
            {
                var param = Parameters.FirstOrDefault(p => p.Name == argParam.Name);
                if (param != null)
                {
                    param.Value = argParam.Value;
                    param.ValueComesFromConfiguration = configuration;
                }
            }
        }
    }
}
