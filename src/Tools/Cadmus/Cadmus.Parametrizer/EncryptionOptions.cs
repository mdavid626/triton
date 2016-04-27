using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadmus.Parametrizer
{
    public enum EncryptionOptions
    {
        [XmlEnum("NotSet")]
        NotSet,
        [XmlEnum("Yes")]
        Yes,
        [XmlEnum("No")]
        No
    }
}
