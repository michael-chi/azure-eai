using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
            var content = request.Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            try
            {
                System.IO.File.WriteAllText(@"./" + Guid.NewGuid().ToString() + ".json", jsonContent);
            }catch(Exception exp)
            {
                jsonContent += "|[Exception]" + exp.Message;
            }
            return request.CreateResponse<string>(HttpStatusCode.OK, $"<Message><Status>Accepted</Status><Desc><![CDATA[{jsonContent}]]><Desc></Message>");
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
