using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadmus.Parametrizer
{
    public class LookupItemInfo
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
