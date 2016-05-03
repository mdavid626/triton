using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadmus.Parametrizer
{
    public class Step
    {
        public Step()
        {
            Operations = new List<Operation>();
        }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlArray]
        [XmlArrayItem("RunOperation", typeof(RunOperation))]
        public List<Operation> Operations { get; set; }
    }
}
