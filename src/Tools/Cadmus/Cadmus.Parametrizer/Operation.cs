using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadmus.Parametrizer
{
    public class Operation
    {
        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }
    }
}
