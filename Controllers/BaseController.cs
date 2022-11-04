using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pyfa.auth.libs;
using System.Text;
using Npgsql;
using System.Data;
using System.Dynamic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Data.SqlClient;
using System.Globalization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace pyfa.auth.Controllers
{
    public class BaseController : Controller
    {
        private lDbConn dbconn = new lDbConn();

        public string getConnection(string spname, params string[] list)
        {
            //var retObject = new List<dynamic>();
            string retObject = "";
            var parameter = spname;
            if (list != null && list.Count() > 0)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    parameter += ";" + list[i];
                }
            }
            retObject = getDataFromApi(parameter);
            return retObject;
        }

        public string getDataFromApi(string parameter)
        {
            var conn = dbconn.domainGetApi("");
            WebRequest request = WebRequest.Create(conn + parameter);
            WebResponse response = request.GetResponseAsync().Result;
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            return responseFromServer;
        }
        public string execExtAPIGetWithToken(string api, string path, string credential)
        {
            var WebAPIURL = dbconn.domainGetApi2(api);
            string requestStr = WebAPIURL + path;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", credential);
            HttpResponseMessage response = client.GetAsync(requestStr).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }



        public List<dynamic> getDataToObject(string spname, params string[] list)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfahome");


            var conn = dbconn.constringList(provider, cstrname);
            StringBuilder sb = new StringBuilder();
            SqlConnection nconn = new SqlConnection(conn);
            var retObject = new List<dynamic>();

            try
            {
                nconn.Open();
                SqlCommand cmd = new SqlCommand(spname, nconn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (list != null && list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        var pars = item.Split(',');

                        if (pars.Count() > 2)
                        {
                            if (pars[2] == "i")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                            }
                            else if (pars[2] == "s")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                            }
                            else if (pars[2] == "d")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(pars[0], pars[1]);
                            }
                        }
                        else if (pars.Count() > 1)
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[0]);
                        }
                    }
                }

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    return retObject;
                }

                retObject = GetDataObjSqlsvr(dr);

                nconn.Close();
                return retObject;
            }
            catch (Exception ex)
            {
                dynamic DyObj = new ExpandoObject();
                DyObj.success = false;
                DyObj.message = ex.Message;
                retObject.Add(DyObj);

                return retObject;
            }
        }

        public List<dynamic> getDynamicDataToObject(string spname,string parameter)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfahome");

            var conn = dbconn.constringList(provider, cstrname);
            StringBuilder sb = new StringBuilder();
            SqlConnection nconn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            var retObject = new List<dynamic>();

            try
            {
                nconn.Open();                
                var data = parameter.Split('|');
                if (data.Count() > 1)
                {
                    for (int i = 0; i < data.Count() - 1; i++)
                    {
                        var pars = data[i].Split(',');
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                }

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    nconn.Close();
                    SqlConnection.ClearPool(nconn);
                    return retObject;
                }

                retObject = GetDataObjSqlsvr(dr);
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                SqlConnection.ClearPool(nconn);
                return retObject;
            }
            catch (Exception ex)
            {
                dynamic DyObj = new ExpandoObject() ;
                DyObj.success = false;
                DyObj.message = ex.Message;
                retObject.Add(DyObj);
                nconn.Close();
                SqlConnection.ClearPool(nconn);
                return retObject;
            }
            
        }

        public List<dynamic> getDynamicDataToObjectv2(string spname, string parameter)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfahome");

            var conn = dbconn.constringList(provider, cstrname);
            StringBuilder sb = new StringBuilder();
            SqlConnection nconn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            var retObject = new List<dynamic>();

            try
            {
                nconn.Open();
                var data = parameter.Split('^');
                if (data.Count() > 1)
                {
                    for (int i = 0; i < data.Count() - 1; i++)
                    {
                        var pars = data[i].Split(',');
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                }

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    nconn.Close();
                    SqlConnection.ClearPool(nconn);
                    return retObject;
                }

                retObject = GetDataObjSqlsvr(dr);
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                SqlConnection.ClearPool(nconn);
                return retObject;
            }
            catch (Exception ex)
            {
                dynamic DyObj = new ExpandoObject();
                DyObj.success = false;
                DyObj.message = ex.Message;
                retObject.Add(DyObj);
                nconn.Close();
                SqlConnection.ClearPool(nconn);
                return retObject;
            }

        }

        protected List<dynamic> GetDataObj(NpgsqlDataReader dr)
        {
            var retObject = new List<dynamic>();
            while (dr.Read())
            {
                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    dataRow.Add(
                           dr.GetName(i),
                           dr.IsDBNull(i) ? null : dr[i] // use null instead of {}
                   );
                }
                retObject.Add((ExpandoObject)dataRow);
            }

            return retObject;
        }

        public void execSqlWithSplitSemicolon(string spname, params string[] list)
        {
            var conn = dbconn.conStringLog();
            string message = "";
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            nconn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(';');

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }
            try
            {
                cmd.ExecuteNonQuery();
                message = "success";
            }
            catch (NpgsqlException e)
            {
                message = e.Message;
            }
            finally
            {
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                NpgsqlConnection.ClearPool(nconn);
            }
            //return message;
        }

        public List<dynamic> ExecSqlWithReturnCustomSplit(string strname, string cstsplit, string spname, params string[] list)
        {
            var retObject = new List<dynamic>();
            StringBuilder sb = new StringBuilder();
            var provider = dbconn.sqlprovider();

            var conn = dbconn.constringList(provider,strname);

            SqlConnection nconn = new SqlConnection(conn);
            nconn.Open();
            SqlCommand cmd = new SqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(cstsplit);

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "dt")
                        {
                            cmd.Parameters.AddWithValue(pars[0], DateTime.ParseExact(pars[1], "yyyy-MM-dd", CultureInfo.InvariantCulture));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr == null || dr.FieldCount == 0)
            {
                nconn.Close();
                SqlConnection.ClearPool(nconn);
                return retObject;
            }

            retObject = GetDataObjSqlsvr(dr);
            nconn.Close();
            SqlConnection.ClearPool(nconn);


            return retObject;
        }


        public List<dynamic> GetDataObjSqlsvr(SqlDataReader dr)
        {
            var retObject = new List<dynamic>();
            while (dr.Read())
            {
                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    dataRow.Add(
                           dr.GetName(i),
                           dr.IsDBNull(i) ? null : dr[i] // use null instead of {}
                   );
                }
                retObject.Add((ExpandoObject)dataRow);
            }

            return retObject;
        }

        public string execExtAPIPostWithToken(string path, string json, string credential)
        {
            //var WebAPIURL = dbconn.domainGetApi(api);
            string requestStr =  path;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", credential);
            var contentData = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            //contentData.Headers.Add("Authorization", credential);   

            HttpResponseMessage response = client.PostAsync(requestStr, contentData).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }
    }
}
