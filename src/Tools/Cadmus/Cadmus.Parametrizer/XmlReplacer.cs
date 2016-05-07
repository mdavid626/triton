using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Cadmus.Parametrizer
{
    public class XmlReplacer
    {
        public void Replace(string path, string match, string newValue)
        {
            var xml = XDocument.Load(path);

            var allNamespaces = xml.XPathSelectElements("//*")
                .Select(e => e.Name.NamespaceName)
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct()
                .OrderBy(e => e);

            var nsm = new XmlNamespaceManager(new NameTable());
            int i = 0;

            foreach (var ns in allNamespaces)
            {
                nsm.AddNamespace("ns" + i++, ns);
            }

            var navigator = (IEnumerable<object>)xml.XPathEvaluate(match, nsm);

            var fileChanged = false;
            foreach (var xItem in navigator)
            {
                var valueChange = false;
                var xElement = xItem as XElement;
                if (xElement != null && xElement.Value != newValue)
                {
                    xElement.Value = newValue ?? "";
                    valueChange = true;
                }

                var xAttr = xItem as XAttribute;
                if (xAttr != null && xAttr.Value != newValue)
                {
                    xAttr.Value = newValue ?? "";
                    valueChange = true;
                }

                if (valueChange)
                {
                    fileChanged = true;
                }
            }

            if (fileChanged)
            {
                Save(xml, path);
            }
        }

        private void Save(XDocument xml, string path)
        {
            var settings = new XmlWriterSettings();
            if (xml.Declaration != null && xml.Declaration.Encoding != null)
            {
                settings.Encoding = Encoding.GetEncoding(xml.Declaration.Encoding);
            }
            settings.Indent = true;
            settings.OmitXmlDeclaration = xml.Declaration == null;
            using (var xw = XmlWriter.Create(path, settings))
            {
                xml.Save(xw);
            }
        }
    }
}
