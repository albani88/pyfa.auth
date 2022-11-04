using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using pyfa.auth.libs;
using pyfa.auth.Controllers;

using System.IO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace pyfa.auth.Controllers
{
    public class TokenController : Controller
    {
        private BaseController bc = new BaseController();
        private lDbConn dbconn = new lDbConn();
        public JObject GetToken(string module)
        {
            JObject jOutput = new JObject();
            var WebAPIURL = dbconn.domainGetApi("urlAPI_auth");
            string requestStr = WebAPIURL + "token";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("username", dbconn.domainGetTokenCredential("UserName"));
            client.DefaultRequestHeaders.Add("password", dbconn.domainGetTokenCredential("Password"));
            var contentData = new StringContent("", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = client.PostAsync(requestStr, contentData).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            jOutput = JObject.Parse(result);
            return jOutput;
        }

        public JObject GetOpenAPITokenScheduler(string Channel)
        {
            JObject joHeader = new JObject();
            JObject jOutput = new JObject();
            string allText = "";
            using (StreamReader sr = new StreamReader("file/headerconfig.json"))
            {
                allText = sr.ReadToEnd();
            }
            joHeader = JObject.Parse(allText);

          
            if (joHeader.ContainsKey(Channel))
            {
                var jaData = JObject.Parse(joHeader.GetValue(Channel).ToString());
                if (jaData.Count > 0)
                {
                    var requestStr = dbconn.domainGetApi("urlAPI_idcopenapi") + "token";
                    //var requestStr = jaData.GetValue("domain").ToString() + "token";
                    var userid = jaData.GetValue("user").ToString();
                    var pass = jaData.GetValue("password").ToString();

                    // string requestStr = WebAPIURL + "token";

                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("username", userid);
                    client.DefaultRequestHeaders.Add("password", pass);
                    client.DefaultRequestHeaders.Add("Channel", Channel);

                    var contentData = new StringContent("", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

                    HttpResponseMessage response = client.PostAsync(requestStr, contentData).Result;
                    string result = response.Content.ReadAsStringAsync().Result;
                    jOutput = JObject.Parse(result);
                   
                }
            }

            return jOutput;


        }


    }
}
