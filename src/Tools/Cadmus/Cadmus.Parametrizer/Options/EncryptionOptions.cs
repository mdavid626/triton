using System.Xml.Serialization;

namespace Cadmus.Parametrizer.Options
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
