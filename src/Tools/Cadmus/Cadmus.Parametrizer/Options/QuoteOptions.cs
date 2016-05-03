using System.Xml.Serialization;

namespace Cadmus.Parametrizer.Options
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
