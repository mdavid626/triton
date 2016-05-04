using System.Xml.Serialization;

namespace Cadmus.Parametrizer
{
    public class RunOperation : Operation
    {
        [XmlAttribute("ExecutablePath")]
        public string ExecutablePath { get; set; }

        [XmlAttribute("Arguments")]
        public string Arguments { get; set; }

        [XmlIgnore]
        public string WorkingFolder { get; set; }
    }
}