using pyfa.auth.Controllers;
using pyfa.auth.libs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pyfa.auth.libs
{
    public class lServiceLogs
    {
        private BaseController bc = new BaseController();
        private lDbConn dbconn = new lDbConn();
        public void ServiceRecordLogs(string method, string actionpath, string requestform, string result)
        {
            var _path = Path.GetFullPath("Logs");
            string urlPath = "";
            string apimodule = "";

            apimodule = GetApiModule(actionpath);
            urlPath = GetUrlApi(apimodule, actionpath);

            var today = DateTime.Now.ToString("yyyy-MM-dd");
            string filename = _path + "/" + apimodule + "_" + today + ".txt";

            var message = " Method : " + method + "; " + "url : " + urlPath + "; request form : " + requestform + " ; result : " + result;

            string spname = "public.insert_service_log";
            string p1 = "p_module;" + apimodule + ";s";
            string p2 = "p_date;" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ";s";
            string p3 = "p_method;" + method + ";s";
            string p4 = "p_url;" + urlPath + ";s";
            string p5 = "p_reqform;" + (requestform).Replace("\r\n", "") + ";s";
            string p6 = "p_result;" + (result).Replace("\r\n", "") + ";s";


            var retObject = new List<dynamic>();
            bc.execSqlWithSplitSemicolon(spname, p1, p2, p3, p4, p5, p6);

            //if (!System.IO.File.Exists(filename))
            //{
            //    FileStream fs = System.IO.File.Create(filename);
            //    fs.Dispose();
            //    System.IO.File.AppendAllText(filename,
            //      "[ " + DateTime.Now.ToString() + " ]" + message + Environment.NewLine);
            //}
            //else
            //{
            //    System.IO.File.AppendAllText(filename, Environment.NewLine);
            //    System.IO.File.AppendAllText(filename,
            //      "[ " + DateTime.Now.ToString() + " ]" + message + Environment.NewLine);
            //}
        }

        public void ServiceRecordLogsOpenapi(string chennel, string actionpath, string reqbody, string resform, string msg)
        {
            //var _path = Path.GetFullPath("Logs");
             string urlPath = "";
            string apimodule = "";

            apimodule = GetApiModule(actionpath);
           var path = dbconn.GetNotifSetting("path").ToString();

            //urlPath = GetUrlApi(apimodule, actionpath);

            urlPath = path + actionpath;
       
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            //string filename = _path + "/" + apimodule + "_" + today + ".txt";

            string spname = "openapi.insert_openapi_log";
            string p1 = "p_channel;" + chennel + ";s";
            string p2 = "p_datereq;" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ";s";
            string p3 = "p_dateres;" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ";s";
            string p4 = "p_url;" + urlPath + ";s";
            string p5 = "p_body;" + (reqbody).Replace("\r\n", "") + ";s";
            string p6 = "p_resform;" + (resform).Replace("\r\n", "") + ";s";
            string p7 = "p_msg;" + (msg).Replace("\r\n", "") + ";s";

            var retObject = new List<dynamic>();
            bc.execSqlWithSplitSemicolon(spname, p1, p2, p3, p4, p5, p6, p7);


        }

        public void ServiceRecordLogsHitOpenapi(string chennel,string reqdate, string resdate, string actionpath, string reqbody, string resform, string result)
        {
            //var _path = Path.GetFullPath("Logs");
            string urlPath = "";
            string apimodule = "";

            apimodule = GetApiModule(actionpath);
            var path = dbconn.GetNotifSetting("path").ToString();
            urlPath = actionpath;
          

            var today = DateTime.Now.ToString("yyyy-MM-dd");
            //string filename = _path + "/" + apimodule + "_" + today + ".txt";

            
            string spname = "openapi.insert_openapi_log";
            string p1 = "p_channel;" + chennel + ";s";
            string p2 = "p_datereq;" + reqdate + ";s";
            string p3 = "p_dateres;" + resdate + ";s";
            string p4 = "p_url;" + urlPath + ";s";
            string p5 = "p_body;" + (reqbody).Replace("\r\n", "") + ";s";
            string p6 = "p_resform;" + (resform).Replace("\r\n", "") + ";s";
            string p7 = "p_msg;" + (result).Replace("\r\n", "") + ";s";

            var retObject = new List<dynamic>();
            bc.execSqlWithSplitSemicolon(spname, p1, p2, p3, p4, p5, p6, p7);


        }


        public string GetApiModule(string path)
        {
            string strResult = "";
            var _apiPath = path.Split("/");
            if (_apiPath.Length > 2)
            {
                strResult = _apiPath[1].ToString();
            }
            return strResult;
        }

        public string GetUrlApi(string apimodule, string actionpath)
        {
            string strDomain = "";
            string urlPath = "";

            strDomain = GetDomainApi(apimodule);
            var _domain = strDomain.Split("/");
            if (_domain.Length > 3)
            {
                urlPath = _domain[0] + "//" + _domain[2] + actionpath;
            }
            else
            {
                urlPath = actionpath;
            }

            return urlPath;
        }

        public string GetDomainApi(string apimodule)
        {
            string strResult = "";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            strResult = config.GetSection("APISettings:urlAPI_" + apimodule + "").Value.ToString();

            return strResult;
        }        
    }
}
