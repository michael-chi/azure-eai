using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;

namespace EAIBackendAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [SwaggerOperation("GetAll")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public string Get(int id)
        {
            return id.ToString();
        }

        // POST api/values
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public string Post([FromBody]string value)
        {
            return "accepted";
        }

        // POST api/values
        [SwaggerOperation("Submit")]
        [Route("api/submit")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public HttpResponseMessage SubmitPO2(HttpRequestMessage request)
        {
#if false
            var content = request.Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            string orderId = "";
            string exception = "";
            dynamic json = null;
            try
            {
                jsonContent = Task.Run( () => request.Content.ReadAsStringAsync()).Result;
                //System.IO.File.WriteAllText(@"./" + Guid.NewGuid().ToString() + ".json", jsonContent);
                //jsonContent = "Order has been processed.";
                json = JsonConvert.DeserializeObject(jsonContent);
                var xml = new XmlDocument();
                xml.LoadXml((string)json.content);
                orderId = $"{xml.SelectSingleNode("//SAPOrder/OrderId")?.Value}|{xml.SelectSingleNode("//OrderId")?.Value}|{xml.SelectSingleNode("//*[local-name()='OrderId']")?.Value}";
            }
            catch(Exception exp)
            {
                exception = exp.Message;
            }
            return request.CreateResponse<string>(HttpStatusCode.OK, $"<Message><OrderNumber>{orderId}</OrderNumber><Status>Accepted</Status><Desc><![CDATA[{jsonContent}]]><Desc><Exception><![CDATA[{exception}]]></Exception></Message>");
#else
            byte[] jsonContent = request.Content.ReadAsByteArrayAsync().Result;
            string jsonText = request.Content.ReadAsStringAsync().Result;
            string orderId = "";
            string exception = "";
            try
            {
                var xml = new XmlDocument();
                xml.LoadXml($"{jsonText}");
                //orderId = $"{xml.DocumentElement.SelectSingleNode("//SAPOrder/OrderId")?.Value}|{xml.SelectSingleNode("//*[local-name()='OrderId']")?.Value}";//|{xml.SelectSingleNode("/*[local-name()='SAPOrder']/[local-name()='OrderId']")?.Value}";
                orderId = $"{xml.DocumentElement.SelectSingleNode("//OrderId")?.InnerText}";//or |{xml.SelectSingleNode("//*[local-name()='OrderId']")?.InnerText};
            }
            catch (Exception exp)
            {
                exception = exp.Message;
            }
            return request.CreateResponse<string>(HttpStatusCode.OK, $"<Message><OrderNumber>{orderId}</OrderNumber><Status>Processing</Status><Desc><![CDATA[{jsonText}]]></Desc><Exception><![CDATA[{exception}]]></Exception></Message>");

#endif
        }
        // POST api/values
        [SwaggerOperation("Submit2")]
        [Route("api/submit2")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public string SubmitPO([FromBody]string value)
        {
            
            var log = Task.Run(() => Request.Content.ReadAsStringAsync()).Result;
            using (var stream = Task.Run(() => Request.Content.ReadAsStreamAsync()).Result)
            {
                using (var sr = new System.IO.StreamReader(stream))
                {
                    var text = sr.ReadToEnd();
                    return $"<Message><Status>Accepted</Status><Desc><![CDATA[{text}|{log}]]><Desc></Message>";
                }
            }
        }
    }
}
