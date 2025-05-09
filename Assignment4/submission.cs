using System;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Program
    {
        // URLs for the XML and XSD files (host these files at your public ASU web page or GitHub Pages)
        public static string xmlURL = "https://mattcarlson22.github.io/cse445_a4/Assignment4/Hotels.xml";       // Q1.2
        public static string xmlErrorURL = "https://mattcarlson22.github.io/cse445_a4/Assignment4/HotelsErrors.xml"; // Q1.3
        public static string xsdURL = "https://mattcarlson22.github.io/cse445_a4/Assignment4/Hotels.xsd";         // Q1.1

        public static void Main(string[] args)
        {
            // Validate the correct XML file against its XSD
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine("Validation result for valid XML:");
            Console.WriteLine(result);
            Console.WriteLine();

            // Validate the error-injected XML file (should produce error messages)
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine("Validation result for invalid XML:");
            Console.WriteLine(result);
            Console.WriteLine();

            // Convert the valid XML file to JSON format
            result = Xml2Json(xmlURL);
            Console.WriteLine("JSON conversion output:");
            Console.WriteLine(result);
        }

        // Q2.1: Validates the XML file against the provided XSD schema.
        // Returns "No Error" if valid; otherwise, returns a string with error details.
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            string validationErrors = "";
            try
            {
                // Create an XmlSchemaSet and add the schema from the provided URL.
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(null, xsdUrl);

                // Configure the XmlReader settings for validation.
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemas;
                settings.ValidationEventHandler += (sender, e) =>
                {
                    // Append error details with line and position information.
                    validationErrors += $"Line {e.Exception.LineNumber}, Position {e.Exception.LinePosition}: {e.Message}\n";
                };

                // Create an XmlReader with the validation settings and read through the XML.
                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }
                }
            }
            catch (Exception ex)
            {
                // In case of exceptions, return the exception message.
                return "Exception: " + ex.Message;
            }

            return string.IsNullOrEmpty(validationErrors) ? "No Error" : validationErrors;
        }

        // Q2.2: Converts the XML file into JSON format.
        // The returned JSON must be deserializable by Newtonsoft.Json (JsonConvert.DeserializeXmlNode).
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlUrl);
                // Remove XML declaration if present
                if (doc.FirstChild is XmlDeclaration)
                {
                    doc.RemoveChild(doc.FirstChild);
                }
                // Convert the XML to JSON and include the root element ("Hotels")
                string jsonText = JsonConvert.SerializeXmlNode(doc.DocumentElement, Newtonsoft.Json.Formatting.Indented, false);
                // Replace attribute prefix "@" with "_" to match the expected JSON format
                jsonText = jsonText.Replace("\"@", "\"_");
                return jsonText;
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message;
            }
}


    }
}
