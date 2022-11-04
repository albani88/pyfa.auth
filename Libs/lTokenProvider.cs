using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pyfa.auth.Controllers;
using Newtonsoft.Json.Linq;
using pyfa.auth.libs;
using System.Data.SqlClient;
using System.Data;



namespace pyfa.auth.libs
{
    public class lTokenProvider
    {
        private BaseController bc = new BaseController();
        private lConvert lc = new lConvert();
        private lMessage mc = new lMessage();
        private lData oDat = new lData();
        private lDbConn dbconn = new lDbConn();
  

        public List<dynamic> getAllApiCredential()
        {

            var retObject = new List<dynamic>();
            string spname = "api_getlistapicredential";
            retObject = bc.getDataToObject(spname);
            return retObject;
        }

        public List<dynamic> getcheckstatus(string username)
        {
            var retObject = new List<dynamic>();
            string spname = "api_checkstatus";
            string p1 = "@p_usrname," + lc.TdesEncrypt("idxpartners", username) + ",s";
            retObject = bc.getDataToObject(spname, p1);
            return retObject;
        }

        public List<dynamic> getcheckExpirationtoken(string code)
        {
            var retObject = new List<dynamic>();
            string spname = "api_checkExpirationtoken";
            string p1 = "@p_code," + code + ",s";
            retObject = bc.getDataToObject(spname, p1);
            return retObject;
        }

        public List<dynamic> getdateapicredential(string id)
        {
            var retObject = new List<dynamic>();
            string spname = "api_getDetailapicredential";
            string p1 = "@p_id," + Int32.Parse(id.ToString()) + ",i";
            retObject = bc.getDataToObject(spname, p1);
            return retObject;
        }


        public string insertAPICredential(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfahome");
            
            string strout = "";
            var conn = dbconn.constringList(provider,cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                //insert role log with previos value from retobject1
                SqlCommand cmd = new SqlCommand("api_insertapicredential", connection, trans);
                cmd.Transaction = trans;
                cmd.Parameters.AddWithValue("@usr_name", lc.TdesEncrypt("idxpartners", json["usrname"].ToString()));
                cmd.Parameters.AddWithValue("@password", lc.TdesEncrypt("idxpartners", json["password"].ToString()));
                cmd.Parameters.AddWithValue("@senderip", json["senderip"].ToString());
                cmd.Parameters.AddWithValue("@checkip", json["checkip"].ToString());
                cmd.Parameters.AddWithValue("@logid", json["logid"].ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

        public string updateAPICredential(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfahome");

            string strout = "";
            var conn = dbconn.constringList(provider,cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                //insert role log with previos value from retobject1
                SqlCommand cmd = new SqlCommand("api_updateapicredential", connection, trans);
                cmd.Transaction = trans;

                cmd.Parameters.AddWithValue("@id", Int32.Parse(json["id"].ToString()));
                cmd.Parameters.AddWithValue("@usr_name", lc.TdesEncrypt("idxpartners", json["usrname"].ToString()));
                cmd.Parameters.AddWithValue("@password", lc.TdesEncrypt("idxpartners", json["password"].ToString()));
                cmd.Parameters.AddWithValue("@senderip", json["senderip"].ToString());
                cmd.Parameters.AddWithValue("@checkip", json["checkip"].ToString());
                cmd.Parameters.AddWithValue("@logid", json["logid"].ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }


        public string deleteAPICeredential(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfahome");

            string strout = "";
            var conn = dbconn.constringList(provider,cstrname);
            SqlTransaction trans;
            SqlConnection connection = new SqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {


                JArray jaData = new JArray();

                SqlCommand cmd = new SqlCommand("delete_APICredential", connection, trans);
                cmd.Transaction = trans;
                cmd.Parameters.AddWithValue("@p_id", Int32.Parse(json["id"].ToString()));
                cmd.Parameters.AddWithValue("@logid", json["logid"].ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                trans.Commit();
                strout = "success";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }
            connection.Close();
            SqlConnection.ClearPool(connection);
            return strout;
        }

    }
}
