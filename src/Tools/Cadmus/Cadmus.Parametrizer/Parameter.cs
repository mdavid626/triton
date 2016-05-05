using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Cadmus.Parametrizer.Options;

namespace Cadmus.Parametrizer
{
    public class Parameter
    {
        public Parameter()
        {
            Lookups = new List<LookupItemInfo>();
            Steps = new List<string>();
        }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }

        [XmlAttribute("Order")]
        [DefaultValue(0)]
        public int Order { get; set; }

        [XmlAttribute("Editor")]
        [DefaultValue(EditorOptions.NotSet)]
        public EditorOptions Editor { get; set; }

        [XmlAttribute("Encryptable")]
        [DefaultValue(EncryptableOptions.NotSet)]
        public EncryptableOptions Encryptable { get; set; }

        [XmlAttribute("Encrypted")]
        [DefaultValue(EncryptionOptions.NotSet)]
        public EncryptionOptions Encrypted { get; set; }

        [XmlIgnore]
        public Configuration ValueComesFromConfiguration { get; set; }

        [XmlArray]
        [XmlArrayItem("LookupItem", typeof(LookupItemInfo))]
        public List<LookupItemInfo> Lookups { get; set; }

        [XmlIgnore]
        public bool LookupsSpecified => Lookups.Any();

        [XmlElement("Step")]
        public List<string> Steps { get; set; }

        public bool StepsSpecified => Steps.Any();

        public override string ToString()
        {
            return Title ?? Name;
        }
    }
}
