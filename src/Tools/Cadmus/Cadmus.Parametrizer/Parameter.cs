using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadmus.Parametrizer
{
    public class Parameter
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }

        [XmlAttribute("Description")]
        public string Value { get; set; }

        [XmlAttribute("Order")]
        [DefaultValue(0)]
        public int Order { get; set; }

        [XmlAttribute("Editor")]
        [DefaultValue(EditorOptions.NotSet)]
        public EditorOptions Editor { get; set; }

        [XmlAttribute("Quote")]
        [DefaultValue(QuoteOptions.NotSet)]
        public QuoteOptions Quote { get; set; }

        [XmlAttribute("Encryptable")]
        [DefaultValue(EncryptableOptions.NotSet)]
        public EncryptableOptions Encryptable { get; set; }

        [XmlAttribute("Encryption")]
        [DefaultValue(EncryptionOptions.NotSet)]
        public EncryptionOptions Encryption { get; set; }
    }
}
