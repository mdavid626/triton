using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadmus.Parametrizer
{
    public enum QuoteOptions
    {
        [XmlEnum("NotSet")]
        NotSet,
        [XmlEnum("No")]
        No,
        [XmlEnum("YesSimple")]
        YesSimple,
        [XmlEnum("YesDouble")]
        YesDouble
    }
}
