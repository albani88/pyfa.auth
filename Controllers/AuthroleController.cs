using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using pyfa.auth.libs;
using pyfa.auth.Controllers;
using CustomTokenAuthProvider;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Net.Http;


namespace pyfa.auth.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("pyfaauth/[controller]")]
    public class AuthroleController : Controller
    {
        private BaseController bc = new BaseController();
        private lConvert lc = new lConvert();
        private lMessage mc = new lMessage();
        private lServiceLogs lsl = new lServiceLogs();
        private lDbConn dbconn = new lDbConn();
        private lDataLayer ldl = new lDataLayer();
        private lPgsqlMapping lgsql = new lPgsqlMapping();
        private TokenController tc = new TokenController();

        [HttpPost("GetAccesslevel")]
        public JObject GetAccesslevel([FromBody] JObject json)
        {
            var data = new JObject();
            try
            {

                var dtReturn = ldl.accesslevelmenulist(json);
                data.Add("status", mc.GetMessage("api_output_ok"));
                data.Add("message", mc.GetMessage("process_success"));
                data.Add("data", dtReturn);
            }
            catch (Exception ex)
            {
                data = new JObject();
                data.Add("status", mc.GetMessage("execdb_failed"));
                data.Add("message", mc.GetMessage("output_field_not_req"));
            }
            return data;
        }
    }
}
