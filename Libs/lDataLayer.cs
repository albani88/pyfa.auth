
using pyfa.auth.libs;
using pyfa.auth.Controllers;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace pyfa.auth.libs
{
    public class lDataLayer
    {
        private lDbConn dbconn = new lDbConn();
        private lConvert lc = new lConvert();
        private BaseController bc = new BaseController();

        public JArray accesslevelmenulist(JObject json)
        {
            var jaReturn = new JArray();
            var joReturn = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var split = ",";
            string spname = "DashboardMenu";
            string p1 = "@Email" + split + json.GetValue("email").ToString() + split + "s";
            string p2 = "@App_Id" + split + json.GetValue("app_id").ToString() + split + "s";
            retObject = bc.getDataToObject(spname, p1, p2);
            jaReturn = lc.convertDynamicToJArray(retObject);
            return jaReturn;
        }
    }
}
