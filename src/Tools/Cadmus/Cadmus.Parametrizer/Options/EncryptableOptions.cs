using System.Xml.Serialization;

namespace Cadmus.Parametrizer.Options
{
    public enum EncryptableOptions
    {
        [XmlEnum("NotSet")]
        NotSet,
        [XmlEnum("Yes")]
        Yes,
        [XmlEnum("No")]
        No,
    }
}
