
using pyfa.auth.Controllers;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using pyfa.auth.libs;
using System.Dynamic;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Globalization;


namespace pyfa.auth.libs
{
    public class lPgsql
    {
        public  lDbConn dbconn = new lDbConn();
        private BaseController bc = new BaseController();
        private MessageController mc = new MessageController();

     
        //public void execSql(string sql)
        //{
        //    var conn = dbconn.conString();
        //    NpgsqlConnection nconn = new NpgsqlConnection(conn);
        //    nconn.Open();
        //    NpgsqlCommand cmd = new NpgsqlCommand(sql, nconn);
        //    cmd.CommandType = CommandType.Text;
        //    cmd.ExecuteNonQuery();
        //    nconn.Close();
        //    NpgsqlConnection.ClearPool(nconn);
        //}

        public void execSqlV2(string sql)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfahome");
            var conn = dbconn.constringList(provider,cstrname);
            //var conn = dbconn.constringName(cstrname);
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            nconn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, nconn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            nconn.Close();
            NpgsqlConnection.ClearPool(nconn);
        }

        //public string execSqlWithExecption(string sql)
        //{
        //    var conn = dbconn.conString();
        //    string message = "";
        //    NpgsqlConnection nconn = new NpgsqlConnection(conn);
        //    nconn.Open();
        //    NpgsqlCommand cmd = new NpgsqlCommand(sql, nconn);
        //    cmd.CommandType = CommandType.Text;
        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //        message = "success";
        //    }
        //    catch (NpgsqlException e)
        //    {
        //        message = e.Message;
        //    }
        //    finally 
        //    {
        //        if (nconn.State.Equals(ConnectionState.Open))
        //        {
        //            nconn.Close();
        //        }
        //        NpgsqlConnection.ClearPool(nconn);
        //    }
        //    return message;
        //}
        
        public string execStringQuery(string sql)
        {
            string result = "";
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfahome");


            var conn = dbconn.constringList(provider, cstrname);
            //var conn = dbconn.constringName(cstrname);

            SqlConnection nconn = new SqlConnection(conn);
            nconn.Open();
            SqlCommand cmd = new SqlCommand(sql, nconn);
            cmd.CommandType = CommandType.Text;
            try
            {
                cmd.ExecuteNonQuery();
                nconn.Close();
                SqlConnection.ClearPool(nconn);


                //result = mc.GetMessage("execdb_success");
            }
            catch (Exception ex)
            {
                result = mc.GetMessage("execdb_failed");
                nconn.Close();
                SqlConnection.ClearPool(nconn);
            }
            finally
            {
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                SqlConnection.ClearPool(nconn);
            }
            return result;
        }

        public List<dynamic> execStringQuerywithresult(string sql)
        {
            var data = new List<dynamic>();
            string result = "";
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("pyfauser");
            

            var conn = dbconn.constringList(provider, cstrname);
            //var conn = dbconn.constringName(cstrname);

            SqlConnection nconn = new SqlConnection(conn);
            nconn.Open();
            SqlCommand cmd = new SqlCommand(sql, nconn);
            cmd.CommandType = CommandType.Text;
            try
            {
                cmd.ExecuteNonQuery();
                //nconn.Close();
                //SqlConnection.ClearPool(nconn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    return data;
                }

                data = GetDataObjSqlsvr(dr);

                nconn.Close();
                //result = data.ToString();
                //result = mc.GetMessage("execdb_success");
            }
            catch (Exception ex)
            {
               
                data.Add(mc.GetMessage("execdb_failed")) ;
                nconn.Close();;
                SqlConnection.ClearPool(nconn);
            }
            finally
            {
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                SqlConnection.ClearPool(nconn);
            }
            return data;
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
    }
}
