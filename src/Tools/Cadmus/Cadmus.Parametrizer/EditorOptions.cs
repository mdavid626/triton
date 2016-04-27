using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadmus.Parametrizer
{
    public enum EditorOptions
    {
        [XmlEnum("NotSet")]
        NotSet,
        [XmlEnum("Text")]
        Text,
        [XmlEnum("Hidden")]
        Hidden,
        [XmlEnum("TrueFalse")]
        TrueFalse,
        [XmlEnum("Password")]
        Password,
        [XmlEnum("Lookup")]
        Lookup,
        [XmlEnum("FolderPicker")]
        FolderPicker,
        [XmlEnum("MultiLine")]
        MultiLine,
        [XmlEnum("GuidGenerator")]
        GuidGenerator,
        [XmlEnum("ConnectionString")]
        ConnectionString,
        [XmlEnum("Credentials")]
        Credentials,
        [XmlEnum("MachineDecryptionKey")]
        MachineDecryptionKey,
        [XmlEnum("MachineValidationKey")]
        MachineValidationKey
    }
}
