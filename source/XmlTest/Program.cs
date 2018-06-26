using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonText = System.IO.File.ReadAllText(@".\\test.xml");
            string orderId = "";
            string exception = "";
            try
            {
                var xml = new XmlDocument();
                xml.LoadXml($"{jsonText}");
                orderId = $"{xml.DocumentElement.SelectSingleNode("//OrderId")?.InnerText}";//or |{xml.SelectSingleNode("//*[local-name()='OrderId']")?.InnerText};
            }
            catch (Exception exp)
            {
                exception = exp.Message;
            }

        }
    }
}
