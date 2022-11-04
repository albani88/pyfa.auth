using pyfa.auth.Controllers;
using pyfa.auth.libs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pyfa.auth.libs
{
    public class lData
    {
        private BaseController bc = new BaseController();
        private lDbConn dbconn = new lDbConn();
        private lConvert lc = new lConvert();


        public bool CheckCycle(string runningtime)
        {
            var valReturn = true;
            string[] runningtimesnext;

            try
            {
                runningtimesnext = runningtime.Split("-");
                var strtime = Convert.ToDateTime(runningtimesnext[0].ToString());
                var endtime = Convert.ToDateTime(runningtimesnext[1].ToString());
                var today = DateTime.Now;
                var currentTime = Convert.ToDateTime(today.ToString("yyyy-MM-dd HH:mm"));
                if (strtime < currentTime && currentTime < endtime)
                {
                    valReturn = true;
                }
                else
                {
                    valReturn = false;
                }
               
            }
            catch (Exception ex)
            {
                valReturn = false;
            }

            return valReturn;

        }
    }
}
